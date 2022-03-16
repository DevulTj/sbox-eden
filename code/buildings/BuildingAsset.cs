// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eden;

[Library( "e_bld" ), AutoGenerate]
public class BuildingAsset : Asset
{
	public static HashSet<BuildingAsset> All { get; protected set; } = new();

	[Property, Category( "Meta" )]
	public string BuildingName { get; set; }

	[Property, Category( "Meta" ), ResourceType( "vmdl" )]
	public string BuildingModelPath { get; set; }

	[Property, Category( "Meta" ), ResourceType( "png" )]
	public string BuildingIconPath { get; set; }

	[Property, Category( "Meta" )]
	public bool BuildFreestanding { get; set; }

	public List<Transform> GetLocalSnapPointTransforms() => ModelSnapPoints.GetLocalSnapPointTransforms( BuildingModel );

	public bool CanAfford( Player player )
	{
		// TODO
		return true;
	}

	public bool CheckValidPlacement( Vector3 position, Rotation rotation )
	{
		// TODO
		return true;
	}

	public Model BuildingModel { get; set; }

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );

			// Cache the building model immediately
			if ( !string.IsNullOrEmpty( BuildingModelPath ) )
			{
				BuildingModel = Model.Load( BuildingModelPath );
			}

			Log.Info( $"Eden: Loading building asset: {BuildingName}" );
		}
	}
}
