// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Eden;

public partial class CraftingCategoryButton : Button
{
	// @text
	public int ItemCount { get; set; } = 0;
	public Label AmountLabel { get; set; }

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
		AmountLabel = Add.Label( "0", "amount" );
	}

	public void SetCount( int count )
	{
		ItemCount = count;
		AmountLabel.Text = $"{ItemCount}";
	}
}

public partial class CraftingItemButton : Button
{
	public CraftingItemButton( ItemAsset item )
	{
		AddClass( "item" );
		//
		Add.Image( item.IconPath, "icon" );
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

	public void SetCategory( ItemCategory category )
	{
		ItemsLayout.DeleteChildren( true );

		foreach ( var item in ItemAsset.All )
		{
			if ( category != item.Category )
				continue;

			var itemButton = new CraftingItemButton( item );
			itemButton.Parent = ItemsLayout;
		}
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();

		CategoryLayout.DeleteChildren( true );
		CategoryButtons.Clear();

		foreach ( ItemCategory category in Enum.GetValues( typeof( ItemCategory ) ) )
		{
			var button = new CraftingCategoryButton( category );
			button.Parent = CategoryLayout;
			button.AddEventListener( "onclick", () => SetCategory( category ) );

			button.SetCount( ItemAsset.FromCategory( category ).Count );
		}

		SetCategory( ItemCategory.Misc );
	}
}
