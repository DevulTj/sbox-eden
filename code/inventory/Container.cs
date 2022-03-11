// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class Container : BaseNetworkable
{
	[Net]
	public IList<InventoryItem> Items { get; set; }

	public void Add( InventoryItem item, int slot = -1 )
	{
		//
	}

	public void Remove( InventoryItem item )
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
