// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class HotbarContainer : Container
{
	public HotbarContainer()
	{
	}

	public HotbarContainer( Player owner )
	{
		Owner = owner;
	}

	[Net]
	public Player Owner { get; set; }

	[Net]
	public Slot ActiveSlot { get; set; }

	public void SetActiveSlot( int index )
	{
		if ( index >= Items.Count ) return;

		var slot = Items[index];
		SetActiveSlot( slot );
	}

	public void SetActiveSlot( Slot slot )
	{
		// Allow to toggle by hitting the same slot
		if ( slot == ActiveSlot )
			ActiveSlot = null;
		else
			ActiveSlot = slot;
	}
}
