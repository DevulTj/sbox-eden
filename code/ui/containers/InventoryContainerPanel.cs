// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Eden;
using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "/ui/containers/ContainerPanel.html" )]
public partial class InventoryContainerPanel : ContainerPanel
{
	[GameEvent.Client.BackpackChanged]
	protected void Setup( Container container )
	{
		SetContainer( container );
	}

	public InventoryContainerPanel()
	{
	}
}
