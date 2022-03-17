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
		return new();
	}

	private void FindBestSnapPoints()
	{
	}
}
