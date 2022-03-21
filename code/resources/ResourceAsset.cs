// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eden;

[Library( "e_res" ), AutoGenerate]
public partial class ResourceAsset : Asset
{
	public static HashSet<ResourceAsset> All { get; protected set; } = new();

	[Property, Category( "Meta" )]
	public string ResourceName { get; set; }

	[Property, Category( "World" ), ResourceType( "vmdl" ), Sandbox.Description( "Test" )]
	public string[] WorldModelPath { get; set; }

	[Property, Category( "Gathering" )]
	public ResourceType ResourceType { get; set; }

	[Property, Category( "Gathering" )]
	public bool Collectable { get; set; }

	[Property, Category( "Gathering" )]
	public int RequiredHitsPerItem { get; set; }

	[Property, Category( "Gathering" )]
	public List<ResourceItemQuantity> ItemsToGather { get; set; }

	public Model WorldModel { get; set; }
	public static Model FallbackWorldModel = Model.Load( "models/resources/resource_blockout.vmdl" );
	public bool ResourceHasMultipleModels => WorldModelPath.Length > 1;

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );

			// Cache the world models immediately
			for ( int i = 0; i < WorldModelPath.Length; i++ )
			{
				if ( i == 0 )
				{
					WorldModel = Model.Load( WorldModelPath[i] );
					continue;
				}

				Model.Load( WorldModelPath[i] );
			}

			foreach ( var item in ItemsToGather )
				item.AmountRemaining = item.InitialAmount;

			Log.Info( $"Eden: Loading resource asset: {ResourceName}" );
		}
	}
}
