// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class InventoryPanel : Panel
{
	public bool IsOpen { get; protected set; } = false;

	// @ref
	public InventoryContainerPanel Inventory { get; set; }

	public InventoryPanel()
	{
		BindClass( "open", () => IsOpen );
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();
	}

	public override void Tick()
	{
		base.Tick();

		if ( Input.Pressed( InputButton.Score ) )
		{
			IsOpen = !IsOpen;
		}
	}
}
