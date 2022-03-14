// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "ui/hud/vitals/vitalentry.html" )]
public partial class WetnessVitalPanel : VitalEntry
{
	public string IconPath => "/ui/hud/vitals/wetness.png";

	public WetnessVitalPanel() : base()
	{
		Icon.Style.SetBackgroundImage( IconPath );

		BindClass( "invisible", () => BoundValue < 5f );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn as Player;
		var vital = player.GetVital( "Wetness" );

		BoundValue = vital.Value.CeilToInt();
		BoundMaxValue = vital.MaxValue.CeilToInt();
		FormattedValue = vital.ValueFormat;
	}
}
