// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class Container : BaseNetworkable
{
	public override string ToString() => $"[{Size}]( {string.Join( ", ", Items )} )";

	[Net]
	public int Size { get; protected set; } = 12;

	[Net]
	public IList<Slot> Items { get; set; }

	/// <summary>
	/// Used to interact with the Container over the network
	/// </summary>
	[Net]
	public Guid ID { get; set; }

	public Container()
	{
		if ( Host.IsServer )
		{
			ContainerNetwork.Register( this );
		}
	}

	~Container()
	{
		if ( Host.IsServer )
		{
			ContainerNetwork.Dispose( this );
		}
	}

	public void SetSize( int size )
	{
		Items.Clear();

		Size = size;

		for ( int i = 0; i < size; i++ )
		{
			var slot = new Slot();
			Items.Add( slot );
		}
	}

	public int FindEmptySlot()
	{
		for ( int i = 0; i < Items.Count; i++ )
		{
			var slot = Items[i];
			if ( slot.Item is null )
				return i;
		}

		return -1;
	}

	public int Add( Item item, int quantity = 1 )
	{
		int quantityLeft = quantity;
		int maxStack = item.MaxStack;

		// Try and fill items that exist
		int lastSlot = -1;
		for ( int i = 0; i < Items.Count; i++ )
		{
			var slot = Items[i];

			if ( slot.Item is not null && slot.Quantity <= maxStack )
			{
				var availableSpace = maxStack - slot.Quantity;
				var amountToAdd = Math.Min( availableSpace, quantity );

				slot.SetQuantity( slot.Quantity + amountToAdd );

				quantityLeft -= amountToAdd;
				lastSlot = i;
			}
		}

		// Now let's go through empty slots
		while ( quantityLeft > 0 )
		{
			var newQuantity = quantity.Clamp( 1, maxStack );
			var slot = FindEmptySlot();

			if ( slot != -1 )
			{
				Items[slot].SetItem( item );
				Items[slot].SetQuantity( newQuantity );

				OnItemAdded( item, slot );

				return slot;
			}
			else
			{
				return lastSlot;
			}
		}

		return lastSlot;
	}

	public void Remove( int slot )
	{
		Items[slot].RemoveItem();

		OnItemRemoved( slot );
	}

	public void Move( int slotA, int slotB, Container destination = null )
	{
		var slotAItem = Items[slotA].Item;
		var slotBItem = destination is not null ? destination.Items[slotB].Item : Items[slotB].Item;

		Items[slotA].SetItem( slotBItem );

		if ( destination is not null )
			destination.Items[slotB].SetItem( slotAItem );
		else
			Items[slotB].SetItem( slotAItem );

		OnItemMoved( slotA, slotB, destination );
	}

	public Slot GetSlot( int slotA )
	{
		if ( slotA >= Items.Count )
			return null;

		return Items[slotA];
	}

	protected virtual void OnItemMoved( int slotA, int slotB, Container destination = null )
	{
		//
	}

	protected virtual void OnItemRemoved( int slotA )
	{
		//
	}

	protected virtual void OnItemAdded( Item item, int slot )
	{
		//
	}
}
