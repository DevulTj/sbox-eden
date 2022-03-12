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

	public virtual ItemType Type => ItemType.Item;

	[Property, Category( "Meta" )]
	public string ItemName { get; set; }

	[Property, Category( "Meta" )]
	public string ItemDescription { get; set; }

	[Property, Category( "Meta" ), ResourceType( "png" )]
	public string IconPath { get; set; }

	protected override void PostLoad()
	{
		base.PostLoad();

		if ( !All.Contains( this ) )
		{
			All.Add( this );
			Classes[Name] = this;

			Log.Info( $"Eden: Loading item asset: {ItemName}" );
		}
	}
}
