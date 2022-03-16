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
			PlaceBuilding();
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

	private void PlaceBuilding()
	{
		CmdCreateBuildingManual( selectedAsset.Id, TraceForward( Owner ).EndPosition, Rotation.Identity );
	}

	private static BuildingEntity CreateBuilding( int assetId, Vector3 position, Rotation rotation )
	{
		Host.AssertServer();

		var asset = Asset.FromId<BuildingAsset>( assetId );
		var player = ConsoleSystem.Caller.Pawn as Player;

		if ( player == null || asset == null )
			return default;

		// Server-side validation checks, make sure the player isn't doing anything funky
		if ( !asset.CanAfford( player ) )
			return default;

		var building = new BuildingEntity();
		building.Position = position;
		building.Rotation = rotation;
		building.UpdateFromAsset( asset );

		return building;
	}

	[ServerCmd( "eden_building_create_manual" )]
	public static void CmdCreateBuildingManual( int assetId, Vector3 position, Rotation rotation )
	{
		CreateBuilding( assetId, position, rotation );
	}

	[ServerCmd( "eden_building_create" )]
	public static void CmdCreateBuilding( int assetId, int snapId, int attachedBuildingId, int attachedSnapId )
	{
		var snapBuilding = Entity.All.OfType<BuildingEntity>().First( x => x.NetworkIdent == attachedBuildingId );
		var snap = snapBuilding.SnapPoints[attachedSnapId];

		var building = CreateBuilding( assetId, snap.Transform.Position, snap.Transform.Rotation );

		if ( building == null )
			return;

		building.SnapPoints[snapId].AttachedEntity = snapBuilding;
		snapBuilding.SnapPoints[attachedSnapId].AttachedEntity = building;
	}

	private static TraceResult TraceForward( Entity entity, float distance = maxBuildDistance )
	{
		return Trace.Ray( entity.EyePosition, entity.EyePosition + entity.EyeRotation.Forward * distance ).Ignore( entity ).Run();
	}

	/// <summary>
	/// Find nearby entities, fetch their snap points
	/// </summary>
	private List<SnapPoint> GetNearbySnapPoints()
	{
		var trace = TraceForward( Owner );
		var overlaps = Entity.FindInSphere( trace.EndPosition, maxBuildDistance ).OfType<BuildingEntity>();

		var snapPoints = new List<SnapPoint>();

		foreach ( var overlap in overlaps )
		{
			if ( !overlap.IsValid )
				continue;

			snapPoints.AddRange( overlap.SnapPoints );
		}

		return snapPoints;
	}

	/// <summary>
	/// Fetch a transform that snaps our ghost entity to a nearby building
	/// </summary>
	private SnapPoint? GetBestSnapPoint()
	{
		var transform = new Transform( TraceForward( Owner ).EndPosition );

		var forwardTracePosition = TraceForward( Owner ).EndPosition;
		var forwardTraceDirection = Owner.EyeRotation;

		// Get all nearby snap points, order them by most appropriate (closest & with similar rotation angles)
		// TODO: Ensure that the placement is valid too
		var nearbySnapPoints = GetNearbySnapPoints().Where( x => x.AttachedEntity == null || !x.AttachedEntity.IsValid );
		var orderedSnapPoints = nearbySnapPoints.OrderBy( x => x.Transform.Position.Distance( forwardTracePosition ) );
		if ( !orderedSnapPoints.Any() )
			return null;

		// Get the best available snap point
		var bestSnapPoint = orderedSnapPoints.First();

		return bestSnapPoint;
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
