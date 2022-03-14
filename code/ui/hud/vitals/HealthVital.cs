// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate( "ui/hud/vitals/vitalentry.html" )]
public partial class HealthVital : VitalEntry
{
	public string IconPath => "/ui/hud/vitals/health.png";

	public HealthVital() : base()
	{
		Icon.Style.SetBackgroundImage( IconPath );
	}

	public override void Tick()
	{
		base.Tick();

		BoundValue = Local.Pawn?.Health ?? 100f;
		FormattedValue = $"{BoundValue}";
	}
}
