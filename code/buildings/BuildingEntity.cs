// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;

namespace Eden;

public partial class BuildingEntity : ModelEntity
{
	[Net] public IList<SnapPoint> SnapPoints { get; set; }

	[Event.Tick]
	public void OnTick()
	{
		foreach ( var snapPoint in SnapPoints )
		{
			if ( IsServer )
				DebugOverlay.Sphere( snapPoint.Transform.Position, 2f, Color.Cyan, false );
			else
				DebugOverlay.Sphere( snapPoint.Transform.Position, 3f, Color.Blue, false );

			if ( snapPoint.AttachedEntity != null && snapPoint.AttachedEntity.IsValid )
			{
				DebugOverlay.Text( snapPoint.Transform.Position, snapPoint.AttachedEntity.Name );
			}
		}
	}

	public void UpdateFromAsset( BuildingAsset asset )
	{
		Host.AssertServer();

		Model = asset.BuildingModel;
		SetupPhysicsFromModel( PhysicsMotionType.Static );

		foreach ( var localTransform in asset.GetLocalSnapPointTransforms() )
		{
			var worldTransform = Transform.ToWorld( localTransform );
			SnapPoints.Add( new SnapPoint( worldTransform ) );
		}

		Health = 100;
	}
}
