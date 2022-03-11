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
	public Game()
	{
		if ( IsServer )
		{
			var hud = new GameHud();
			hud.Parent = this;
		}
	}

	/// <summary>
	/// A client has joined the server. Make them a pawn to play with
	/// </summary>
	public override void ClientJoined( Client client )
	{
		base.ClientJoined( client );

		var player = new Player();
		client.Pawn = player;
		player.Respawn();

		var spawnPoints = Entity.All.OfType<SpawnPoint>();
		var randomSpawnPoint = spawnPoints.OrderBy( x => Guid.NewGuid() ).FirstOrDefault();

		if ( randomSpawnPoint is null )
			return;
			
		var tx = randomSpawnPoint.Transform;
		tx.Position = tx.Position + Vector3.Up * 50.0f;
		player.Transform = tx;
	}
}
