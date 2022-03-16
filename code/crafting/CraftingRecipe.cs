// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;

namespace Eden;

public class CraftingEntry
{
	[Property]
	public string ItemId { get; set; }
	[Property]
	public int Amount { get; set; } = 0;
}

public class CraftingRecipe
{
	[Property]
	public List<CraftingEntry> Items { get; set; }

	[Property]
	public int Output { get; set; } = 1;
}
