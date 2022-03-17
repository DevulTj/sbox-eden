// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Linq;

namespace Eden;

partial class Blueprint
{
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
}
