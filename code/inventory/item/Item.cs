// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class Item
{
	public static Item FromAsset( ItemAsset asset )
	{
		var item = asset.Type.Instantiate();
		item.Asset = asset;
		return item;
	}

	public static Item FromAsset( string assetName )
	{
		if ( ItemAsset.Classes.TryGetValue( assetName, out var asset ) )
			return FromAsset( asset );

		return null;
	}

	public override string ToString() => Asset?.ItemName ?? "Item";
	public virtual ItemType Type => ItemType.Item;
	public virtual Color DefaultColor => Asset?.DefaultColor ?? Color.White;

	public ItemAsset Asset { get; set; }

	public int Quantity { get; set; } = 1;

	public virtual void Write( NetWrite write )
	{
		write.Write( Asset );
		write.Write( Quantity );
	}

	public virtual void Read( NetRead read )
	{
		Asset = read.ReadClass<ItemAsset>();
		Quantity = read.Read<int>();
	}
}
