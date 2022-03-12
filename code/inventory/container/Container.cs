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

	[Net, Change( nameof( OnItemsChanged ) )]
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

	protected void OnItemsChanged( IList<Slot> before, IList<Slot> after )
	{
		Log.Info( "Items changed" );
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

	public void Add( Item item, int slot = -1 )
	{
		//
	}

	public void Remove( Item item )
	{
		//
	}

	public void Remove( int slot )
	{
		// 
	}

	public void Move( int slotA, int slotB )
	{
		//
	}

	public void Transfer( int slotA, Container destination, int slotB )
	{
		//
	}

	public Slot GetSlot( int slotA )
	{
		if ( slotA >= Items.Count )
			return null;

		return Items[slotA];
	}
}
