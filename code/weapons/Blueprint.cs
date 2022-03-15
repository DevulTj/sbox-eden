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

	private static TraceResult TraceForward( Entity entity, float distance = 512f )
	{
		return Trace.Ray( entity.EyePosition, entity.EyePosition + entity.EyeRotation.Forward * distance ).Ignore( entity ).Run();
	}

	private List<Transform> GetNearestSnapPoints()
	{
		var trace = TraceForward( Owner, 128f );
		var overlaps = Entity.FindInSphere( trace.EndPosition, 128f ).OfType<BuildingEntity>();

		var list = new List<Transform>();

		foreach ( var overlap in overlaps )
		{
			if ( !overlap.IsValid )
				continue;

			var datas = overlap.Model?.GetData<ModelSnapPoints[]>();

			if ( datas == null )
				continue;

			var data = datas[0];
			var snapPoints = data.SnapPoints;
			var snapTransforms = snapPoints.Select( attachment => overlap.GetAttachment( attachment ) );
			snapTransforms.ToList().ForEach( snapPoint => list.Add( snapPoint ?? default ) );
		}

		return list;
	}

	[Event.Frame]
	public void OnFrame()
	{
		foreach ( var snapPoint in GetNearestSnapPoints() )
		{
			DebugOverlay.Sphere( snapPoint.Position, 4f, Color.Cyan, false );
		}

		if ( ghostEntity == null )
			return;

		foreach ( var snapPoint in ghostEntity.GetSnapPoints() )
		{
			DebugOverlay.Sphere( snapPoint.Position, 4f, Color.Green, false );
		}
	}

	private Transform GetSnappedTransform( Transform transform )
	{
		var forwardTracePosition = TraceForward( Owner, 128f ).EndPosition;

		var nearestSnapPoint = GetNearestSnapPoints().OrderBy( x => x.Position.Distance( forwardTracePosition ) ).FirstOrDefault();

		if ( nearestSnapPoint == default )
			return transform;

		if ( forwardTracePosition.Distance( nearestSnapPoint.Position ) < 128 )
		{
			var localOther = ghostEntity.Transform.ToLocal( nearestSnapPoint );
			var localGhostSnapPoints = ghostEntity.GetSnapPoints().Select( x => ghostEntity.Transform.ToLocal( x ) );
			var localClosestSnapPoint = localGhostSnapPoints.OrderBy( x => x.Position.Distance( localOther.Position ) ).First();

			DebugOverlay.ScreenText( 0, localClosestSnapPoint.Position.ToString() );

			return nearestSnapPoint.WithPosition( nearestSnapPoint.Position - localClosestSnapPoint.Position );
		}

		return transform;
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
