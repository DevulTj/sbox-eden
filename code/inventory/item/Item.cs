// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
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
		var item = asset.Type.Create();
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
	public virtual Color DefaultColor => Asset?.DefaultColor ?? new Color( 100, 100, 100 );
	public virtual bool CanStack => MaxStack > 1;
	public virtual int MaxStack => Asset?.StackSize ?? 1;

	// @net
	public ItemAsset Asset { get; set; }

	public virtual void Write( NetWrite write )
	{
		write.Write( Asset );
	}

	public virtual void Read( NetRead read )
	{
		Asset = read.ReadClass<ItemAsset>();
	}

	/// <summary>
	/// Tells if an item is the same as another
	/// </summary>
	/// <param name="other"></param>
	/// <returns>true if <type><paramref name="other"/></type> has the same asset as our item, otherwise false</returns>
	public bool IsSame( Item other )
	{
		if ( other is null ) return false;

		return other.Asset == Asset;
	}

	public bool CanDoAction( Player player, string id, Slot slot )
	{
		var actions = GetActions();
		if ( actions.Count < 1 )
			return false;

		var itemAction = actions.FirstOrDefault( x => x.ID == id );

		return itemAction?.CanDo( player, slot ) ?? false;
	}

	public bool DoAction( Player player, string id, Slot slot )
	{
		var actions = GetActions();
		if ( actions.Count < 1 )
			return false;

		var itemAction = actions.FirstOrDefault( x => x.ID == id );

		return itemAction?.Execute( player, slot ) ?? false;
	}

	public List<ItemAction> GetActions()
	{
		var actions = new List<ItemAction>();
		GatherActions( ref actions );

		return actions;
	}

	protected virtual void GatherActions( ref List<ItemAction> actions )
	{
		actions.Add( new DropItemAction() );
	}
}
