// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Linq;

namespace Eden;


partial class Blueprint : Weapon
{
	private partial class GhostEntity : ModelEntity
	{
		public override void Spawn()
		{
			base.Spawn();
		}

		public void UpdateFromAsset( BuildingAsset asset )
		{
			Model = asset.BuildingModel;
			SetMaterialOverride( "materials/buildings/building_ghost.vmat" );
		}

		[Event.Frame]
		public void OnFrameUpdate()
		{
			Position = TraceForward( Owner ).EndPosition;
			Rotation = Rotation.Identity;
		}
	}
}
