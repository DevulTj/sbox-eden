// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

[Library( "eden_blueprint", Title = "Blueprint", Spawnable = false )]
partial class Blueprint : Weapon
{
	private RadialWheel buildWheel;
	private GhostEntity ghostEntity;

	private BuildingAsset selectedAsset = BuildingAsset.All.First();

	private const float maxBuildDistance = 128f;

	[ClientVar( "eden_debug_blueprint" )]
	public static bool Debug { get; set; } = false;

	public override void Simulate( Client cl )
	{
		base.Simulate( cl );

		if ( !IsClient )
			return;

		if ( Input.Pressed( InputButton.Attack2 ) )
			CreateBuildWheel();

		if ( Input.Released( InputButton.Attack2 ) )
			DeleteBuildWheel();

		if ( Input.Pressed( InputButton.Attack1 ) )
		{
			var targetTransform = GetSnappedTransform();
			PlaceBuilding( selectedAsset.Id, targetTransform.Position, targetTransform.Rotation );
		}
	}

	[Event.Frame]
	public void OnFrame()
	{
		if ( !Debug )
			return;

		foreach ( var snapPoint in GetNearbySnapPoints() )
		{
			DebugOverlay.Sphere( snapPoint.Position, 4f, Color.Cyan, false );
		}

		if ( !ghostEntity.IsValid )
			return;

		foreach ( var snapPoint in selectedAsset.GetSnapPoints() )
		{
			var worldSnapPoint = GetSnappedTransform().ToWorld( snapPoint );
			DebugOverlay.Sphere( worldSnapPoint.Position, 4f, Color.Green, false );
		}
	}

	public override void ActiveStart( Entity ent )
	{
		base.ActiveStart( ent );

		if ( IsClient )
		{
			ghostEntity = new GhostEntity()
			{
				Owner = Owner,
				Blueprint = this
			};
		}
	}

	public override void ActiveEnd( Entity ent, bool dropped )
	{
		base.ActiveEnd( ent, dropped );

		if ( IsClient )
			ghostEntity?.Delete();
	}

	/// <summary>
	/// Place a building down
	/// </summary>
	[ServerCmd( "eden_building_place" )]
	public static void PlaceBuilding( int assetId, Vector3 position, Rotation rotation )
	{
		var asset = Asset.FromId<BuildingAsset>( assetId );
		var player = ConsoleSystem.Caller.Pawn as Player;

		if ( player == null || asset == null )
			return;

		// Server-side validation checks, make sure the player isn't doing anything funky
		if ( !asset.CanAfford( player ) )
			return;

		var building = new BuildingEntity();
		building.Position = position;
		building.Rotation = rotation;
		building.Model = asset.BuildingModel;
		building.SetupPhysicsFromModel( PhysicsMotionType.Static );
	}

	private static TraceResult TraceForward( Entity entity, float distance = maxBuildDistance )
	{
		return Trace.Ray( entity.EyePosition, entity.EyePosition + entity.EyeRotation.Forward * distance ).Ignore( entity ).Run();
	}

	/// <summary>
	/// Find nearby entities, fetch their snap points
	/// </summary>
	private List<Transform> GetNearbySnapPoints()
	{
		var trace = TraceForward( Owner );
		var overlaps = Entity.FindInSphere( trace.EndPosition, maxBuildDistance ).OfType<BuildingEntity>();

		var snapPoints = new List<Transform>();

		foreach ( var overlap in overlaps )
		{
			if ( !overlap.IsValid )
				continue;

			snapPoints.AddRange( ModelSnapPoints.GetSnapPoints( overlap.Model ).Select( x => overlap.Transform.ToWorld( x ) ) );
		}

		return snapPoints;
	}

	/// <summary>
	/// Fetch a transform that snaps our ghost entity to a nearby building
	/// </summary>
	private Transform GetSnappedTransform()
	{
		var transform = new Transform( TraceForward( Owner ).EndPosition );

		// Where are we aiming / what are we aiming at
		var forwardTracePosition = TraceForward( Owner ).EndPosition;
		var forwardTraceDirection = Owner.EyeRotation;

		// Get all nearby snap points, order them by most appropriate (closest & with similar rotation angles)
		// TODO: Ensure that the placement is valid too
		var orderedSnapPoints = GetNearbySnapPoints().OrderBy( x => x.Position.Distance( forwardTracePosition ) + x.Rotation.Distance( forwardTraceDirection ) );
		if ( !orderedSnapPoints.Any() )
			return transform;

		// Get the best available snap point
		var nearestSnapPoint = orderedSnapPoints.First();

		// Convert everything into local space; this prevents us from entering any funky feedback loops
		var localOther = ghostEntity.Transform.ToLocal( nearestSnapPoint );
		var localGhostSnapPoints = selectedAsset.GetSnapPoints();
		var localClosestSnapPoint = localGhostSnapPoints.OrderBy( x => x.Position.Distance( localOther.Position ) ).First();

		// Return best available snap point, offset by best ghost snap point
		return nearestSnapPoint.WithPosition( nearestSnapPoint.Position - localClosestSnapPoint.Position );
	}

	private void CreateBuildWheel()
	{
		if ( buildWheel != null )
			return;

		buildWheel = RadialWheel.Create();

		foreach ( var buildingAsset in BuildingAsset.All )
		{
			buildWheel.AddOption( buildingAsset.BuildingName, buildingAsset.BuildingIconPath, () =>
			{
				selectedAsset = buildingAsset;
				ghostEntity.UpdateFromAsset( buildingAsset );

				DeleteBuildWheel();
			} );
		}
	}

	private void DeleteBuildWheel()
	{
		buildWheel?.Delete();
		buildWheel = null;
	}
}
