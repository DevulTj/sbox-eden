// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.ComponentModel;

namespace Eden;

[Library( "e_resource" ), AutoGenerate]
public partial class ResourceAsset : Asset
{
	public static HashSet<ResourceAsset> All { get; protected set; } = new();

	[Property, Category( "Meta" )]
	public string ResourceName { get; set; }

	[Property, Category( "World" ), ResourceType( "vmdl" )]
	public string WorldModelPath { get; set; }

	[Property, Category( "Gathering" )]
	public ResourceType ResourceType { get; set; }

	[Property, Category( "Gathering" )]
	public int YieldPerHit { get; set; }

	[Property, Category( "Gathering" )]
	public List<ResourceItemQuantity> ItemsToGather { get; set; }

	public Model WorldModel { get; set; }
	public static Model FallbackWorldModel = Model.Load( "models/sbox_props/bin/rubbish_bag.vmdl" );

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );

			// Cache the world model immediately
			if ( !string.IsNullOrEmpty( WorldModelPath ) )
			{
				WorldModel = Model.Load( WorldModelPath );
			}

			Log.Info( $"Eden: Loading resource asset: {ResourceName}" );
		}
	}
}
