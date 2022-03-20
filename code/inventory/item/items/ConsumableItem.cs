// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class ConsumableItem : Item
{
	public override ItemType Type => ItemType.Consumable;

	[ItemActionCheck( ItemActionType.Consume )]
	public bool CanConsume( Player player )
	{
		return true;
	}

	[ItemActionExec( ItemActionType.Consume )]
	public bool Consume( Player player )
	{
		return true;
	}
}
