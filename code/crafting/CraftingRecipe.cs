// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public struct CraftingRecipe
{
	public bool IsValid => ItemAsset.Classes.ContainsKey( ItemId ) && Amount > 0;

	public string ItemId { get; set; }
	public int Amount { get; set; } = 0;
}
