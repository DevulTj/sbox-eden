// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class ConsumableItem : Item
{
	public override ItemType Type => ItemType.Consumable;
	public override HashSet<ItemActionType> ItemActions => new()
	{
		ItemActionType.Consume,
		ItemActionType.Split,
		ItemActionType.Drop
	};

	[ItemActionCheck( ItemActionType.Consume )]
	public bool CanConsume( Player player, Slot slotRef )
	{
		return true;
	}

	[ItemActionExec( ItemActionType.Consume, "Consume" )]
	public bool Consume( Player player, Slot slotRef )
	{
		Log.Info( "running consume" );

		return true;
	}
}
