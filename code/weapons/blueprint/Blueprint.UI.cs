// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

partial class Blueprint
{
	private RadialWheel buildWheel;

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
