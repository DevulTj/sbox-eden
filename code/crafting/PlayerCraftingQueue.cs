// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class PlayerCraftingQueue : CraftingQueue
{
	[ServerCmd( "eden_crafting_playercraft" )]
	public static void Craft( int resourceId, int quantity )
	{
		var player = ConsoleSystem.Caller.Pawn as Player;
		if ( !player.IsValid() ) return;

		var asset = Asset.FromId<ItemAsset>( resourceId );
		if ( asset is null ) return;

		var queue = player.CraftingQueue;
		queue.AddToQueue( asset, quantity );
	}

	[ServerCmd( "eden_crafting_playercraft_cancel" )]
	public static void CraftCancel( int index )
	{
		var player = ConsoleSystem.Caller.Pawn as Player;
		if ( !player.IsValid() ) return;

		var queue = player.CraftingQueue;
		queue.Cancel( index );
	}

	// @BaseNetworkable
	public PlayerCraftingQueue()
	{
	}

	public PlayerCraftingQueue( Player player ) : base()
	{
		Player = player;
	}

	public Player Player { get; set; }

	public ContainerRemoveTransaction CreateTransaction( ItemAsset asset )
	{
		var t = new ContainerRemoveTransaction();
		t.AddContainer( Player.Hotbar, Player.Backpack );

		foreach ( var item in asset.Recipe.Items )
			t.AddRequirement( new ContainerTransactionItem() { ItemAsset = ItemAsset.FromName( item.ItemId ), Quantity = item.Amount } );

		return t;
	}

	protected override bool CanAddToQueue( ItemAsset asset, int quantity )
	{
		// @TODO: take shit from the player
		bool baseResult = base.CanAddToQueue( asset, quantity );
		var transaction = CreateTransaction( asset );

		return baseResult && transaction.CanDo() && transaction.Execute();
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
