// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "ui/hud/vitals/vitalentry.html" )]
public partial class HungerVital : VitalEntry
{
	public string IconPath => "/ui/hud/vitals/hunger.png";

	public HungerVital() : base()
	{
		Icon.SetTexture( IconPath );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as Player;
		var vital = player.GetVital( "Hunger" );

		BoundValue = vital.Value.CeilToInt();
		BoundMaxValue = vital.MaxValue.CeilToInt();
		FormattedValue = vital.ValueFormat;
	}
}
