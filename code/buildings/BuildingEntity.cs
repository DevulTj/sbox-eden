// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public class BuildingEntity : ModelEntity
{
	public void UpdateFromAsset( BuildingAsset asset )
	{
		Host.AssertServer();

		Model = asset.BuildingModel;
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		Health = 100;
	}
}
