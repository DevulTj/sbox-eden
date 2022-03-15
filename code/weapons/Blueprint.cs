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
			var targetTransform = GetSnappedTransform( new Transform( TraceForward( Owner ).EndPosition, Rotation.Identity ) );
			PlaceBuilding( selectedAsset.Id, targetTransform.Position, targetTransform.Rotation );
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

	[ServerCmd( "eden_building_place" )]
	public static void PlaceBuilding( int assetId, Vector3 position, Rotation rotation )
	{
		var asset = Asset.FromId<BuildingAsset>( assetId );

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

	private Transform GetSnappedTransform( Transform transform )
	{
		var forwardTracePosition = TraceForward( Owner ).EndPosition;
		var forwardTraceDirection = Owner.EyeRotation;

		var orderedSnapPoints = GetNearbySnapPoints().OrderBy( x => x.Position.Distance( forwardTracePosition ) + x.Rotation.Distance( forwardTraceDirection ) );
		if ( !orderedSnapPoints.Any() )
			return transform;

		var nearestSnapPoint = orderedSnapPoints.First();

		var localOther = ghostEntity.Transform.ToLocal( nearestSnapPoint );
		var localGhostSnapPoints = ghostEntity.GetSnapPoints( worldSpace: false );
		var localClosestSnapPoint = localGhostSnapPoints.OrderBy( x => x.Position.Distance( localOther.Position ) ).First();

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
