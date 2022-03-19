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
	protected TimeSince CheckedDecayingEntities { get; set; } = 0;

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
			.OfType<ResourceEntity>()
			.ToList();

		list.ForEach( x =>
		{
			x.LastRefresh = 0;
		} );

		GeneratePoints( entity, entity.Position, 1500, 4, 16 );
	}

	protected Vector3 GeneratePoint( Vector2 origin, float radius )
	{
		var x = Rand.Float() - 0.5f;
		var y = Rand.Float() - 0.5f;

		var magnitude = MathF.Sqrt( x * x + y * y );

		x /= magnitude;
		y /= magnitude;

		var d = Rand.Float( radius );

		return origin += new Vector2( x * d, y * d );
	}

	protected void GeneratePoints( Entity entity, Vector2 coordinate, float radius, int minAmount, int maxAmount )
	{
		var amount = Rand.Int( minAmount, maxAmount );

		for ( int i = 0; i < amount; i++ )
		{
			var point = GeneratePoint( coordinate, radius );
			point.z = entity.Position.z + 1000f;

			var tr = Trace.Ray( point, point + Vector3.Down * 4096f )
				.WorldOnly()
				.Run();

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
		entity.SetResourceAs( ResourceType.Wood );
		entity.Position = point;

		// TODO: Handle this better, this will only work for entities that you want to lay down flat on the terrain.
		if ( entity.ResourceAsset.Collectable )
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
