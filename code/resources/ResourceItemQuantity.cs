// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public class ResourceItemQuantity
{
	public string ItemAssetName { get; set; }
	public int InitialAmount { get; set; }
	public int AmountRemaining;

	public ResourceItemQuantity()
	{
	}

	public ResourceItemQuantity( ResourceItemQuantity other )
	{
		ItemAssetName = other.ItemAssetName;
		InitialAmount = other.InitialAmount;
		AmountRemaining = other.AmountRemaining;
	}
}
