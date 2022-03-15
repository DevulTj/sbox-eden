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
	[Net]
	public IList<Craft> Queue { get; set; }
	[Net]
	public Craft CurrentCraft { get; set; }
	[Net]
	public TimeSince CraftStarted { get; set; }
	[Net]
	public TimeUntil CraftFinished { get; set; }

	protected async Task CraftingRoutine( Craft craft )
	{
		CraftStarted = 0;
		CraftFinished = craft.Asset.CraftingDuration;

		// Wait for a while
		await GameTask.DelaySeconds( craft.Asset.CraftingDuration );

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

	public void AddToQueue( Container source, ItemAsset asset, int quantity = 1 )
	{
		// @TODO: Take items from the container source, if they can afford it.

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
