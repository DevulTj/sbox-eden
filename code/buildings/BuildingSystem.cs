// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public static class BuildingSystem
{
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
		building.UpdateFromAsset( asset );
	}
}
