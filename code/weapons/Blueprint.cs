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
			BuildingSystem.PlaceBuilding( selectedAsset.Id, targetTransform.Position, targetTransform.Rotation );
		}
	}

	[Event.Frame]
	public void OnFrame()
	{
		if ( !Debug )
			return;

		if ( ghostEntity == null || !ghostEntity.IsValid )
			return;

		foreach ( var snapPoint in selectedAsset.GetLocalSnapPointTransforms() )
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
	/// Get all nearby snap points, select most appropriate
	/// </summary>
	/// <returns></returns>
	public bool GetBestSnapTransforms( out Transform nearestSnapPoint, out Transform localClosestSnapPoint )
	{
		var transform = new Transform( TraceForward( Owner ).EndPosition );
		var forwardTracePosition = TraceForward( Owner ).EndPosition;

		nearestSnapPoint = transform;
		localClosestSnapPoint = transform;

		// Ensure that the placement is valid
		var nearbySnapPoints = GetNearbySnapPoints().Where( x => x.AttachedEntity == null || !x.AttachedEntity.IsValid );
		var validSnapPoints = nearbySnapPoints.OrderBy( x => x.Transform.Position.Distance( forwardTracePosition ) );

		// If we don't have anywhere to snap, bail
		if ( !validSnapPoints.Any() )
			return false;

		// Get the best available snap point
		nearestSnapPoint = validSnapPoints.First().Transform;

		// Convert everything into local space; this prevents us from entering any funky feedback loops
		var localOther = ghostEntity.Transform.ToLocal( nearestSnapPoint );
		var localGhostSnapPoints = selectedAsset.GetLocalSnapPointTransforms();
		localClosestSnapPoint = localGhostSnapPoints.OrderBy( x => x.Position.Distance( localOther.Position ) ).FirstOrDefault();

		return true;
	}

	/// <summary>
	/// Fetch a transform that snaps our ghost entity to a nearby building
	/// </summary>
	private Transform GetSnappedTransform()
	{
		if ( !GetBestSnapTransforms( out var nearestSnapPoint, out var localClosestSnapPoint ) )
			return new Transform( TraceForward( Owner ).EndPosition );

		if ( Debug )
			DebugOverlay.Sphere( nearestSnapPoint.Position, 8f, Color.Red, false );

		// Return best available snap point, offset by nearest selected building snap point
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
