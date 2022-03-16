// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public class ContainerTransactionItem
{
	public ItemAsset ItemAsset { get; set; }
	public int Quantity { get; set; }
}

public class TransactionCriteria
{
	public int Quantity { get; set; } = 0;

	public bool HasBeenMet { get; set; } = false;

	public TransactionCriteria()
	{
	}

	public TransactionCriteria( int quantity )
	{
		Quantity = quantity;
	}
}

public partial class ContainerTransaction
{
	/// <summary>
	/// The items we are looking to find.
	/// </summary>
	protected Dictionary<ItemAsset, TransactionCriteria> Requirements { get; set; } = new();

	/// <summary>
	/// The containers the transaction will try to search from.
	/// </summary>
	protected List<Container> Containers { get; set; } = new();

	/// <summary>
	/// The items gathered.
	/// </summary>
	protected Dictionary<ItemAsset, TransactionCriteria> Gathered { get; set; } = new();

	protected int GatheredCount { get; set; } = 0;

	protected Dictionary<ItemAsset, TransactionCriteria> CreateCriteriaHolder()
	{
		var holder = new Dictionary<ItemAsset, TransactionCriteria>();
		foreach ( var keyValue in Requirements )
			holder.Add( keyValue.Key, new() );

		return holder;
	}

	public void AddContainer( params Container[] containers )
	{
		Containers.AddRange( containers );
	}

	public void AddRequirement( params ContainerTransactionItem[] requirements )
	{
		// Add requirements to the list
		requirements.ToList()
			.ForEach( x => Requirements.Add( x.ItemAsset, new TransactionCriteria( x.Quantity ) ) );
	}

	public bool CanAfford()
	{
		var holder = CreateCriteriaHolder();

		foreach ( var container in Containers )
		{
			Log.Info( $"Iterating container: {container.ID}" );

			foreach ( var slot in container.Items )
			{
				if ( slot.Item is not null && holder.ContainsKey( slot.Item.Asset ) )
				{
					var itemAsset = slot.Item.Asset;
					var criteria = holder[itemAsset];
					criteria.Quantity += slot.Quantity;

					if ( criteria.Quantity >= Requirements[itemAsset].Quantity )
						criteria.HasBeenMet = true;
				}
			}
		}

		foreach ( var criteria in holder.Values )
		{
			if ( !criteria.HasBeenMet )
			{
				Log.Info( "Criteria has not been met." );
				return false;
			}
		}

		return true;
	}

	public bool Execute()
	{
		if ( !CanAfford() ) return false;

		// Copy
		Gathered = new( Requirements );

		foreach ( var container in Containers )
		{
			Log.Info( $"Iterating container: {container.ID}" );

			for ( int slotId = 0; slotId < container.Items.Count; slotId++ )
			{
				var slot = container.Items[slotId];

				if ( slot.Item is not null && Requirements.ContainsKey( slot.Item.Asset ) )
				{
					var itemAsset = slot.Item.Asset;
					var requirement = Requirements[itemAsset];

					var howMuch = requirement.Quantity;
					var slotQuantity = slot.Quantity;

					var resultingQuantity = slotQuantity - howMuch;

					// Overlap
					if ( resultingQuantity <= 0 )
					{
						Log.Info( "We're eliminating one item in its entirety" );
						container.Remove( slotId );
						var overlap = Math.Abs( resultingQuantity );
						requirement.Quantity = overlap;
					}
					else
					{
						Log.Info( "We're just changing the quantity of this item" );
						requirement.Quantity = 0;
						slot.SetQuantity( resultingQuantity );
					}

					if ( requirement.Quantity < 1 )
						GatheredCount++;

					if ( GatheredCount == Requirements.Count )
					{
						Log.Info( "The job is done!" );
						return true;
					}
				}
			}
		}

		return false;
	}
}
