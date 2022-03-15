// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Eden;

public partial class CraftingCategoryButton : Button
{
	public string GetCategoryImage( CraftingCategory category )
	{
		return category switch
		{
			CraftingCategory.Misc => "/ui/items/water_bottle.png",
			CraftingCategory.Building => "/ui/items/water_bottle.png",
			CraftingCategory.Food => "/ui/items/water_bottle.png",
			CraftingCategory.Tools => "/ui/items/stone_hatchet.png",
			CraftingCategory.Weapons => "/ui/items/stone_hatchet.png",
			CraftingCategory.Farming => "/ui/items/water_bottle.png",
			CraftingCategory.Clothing => "/ui/items/water_bottle.png",
			_ => "/ui/items/water_bottle.png",
		};
	}

	public CraftingCategoryButton( CraftingCategory category )
	{
		AddClass( "category" );
		//
		Add.Image( GetCategoryImage( category ), "icon" );
		Add.Label( "0", "amount" );
	}
}

[UseTemplate]
public partial class CraftingMenuPanel : Panel
{
	public List<CraftingCategoryButton> CategoryButtons { get; set; } = new();

	// @ref
	public Panel CategoryLayout { get; set; }
	// @ref
	public Panel ItemsLayout { get; set; }
	// @ref
	public Panel InspectorPanel { get; set; }

	public CraftingMenuPanel()
	{
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();

		CategoryLayout.DeleteChildren();
		CategoryButtons.Clear();

		foreach ( CraftingCategory category in Enum.GetValues( typeof( CraftingCategory ) ) )
		{
			var button = new CraftingCategoryButton( category );
			button.Parent = CategoryLayout;
		}
	}
}
