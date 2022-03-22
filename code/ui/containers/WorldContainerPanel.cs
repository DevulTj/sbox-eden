// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Eden;
using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "/ui/containers/ContainerPanel.html" )]
public partial class WorldContainerPanel : ContainerPanel
{
	public static WorldContainerPanel Instance { get; set; }

	[GameEvent.Client.WorldContainerChanged]
	protected void Setup( Container container )
	{
		SetContainer( container );
	}

	[GameEvent.Client.MainMenuClosed]
	protected void MainMenuClosed()
	{
		SetContainer( null );
	}

	public WorldContainerPanel()
	{
		Instance = this;

		BindClass( "hidden", ShouldHide );
	}

	protected bool ShouldHide()
	{
		return Container is null || !Container.HasAccess( Local.Pawn as Player );
	}
}
