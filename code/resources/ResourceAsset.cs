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

	[Property, Category( "World" ), ResourceType( "vmdl" ), Sandbox.Description( "Specify more than 1 for the model to change as gathering progresses." )]
	public string[] WorldModelPath { get; set; }

	[Property, Category( "Gathering" )]
	public ResourceType ResourceType { get; set; }

	[Property, Category( "Gathering" ), Sandbox.Description( "Whether or not the resource can be collected with 'E'" )]
	public bool IsCollectable { get; set; }

	[Property, Category( "Gathering" )]
	public int RequiredHitsPerItem { get; set; }

	[Property, Category( "Gathering" )]
	public int BaseDurabilityPenalty { get; set; } = -1;

	[Property, Category( "Effects" ), ResourceType( "sound" )]
	public string ModelChangeSound { get; set; }

	[Property, Category( "Effects" ), ResourceType( "vpcf" )]
	public string ModelChangeParticle { get; set; }

	[Property, Category( "Gathering" )]
	public List<ResourceItemQuantity> ItemsToGather { get; set; }

	public List<Model> WorldModels { get; set; } = new();
	public static Model FallbackWorldModel = Model.Load( "models/resources/resource_blockout.vmdl" );
	public bool ResourceHasMultipleModels => WorldModelPath.Length > 1;

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );

			// Cache the world models immediately
			foreach ( var modelPath in WorldModelPath )
				WorldModels.Add( Model.Load( modelPath ) );

			foreach ( var item in ItemsToGather )
				item.AmountRemaining = item.InitialAmount;

			Log.Info( $"Eden: Loading resource asset: {ResourceName}" );
		}
	}
}
