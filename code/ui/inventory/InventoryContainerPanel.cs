// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Eden;
using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "/ui/containers/ContainerPanel.html" )]
public partial class InventoryContainerPanel : ContainerPanel
{
	public InventoryContainerPanel()
	{
	}

	bool _waiting = true;

	public override void Tick()
	{
		base.Tick();

		if ( !_waiting ) return;

		var player = Local.Pawn as Player;
		if ( player.IsValid() && player.Backpack != null )
		{
			SetContainer( player.Backpack );
			_waiting = false;
		}
	}
}
