// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using System.Collections.Generic;

namespace Eden;

[UseTemplate]
public partial class MainMenuPage : Panel
{
	public static List<MainMenuPage> All { get; set; }
	public static MainMenuPage Active { get; set; }

	public virtual string PageName => "PAGE";

	public override void SetProperty( string name, string value )
	{
		base.SetProperty( name, value );
	}
}
