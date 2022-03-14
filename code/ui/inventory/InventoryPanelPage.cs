// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using System.Collections.Generic;

namespace Eden;

[UseTemplate]
public partial class InventoryPanelPage : Panel
{
	public static List<InventoryPanelPage> All { get; set; }
	public static InventoryPanelPage Active { get; set; }

	public virtual string PageName => "PAGE";

	public override void SetProperty( string name, string value )
	{
		base.SetProperty( name, value );
	}
}
