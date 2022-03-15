// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Eden;

[Library( "e_bld" ), AutoGenerate]
public class BuildingAsset : Asset
{
	public static HashSet<BuildingAsset> All { get; protected set; } = new();
	public static Dictionary<string, BuildingAsset> Classes { get; protected set; } = new();

	[Property, Category( "Meta" )] public string BuildingName { get; set; }
	[Property, Category( "Meta" ), ResourceType( "vmdl" )] public string BuildingModelPath { get; set; }
	[Property, Category( "Meta" ), ResourceType( "png" )] public string BuildingIconPath { get; set; }

	public List<Transform> GetSnapPoints()
	{
		var snapPoints = ModelSnapPoints.GetSnapPoints( BuildingModel );
		return snapPoints;
	}

	public bool IsPlacementValid()
	{
		return true;
	}

	public bool CanAfford( Player player )
	{
		return true;
	}

	public Model BuildingModel { get; set; }

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );
			Classes[BuildingName] = this;

			// Cache the world model immediately
			if ( !string.IsNullOrEmpty( BuildingModelPath ) )
			{
				BuildingModel = Model.Load( BuildingModelPath );
			}

			Log.Info( $"Eden: Loading building asset: {BuildingName}" );
		}
	}
}
