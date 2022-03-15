// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Eden;

[Library( "e_item" ), AutoGenerate]
public partial class ItemAsset : Asset
{
	public static HashSet<ItemAsset> All { get; protected set; } = new();
	public static Dictionary<string, ItemAsset> Classes { get; protected set; } = new();

	public static ItemAsset Random
	{
		get
		{
			var random = new Random();
			var index = random.Next( All.Count );
			return All.ElementAt( index );
		}
	}

	public virtual ItemType Type => ItemType.Item;
	public virtual Color DefaultColor => Color.White;

	[Property, Category( "Meta" )]
	public string ItemName { get; set; }

	[Property, Category( "Meta" )]
	public string ItemDescription { get; set; }

	[Property, Category( "Meta" ), Range( 0, 256 )]
	public int StackSize { get; set; } = 1;

	[Property, Category( "Meta" ), ResourceType( "png" )]
	public string IconPath { get; set; }

	[Property, Category( "World" ), ResourceType( "vmdl" )]
	public string WorldModelPath { get; set; }

	[Property, Category( "Crafting" )]
	public int CraftingDuration { get; set; } = 10;

	[Property, Category( "Crafting" )]
	public List<CraftingRecipe> CraftingRecipe { get; set; } = new();

	public Model WorldModel { get; set; }

	public static Model FallbackWorldModel = Model.Load( "models/sbox_props/bin/rubbish_bag.vmdl" );

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );
			Classes[Name] = this;

			// Cache the world model immediately
			if ( !string.IsNullOrEmpty( WorldModelPath ) )
			{
				WorldModel = Model.Load( WorldModelPath );
			}

			Log.Info( $"Eden: Loading item asset: {ItemName}" );
		}
	}
}
