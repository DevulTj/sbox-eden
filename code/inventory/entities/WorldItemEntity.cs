// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class WorldItemEntity : Prop, IUse
{
	public static WorldItemEntity Instantiate( Item item, int quantity = 1 )
	{
		var entity = new WorldItemEntity();
		entity.SetItem( item, quantity );

		return entity;
	}

	public static WorldItemEntity InstantiateFromPlayer( Player player, Item item, int quantity = 1 )
	{
		var entity = Instantiate( item, quantity );
		entity.Position = player.EyePosition + player.EyeRotation.Forward * 85;

		return entity;
	}

	[Net]
	public ItemAsset Asset { get; set; }

	[Net]
	public int Quantity { get; set; } = 1;

	// @net
	public Item Item { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Model = ItemAsset.FallbackWorldModel;
	}

	public void SetItem( Item item, int quantity = 1 )
	{
		Item = item;
		Asset = item.Asset;
		Quantity = quantity;

		if ( item.Asset.WorldModel is not null )
			Model = item.Asset.WorldModel;
	}

	bool IUse.OnUse( Entity user )
	{
		var player = user as Player;
		if ( player is null ) return false;

		InventoryHelpers.PickupItem( player, this );

		return true;
	}

	bool IUse.IsUsable( Entity user )
	{
		return true;
	}
}
