// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;

namespace Eden;

public partial class Crosshair : Panel
{
	public static Crosshair Current { get; set; }

	public Crosshair()
	{
		Current = this;

		StyleSheet.Load( "/ui/hud/Crosshair.scss" );
	}

	public static void SetCrosshair( Panel crosshairPanel )
	{
		if ( Current == null )
			return;

		Current.DeleteChildren();
		crosshairPanel.Parent = Current;
	}
}
