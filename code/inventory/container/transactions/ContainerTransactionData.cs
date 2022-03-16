// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public class ContainerTransactionData
{
	public int Quantity { get; set; } = 0;

	public bool HasBeenMet { get; set; } = false;

	public ContainerTransactionData()
	{
	}

	public ContainerTransactionData( int quantity )
	{
		Quantity = quantity;
	}
}
