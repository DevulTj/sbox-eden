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

			if ( Rand.Int( 2 ) < 2 )
				slot.SetItem( ItemAsset.Random );
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

	public void Add( Item item, int slot = -1 )
	{
		if ( slot == -1 )
			slot = FindEmptySlot();

		if ( slot == -1 )
			return;

		Items[slot].SetItem( item );
	}

	public void Remove( Slot slot )
	{
		slot.RemoveItem();
	}

	public void Remove( int slot )
	{
		Items[slot].RemoveItem();
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

	}

	public Slot GetSlot( int slotA )
	{
		if ( slotA >= Items.Count )
			return null;

		return Items[slotA];
	}
}
