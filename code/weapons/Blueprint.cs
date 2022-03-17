// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

[Library( "eden_blueprint", Title = "Blueprint", Spawnable = false )]
partial class Blueprint : Weapon
{
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

	private static TraceResult TraceForward( Entity entity, float distance = maxBuildDistance ) =>
		Trace.Ray( entity.EyePosition, entity.EyePosition + entity.EyeRotation.Forward * distance ).Ignore( entity ).Run();

	private List<SnapReference> FindNearbySnapPoints()
	{
		var tracePosition = TraceForward( Owner ).EndPosition;
		var nearbyBuildings = Entity.FindInSphere( tracePosition, 128f ).OfType<BuildingEntity>();

		var snapReferenceList = new List<SnapReference>();

		foreach ( var building in nearbyBuildings )
		{
			snapReferenceList.AddRange(
				building.SnapPoints.Select( ( _, index ) => new SnapReference( building, index ) )
			);
		}

		return snapReferenceList;
	}

	private SnapReference? FindBestSnapPoint()
	{
		var nearbySnapPoints = FindNearbySnapPoints();
		var tracePosition = TraceForward( Owner ).EndPosition;

		// Get all nearby snap points, order them by most appropriate (closest & with similar rotation angles)
		var validSnapPoints = nearbySnapPoints.Where( x => x.SnapPoint.AttachedEntity == null || !x.SnapPoint.AttachedEntity.IsValid );
		var orderedSnapPoints = nearbySnapPoints.OrderBy( x => x.SnapPoint.Transform.Position.Distance( tracePosition ) );

		if ( !orderedSnapPoints.Any() )
			return null;

		// Get the best available snap point
		var bestSnapPoint = orderedSnapPoints.First();
		return bestSnapPoint;
	}
}
