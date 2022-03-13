// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Eden;
using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "/ui/containers/ContainerPanel.html" )]
public partial class HotbarContainerPanel : ContainerPanel
{
	public HotbarContainerPanel()
	{
	}

	bool _waiting = true;

	public override void Tick()
	{
		base.Tick();

		if ( !_waiting ) return;

		var player = Local.Pawn as Player;
		if ( player.IsValid() && player.Hotbar != null )
		{
			SetContainer( player.Hotbar );
			_waiting = false;
		}
	}
}
