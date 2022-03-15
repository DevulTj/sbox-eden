// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Eden;

public partial class CraftingCategoryButton : Button
{
	public string GetCategoryImage( ItemCategory category )
	{
		return category switch
		{
			ItemCategory.Misc => "/ui/items/water_bottle.png",
			ItemCategory.Building => "/ui/items/water_bottle.png",
			ItemCategory.Food => "/ui/items/water_bottle.png",
			ItemCategory.Tools => "/ui/items/stone_hatchet.png",
			ItemCategory.Weapons => "/ui/items/stone_hatchet.png",
			ItemCategory.Farming => "/ui/items/water_bottle.png",
			ItemCategory.Clothing => "/ui/items/water_bottle.png",
			_ => "/ui/items/water_bottle.png",
		};
	}

	public CraftingCategoryButton( ItemCategory category )
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

		foreach ( ItemCategory category in Enum.GetValues( typeof( ItemCategory ) ) )
		{
			var button = new CraftingCategoryButton( category );
			button.Parent = CategoryLayout;
		}

		foreach ( var item in ItemAsset.All )
		{
			var category = item.Category;
		}
	}
}
