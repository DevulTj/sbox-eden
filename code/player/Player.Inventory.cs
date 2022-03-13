// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

public partial class Player
{
	[Net, Local]
	public Container Backpack { get; protected set; }
	// @IContainerEntity
	public Container Container { get => Backpack; set => Backpack = value; }

	[Net, Local]
	public Container Hotbar { get; protected set; }

	protected void PrintBackpack()
	{
		var position = Host.IsServer ? new Vector2( 100, 100 ) : new Vector2( 100, 200 );

		DebugOverlay.ScreenText( position, 0, Color.White,
			$"{( Host.IsServer ? "[Server]" : "[Client]" )} Inventory of \"{Client?.Name ?? "Unknown player"}\"\n" + Backpack );
	}
}
