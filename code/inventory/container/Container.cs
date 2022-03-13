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
		Items[slot].SetQuantity( 1 );

		OnItemRemoved( slot );
	}

	public void Move( int slotAIndex, int slotBIndex, Container destination = null )
	{
		Slot slotA = Items[slotAIndex];
		Slot slotB = destination is not null ? destination.Items[slotBIndex] : Items[slotBIndex];

		Item slotAItem = slotA.Item;
		if ( slotAItem is null ) return;

		Item slotBItem = slotB.Item;

		bool sameType = slotAItem.IsSame( slotBItem );
		if ( sameType )
		{
			// Combine items together
			// How many items can we move from A to B?
			int availableSpaceOnB = slotBItem.MaxStack - slotB.Quantity;
			// Clamp it to our available items
			int amountToAdd = Math.Min( availableSpaceOnB, slotA.Quantity );
			// Update destination's slots
			slotB.SetQuantity( slotB.Quantity + amountToAdd );

			int newQuantityForA = slotA.Quantity - amountToAdd;
			if ( newQuantityForA < 1 )
				Remove( slotAIndex );
			else
				slotA.SetQuantity( newQuantityForA );

			OnItemMoved( slotAIndex, slotBIndex, destination );
		}
		else
		{
			int quantityOfA = slotA.Quantity;

			// Set new A
			slotA.SetItem( slotBItem );
			slotA.SetQuantity( slotB.Quantity );
			// Set new B
			slotB.SetItem( slotAItem );
			slotB.SetQuantity( quantityOfA );

			OnItemMoved( slotAIndex, slotBIndex, destination );
		}
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
