// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class CraftingMenuRecipeItem : Panel
{
	public CraftingMenuRecipeItem()
	{
		AddClass( "recipe-item" );
	}

	public void SetItem( CraftingEntry entry, bool canAfford, int weHave )
	{
		var asset = entry.ItemAsset;
		var our = Add.Label( $"({weHave})", "recipe-our" );
		var amount = Add.Label( $"{entry.Amount}", "recipe-amount" );
		var name = Add.Label( $"{asset.ItemName}", "recipe-name" );

		SetClass( "can-afford", canAfford );
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
		RecipeLayout.DeleteChildren( true );

		var query = new ContainerQuery();
		var player = Local.Pawn as Player;
		query.AddContainer( player.Containers );
		query.AddItems( ItemAsset.Recipe.Items.Select( x => x.ItemAsset ).ToArray() );
		query.Execute();

		foreach ( var item in ItemAsset.Recipe.Items )
		{
			var recipeItem = RecipeLayout.AddChild<CraftingMenuRecipeItem>();
			var asset = item.ItemAsset;
			var weHave = query.Results[asset];
			var canAfford = weHave >= item.Amount;

			recipeItem.SetItem( item, canAfford, weHave );

			CraftButton.SetClass( "cant-afford", !canAfford );
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
