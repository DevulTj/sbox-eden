// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

/// <summary>
/// This is your game class. This is an entity that is created serverside when
/// the game starts, and is replicated to the client. 
/// 
/// You can use this to create things like HUDs and declare which player class
/// to use for spawned players.
/// </summary>
public partial class Game : Sandbox.Game
{
	public ClimateAudioManager ClimateAudioManager { get; protected set; }
	public DayNight.DayNightSystem DayNightSystem { get; protected set; }
	public MetricsWebSocketClient MetricsWebSocketClient { get; protected set; }

	public Game()
	{
		if ( IsServer )
		{
			var hud = new GameHud();
			hud.Parent = this;

			ResourceManager = new();
			DayNightSystem = new();

			var metricsConfig = ConfigReader.GetMetricsConfig();
			if ( metricsConfig == null )
			{
				Log.Warning( "Metrics Config is null. Metrics will not be recorded." );
			}
			else
			{
				InitializeWebSocketServer( metricsConfig );
			}
		}

		CacheAssets();
	}

	public override void PostLevelLoaded()
	{
		base.PostLevelLoaded();

		ResourceManager.Initialize();

		if ( IsServer )
			ClimateAudioManager = new();
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client cl )
	{
		base.ClientJoined( cl );

		var player = new Player( cl );
		cl.Pawn = player;
		player.Respawn();

		var spawnPoints = Entity.All.OfType<SpawnPoint>();
		var randomSpawnPoint = spawnPoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( randomSpawnPoint is null )
			return;

		var tx = randomSpawnPoint.Transform;
		tx.Position = tx.Position + Vector3.Up * 50.0f;
		player.Transform = tx;

		ResourceManager.AddEntity( player );
	}

	public override void ClientDisconnect( Client cl, NetworkDisconnectionReason reason )
	{
		if ( cl.Pawn.IsValid() )
			ResourceManager.RemoveEntity( cl.Pawn );

		base.ClientDisconnect( cl, reason );
	}

	private async void InitializeWebSocketServer( MetricsConfig config )
	{
		MetricsWebSocketClient = new MetricsWebSocketClient( config );
		await MetricsWebSocketClient.Connect();

		// Sample message send.
		OutgoingMetricMessage message = new();
		message.PlayerId = "1234";
		message.MetricType = "SomeGayAssMetric";

		await MetricsWebSocketClient.Send( message );
	}
}
