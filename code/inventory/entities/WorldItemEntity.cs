// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class WorldItemEntity : Prop, IUse
{
	public static WorldItemEntity Instantiate( Item item )
	{
		var entity = new WorldItemEntity();
		entity.SetItem( item );

		return entity;
	}

	public static WorldItemEntity InstantiateFromPlayer( Player player, Item item )
	{
		var entity = Instantiate( item );
		entity.Position = player.EyePosition + player.EyeRotation.Forward * 85;

		return entity;
	}

	[Net]
	public ItemAsset Asset { get; set; }

	public Item Item { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Model = ItemAsset.FallbackWorldModel;
	}

	public void SetItem( Item item )
	{
		Item = item;
		Asset = item.Asset;

		if ( item.Asset.WorldModel is not null )
			Model = item.Asset.WorldModel;
	}

	bool IUse.OnUse( Entity user )
	{
		var player = user as Player;
		if ( player is null ) return false;

		ContainerNetwork.PickupItem( player, this );

		return true;
	}

	bool IUse.IsUsable( Entity user )
	{
		return true;
	}
}
