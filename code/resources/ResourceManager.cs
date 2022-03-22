// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eden;

public partial class ResourceManager
{
	/// <summary>
	/// Container for a list of entities we are tracking
	/// </summary>
	protected List<Entity> TrackedEntities { get; set; } = new();
	protected List<ResourceNodeEntity> Resources { get; set; } = new();

	/// <summary>
	/// Max amount of concurrently existing resources per game.
	/// </summary>
	protected virtual int MaxResources => 100;

	/// <summary>
	/// How often the resource manager refreshes
	/// </summary>
	protected virtual int RefreshTimeSeconds => 5;

	/// <summary>
	/// The range away from each tracked entity.
	/// </summary>
	protected virtual float RefreshRange => 512f;

	/// <summary>
	/// Check decay every 60 seconds, as it's expensive
	/// </summary>
	protected virtual float CheckDecayTime => 60f;

	/// <summary>
	/// How long until an individual resource will decay from being isolated.
	/// </summary>
	protected virtual float IndividualDecayTime => 120f;

	/// <summary>
	/// When we can't place something, we'll try to offset the location by a
	/// random amount in this range.
	/// </summary>
	protected virtual int AttemptOffsetMaximum => 400;

	/// <summary>
	/// How far up we will trace from a tracked entity
	/// </summary>
	protected virtual float SkyOffset => 2048f;

	/// <summary>
	/// How far down we will trace to the ground, from the sky
	/// </summary>
	protected virtual float GroundOffset => SkyOffset * 4f;

	/// <summary>
	/// How many extra traces we will run if we can't find a suitable location the first time.
	/// </summary>
	protected virtual int MaxTraceAttempts => 10;

	private TimeSince CheckedDecayingEntities { get; set; } = 0;
	private bool Initialized { get; set; } = false;
	private Task CurrentTask { get; set; }

	public void Initialize()
	{
		if ( Initialized ) return;

		Initialized = true;
		CurrentTask = Refresh();
	}

	public void AddEntity( Entity entity )
	{
		Log.Info( $"ResourceManager: Added entity -> {entity}" );

		TrackedEntities.Add( entity );
	}

	public void RemoveEntity( Entity entity )
	{
		Log.Info( $"ResourceManager: Removed entity -> {entity}" );

		TrackedEntities.Remove( entity );
	}

	protected void RefreshEntity( Entity entity )
	{
		Log.Info( $"ResourceManager: Refreshing entity -> {entity}" );

		var list = Entity.FindInSphere( entity.Position, RefreshRange )
			.OfType<ResourceNodeEntity>()
			.ToList();

		list.ForEach( x =>
		{
			x.LastRefresh = 0;
		} );

		GeneratePoints( entity, entity.Position, 600, 1350, 4, 16 );
	}

	protected Vector3 GeneratePoint( Vector2 origin, float radius )
	{
		var x = Rand.Float() - 0.5f;
		var y = Rand.Float() - 0.5f;

		var magnitude = MathF.Sqrt( x * x + y * y );

		x /= magnitude;
		y /= magnitude;

		return origin += new Vector2( x * radius, y * radius );
	}

	protected virtual bool IsValidHit( TraceResult tr )
	{
		return tr.Hit && tr.Entity is WorldEntity;
	}

	protected TraceResult TryTrace( Vector3 point )
	{
		// try a few times with some variation
		int attemptsLeft = MaxTraceAttempts;

		while ( attemptsLeft > 0 )
		{
			var endPoint = point + Vector3.Down * GroundOffset;
			var tr = Trace.Ray( point, endPoint )
				.WorldAndEntities()
				.HitLayer( CollisionLayer.Water, true )
				.Run();

			// This is cool!
			bool inWater = Map.Physics.IsPointWater( tr.EndPosition );

			if ( !inWater && IsValidHit( tr ) )
				return tr;

			// Pick a random point to test next
			point += Vector3.Right * Rand.Int( -AttemptOffsetMaximum, AttemptOffsetMaximum );
			point += Vector3.Forward * Rand.Int( -AttemptOffsetMaximum, AttemptOffsetMaximum );

			attemptsLeft--;
		}

		return new TraceResult();
	}

	protected void GeneratePoints( Entity entity, Vector2 coordinate, float minRadius, float maxRadius, int minAmount, int maxAmount )
	{
		var amount = Rand.Int( minAmount, maxAmount );

		for ( int i = 0; i < amount; i++ )
		{
			var point = GeneratePoint( coordinate, Rand.Float( minRadius, maxRadius ) );
			point.z = entity.Position.z + 2048f;

			var tr = TryTrace( point );

			bool success = tr.Hit && Resources.Count < MaxResources;
			if ( !success )
			{
				//
			}
			else
			{
				CreateResource( tr.EndPosition, tr.Normal );
			}
		}
	}

	protected void OnResourceDestroyed( ResourceNodeEntity entity )
	{
		Resources.Remove( entity );
	}

	protected void CreateResource( Vector3 point, Vector3 normal )
	{
		var entity = new ResourceNodeEntity();

		// TODO: Be more selective about what assets are spawned where, we're some way away yet.
		var resource = ResourceAsset.All.ElementAt( Rand.Int( ResourceAsset.All.Count - 1 ) );
		entity.SetResourceAs( resource );

		entity.Position = point;

		// TODO: Handle this better, this will only work for entities that you want to lay down flat on the terrain.
		if ( entity.ResourceAsset.IsCollectable )
			entity.Rotation = Rotation.LookAt( normal );

		entity.OnDestroyed += OnResourceDestroyed;

		Resources.Add( entity );
	}

	protected void DestroyResource( ResourceNodeEntity resource, int listIndex )
	{
		resource.Delete();
	}

	protected void CheckDecay()
	{
		CheckedDecayingEntities = 0;

		for ( int i = Resources.Count - 1; i >= 0; i-- )
		{
			ResourceNodeEntity entity = Resources[i];

			if ( entity.LastRefresh > IndividualDecayTime )
			{
				DestroyResource( entity, i );
			}
		}
	}

	protected async Task Refresh()
	{
		Log.Info( "ResourceManager: Refresh" );

		TrackedEntities.ForEach( entity => RefreshEntity( entity ) );

		if ( CheckedDecayingEntities >= CheckDecayTime )
			CheckDecay();

		await GameTask.DelaySeconds( RefreshTimeSeconds );

		CurrentTask = Refresh();
	}
}
