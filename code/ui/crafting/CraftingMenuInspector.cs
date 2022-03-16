// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Eden;

public partial class CraftingMenuRecipeItem : Panel
{
	public CraftingMenuRecipeItem()
	{
		AddClass( "recipe-item" );
	}

	public void SetItem( CraftingEntry entry )
	{
		var asset = ItemAsset.FromName( entry.ItemId );
		Add.Label( $"{entry.Amount}", "recipe-amount" );
		Add.Label( $"{asset.ItemName}", "recipe-name" );
	}
}

[UseTemplate]
public partial class CraftingMenuInspector : Panel
{
	public ItemAsset ItemAsset { get; set; }

	// @ref
	public Image ItemIcon { get; set; }
	// @ref
	public Label ItemName { get; set; }
	// @ref
	public Label ItemDescription { get; set; }
	// @ref
	public Button CraftButton { get; set; }
	// @ref
	public Panel RecipeLayout { get; set; }

	//
	public int Quantity { get; set; }

	public CraftingMenuInspector()
	{
		CraftButton.AddEventListener( "onclick", () => PlayerCraftingQueue.Craft( ItemAsset.Id, 1 ) );
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();
	}

	protected void SetupRecipe()
	{
		RecipeLayout.DeleteChildren();

		foreach ( var item in ItemAsset.Recipe.Items )
		{
			var recipeItem = RecipeLayout.AddChild<CraftingMenuRecipeItem>();
			recipeItem.SetItem( item );
		}
	}

	public void SetItem( ItemAsset item )
	{
		ItemAsset = item;
		ItemIcon.SetTexture( item.IconPath );
		ItemName.Text = item.ItemName;
		ItemDescription.Text = item.ItemDescription;

		SetupRecipe();
	}
}
