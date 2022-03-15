// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using System;

namespace Eden;

[UseTemplate( "ui/hud/vitals/vitalentry.html" )]
public partial class CraftingVital : VitalEntry
{
	public string IconPath => "/ui/hud/vitals/crafting.png";

	public CraftingVital() : base()
	{
		Icon.Style.SetBackgroundImage( IconPath );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as Player;
		if ( !player.IsValid() )
			return;

		var queue = player.CraftingQueue;

		SetClass( "invisible", queue.CurrentCraft is null );

		if ( queue.CurrentCraft is not null )
		{
			TimeSpan t = TimeSpan.FromSeconds( queue.CraftFinished );
			BoundValue = queue.CraftStarted;
			BoundMaxValue = queue.CurrentCraft.Asset.CraftingDuration;
			FormattedValue = $"{t.Seconds:D1}s";
		}
	}
}
