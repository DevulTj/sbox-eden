// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class EyeEntityVital : Panel
{
	public bool IsShowing { get; set; } = true;

	// @ref
	public string CurrentEntityName { get; set; }
	// @ref
	public string CurrentEntitySubtitle { get; set; }

	public EyeEntityVital()
	{
		BindClass( "show", () => IsShowing );
	}

	public override void Tick()
	{
		base.Tick();

		var player = Local.Pawn;

		// First try a direct 0 width line
		var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * 160 )
			.Ignore( player )
			.Run();

		IsShowing = false;

		if ( !tr.Hit )
		{
			IsShowing = false;
			return;
		}

		if ( tr.Entity is ItemEntity worldItem )
		{
			CurrentEntityName = worldItem.Asset.ItemName;
			CurrentEntitySubtitle = $"x{worldItem.Quantity}";

			IsShowing = true;

			SetClass( "resource", false );
		}


		if ( tr.Entity is ResourceNodeEntity resource )
		{
			CurrentEntityName = resource.ResourceAsset.ResourceName;
			CurrentEntitySubtitle = "";

			IsShowing = true;

			SetClass( "resource", true );
		}
	}
}
