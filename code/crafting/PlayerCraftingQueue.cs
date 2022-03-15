// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class PlayerCraftingQueue : CraftingQueue
{
	[ServerCmd( "eden_crafting_playercraft" )]
	public static void Craft( string assetName, int quantity )
	{
		var player = ConsoleSystem.Caller.Pawn as Player;
		if ( !player.IsValid() ) return;

		var asset = ItemAsset.FromName( assetName );
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
