// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public partial class PlayerCraftingQueue : CraftingQueue
{
	// Required for BaseNetworkable
	public PlayerCraftingQueue()
	{
	}

	public PlayerCraftingQueue( Player player ) : base()
	{
		Player = player;
	}

	public Player Player { get; set; }

	protected override bool CanAddToQueue( ItemAsset asset, int quantity )
	{
		// @TODO: take shit from the player
		bool baseResult = base.CanAddToQueue( asset, quantity );
		return baseResult;
	}

	protected override void OnFinishCraft( Craft craft )
	{
		var item = Item.FromAsset( craft.Asset );
		if ( !InventoryHelpers.GiveItem( Player, item, craft.Quantity ) )
		{
			// @TODO: handle failure
		}
	}
}
