// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class Container : BaseNetworkable
{
	[Net] public int Size { get; protected set; } = 12;

	public IList<Slot> Items { get; set; }

	public Container()
	{
	}

	public void SetSize( int size )
	{
		Items.Clear();

		Size = size;

		for ( int i = 0; i < size; i++ )
		{
			Items.Add( new Slot() );
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
}
