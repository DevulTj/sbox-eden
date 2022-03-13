// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class Item
{
	/// <summary>
	/// Creates an item based off an Item Asset.
	/// </summary>
	/// <param name="asset"></param>
	/// <returns></returns>
	public static Item FromAsset( ItemAsset asset )
	{
		var item = asset.Type.Instantiate();
		item.Asset = asset;
		return item;
	}

	/// <summary>
	/// Creates an item based off an Item Asset's class name.
	/// </summary>
	/// <param name="assetName"></param>
	/// <returns></returns>
	public static Item FromAsset( string assetName )
	{
		if ( ItemAsset.Classes.TryGetValue( assetName, out var asset ) )
			return FromAsset( asset );

		return null;
	}

	public override string ToString() => Asset?.ItemName ?? "Item";
	public virtual ItemType Type => ItemType.Item;
	public virtual Color DefaultColor => Asset?.DefaultColor ?? Color.White;
	public virtual bool CanStack => Asset.StackSize > 1;

	// @networked
	public ItemAsset Asset { get; set; }
	public int MaxStack => Asset.StackSize;

	public virtual void Write( NetWrite write )
	{
		write.Write( Asset );
	}

	public virtual void Read( NetRead read )
	{
		Asset = read.ReadClass<ItemAsset>();
	}
}
