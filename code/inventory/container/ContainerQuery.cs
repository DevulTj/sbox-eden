// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class ContainerQuery
{
	/// <summary>
	/// The items we are looking to find.
	/// </summary>
	public Dictionary<ItemAsset, int> Results { get; protected set; } = new();

	/// <summary>
	/// The containers the transaction will try to search from.
	/// </summary>
	protected List<Container> Containers { get; set; } = new();

	public void AddContainer( params Container[] containers )
	{
		Containers.AddRange( containers );
	}

	public void AddItems( params ItemAsset[] items )
	{
		items.ToList().ForEach( x => Results.Add( x, 0 ) );
	}

	public void Execute()
	{
		foreach ( var container in Containers )
		{
			foreach ( var slot in container.Items )
			{
				if ( slot.Item is not null && Results.ContainsKey( slot.Item.Asset ) )
				{
					var itemAsset = slot.Item.Asset;
					Results[itemAsset] += slot.Quantity;
				}
			}
		}
	}
}
