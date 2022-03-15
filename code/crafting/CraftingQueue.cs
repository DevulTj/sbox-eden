// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Eden;

public partial class Craft : BaseNetworkable
{
	//
	public bool IsValid => Asset is not null;

	//
	public ItemAsset Asset { get; set; } = null;
	public int Quantity { get; set; } = 1;

	//
	public Craft( ItemAsset asset, int quantity )
	{
		Asset = asset;
		Quantity = quantity;
	}
}

public partial class CraftingQueue : BaseNetworkable
{
	[ServerCmd( "eden_crafting_playercraft" )]
	public static void PlayerCraft( int assetId, int quantity )
	{
		var player = ConsoleSystem.Caller as Player;
		if ( player.IsValid() ) return;

		var asset = Asset.FromId<ItemAsset>( assetId );
		if ( asset is null ) return;

		var queue = player.CraftingQueue;
		queue.AddToQueue( asset, quantity );
	}

	[ServerCmd( "eden_crafting_playercraft_cancel" )]
	public static void PlayerCraftCancel( int index )
	{
		var player = ConsoleSystem.Caller as Player;
		if ( player.IsValid() ) return;

		var queue = player.CraftingQueue;
		queue.Cancel( index );
	}

	/// <summary>
	/// How many items can be in the queue
	/// </summary>
	public virtual int MaxInQueue => 6;

	[Net]
	public IList<Craft> Queue { get; set; }
	[Net]
	public Craft CurrentCraft { get; set; }
	[Net]
	public TimeSince CraftStarted { get; set; }
	[Net]
	public TimeUntil CraftFinished { get; set; }

	protected virtual void OnFinishCraft( Craft craft )
	{
		//
	}

	protected virtual bool CanAddToQueue( ItemAsset asset, int quantity )
	{
		return Queue.Count < MaxInQueue;
	}

	protected async Task CraftingRoutine( Craft craft )
	{
		Log.Info( $"Crafting routine started: {craft.Asset.ItemName} x{craft.Quantity}" );

		CraftStarted = 0;
		CraftFinished = craft.Asset.CraftingDuration;

		// Wait for a while
		await GameTask.DelaySeconds( craft.Asset.CraftingDuration );

		Log.Info( $"Crafting finished for: {craft.Asset.ItemName} x{craft.Quantity}" );

		OnFinishCraft( craft );

		// Bye bye!
		Queue.RemoveAt( 0 );
		CurrentCraft = null;

		if ( Queue.Count > 0 )
			StartCrafting();
	}

	protected void StartCrafting()
	{
		var craft = Queue[0];
		_ = CraftingRoutine( craft );
	}

	public void AddToQueue( ItemAsset asset, int quantity = 1 )
	{
		if ( !CanAddToQueue( asset, quantity ) )
			return;

		// @TODO: Take items from the container source, if they can afford it.

		Log.Info( $"Adding item to queue: {asset.ItemName}, quantity: {quantity}" );

		Queue.Add( new Craft( asset, quantity ) );

		if ( !CurrentCraft.IsValid )
			StartCrafting();
	}

	public void Cancel( int index )
	{
		var craft = Queue[index];
		Queue.RemoveAt( index );

		// @TODO: refunds

		if ( index == 0 )
		{
			CurrentCraft = null;
			if ( Queue.Count > 0 )
				StartCrafting();
		}
	}
}
