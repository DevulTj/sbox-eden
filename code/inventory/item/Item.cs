// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class Item
{
	public virtual ItemType Type => ItemType.Item;

	public virtual void Write( NetWrite write )
	{
		//
	}

	public virtual void Read( NetRead read )
	{
		//
	}
}
