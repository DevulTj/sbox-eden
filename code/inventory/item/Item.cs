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
	public virtual Color DefaultColor => Asset?.DefaultColor ?? new Color( 100, 100, 100 );
	public virtual bool CanStack => MaxStack > 1;
	public virtual int MaxStack => Asset?.StackSize ?? 1;
	public virtual HashSet<ItemActionType> ItemActions => new()
	{
		ItemActionType.Split,
		ItemActionType.Drop
	};

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

	private bool ActionAttributeMethod<T>( Player player, ItemActionType type, Slot slotRef ) where T : ItemActionBaseAttribute
	{
		if ( !ItemActions.Contains( type ) )
			return false;

		var attribute = Library.GetAttributes<T>()
			.FirstOrDefault( x => x.Type == type );

		if ( attribute is null ) return false;

		return (bool)attribute.Invoke( this, player, slotRef );
	}

	public bool CanDoAction( Player player, ItemActionType type, Slot slotRef ) =>
		ActionAttributeMethod<ItemActionCheckAttribute>( player, type, slotRef );

	public bool DoAction( Player player, ItemActionType type, Slot slotRef ) =>
		ActionAttributeMethod<ItemActionExecAttribute>( player, type, slotRef );

	[ItemActionCheck( ItemActionType.Drop )]
	public bool CanDrop( Player player, Slot slotRef )
	{
		return true;
	}

	[ItemActionExec( ItemActionType.Drop, "Drop" )]
	public bool Drop( Player player, Slot slotRef )
	{
		var entity = ItemEntity.InstantiateFromPlayer( player, this, slotRef.Quantity );
		return entity.IsValid();
	}

	[ItemActionCheck( ItemActionType.Split )]
	public bool CanSplit( Player player, Slot slotRef )
	{
		return slotRef.Quantity > 1;
	}

	[ItemActionExec( ItemActionType.Split, "Split" )]
	public bool Split( Player player, Slot slotRef )
	{
		//var entity = ItemEntity.InstantiateFromPlayer( player, this, slotRef.Quantity );
		//return entity.IsValid();
		return false;
	}
}
