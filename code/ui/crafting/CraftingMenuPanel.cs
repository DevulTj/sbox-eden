// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
			ItemCategory.Misc => "/ui/items/categories/misc.png",
			ItemCategory.Building => "/ui/items/categories/building.png",
			ItemCategory.Food => "/ui/items/categories/food.png",
			ItemCategory.Tools => "/ui/items/categories/tools.png",
			ItemCategory.Weapons => "/ui/items/categories/weapons.png",
			ItemCategory.Farming => "/ui/items/categories/farming.png",
			ItemCategory.Clothing => "/ui/items/categories/clothing.png",
			_ => "/ui/items/categories/misc.png",
		};
	}

	public CraftingCategoryButton( ItemCategory category )
	{
		AddClass( "category" );
		//
		Add.Image( GetCategoryImage( category ), "icon" );
		Add.Label( category.ToString(), "name" );
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
	public ItemAsset ItemAsset { get; set; }
	protected Label AmountLabel { get; set; }

	public CraftingItemButton( ItemAsset item )
	{
		ItemAsset = item;

		AddClass( "item" );
		//
		Add.Image( item.IconPath, "icon" );
		AmountLabel = Add.Label( "0", "amount" );
		Add.Label( item.ItemName, "name" );
	}

	public override void Tick()
	{
		base.Tick();

		int count = 0;

		var player = Local.Pawn as Player;
		if ( player.IsValid() )
		{
			var queue = player.CraftingQueue;
			foreach ( var queueItem in queue.Queue )
			{
				if ( queueItem.Asset.Id == ItemAsset.Id )
					count += queueItem.Quantity;
			}

			AmountLabel.SetClass( "show", count > 0 );
			AmountLabel.Text = $"{count}";
		}
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
	public CraftingMenuInspector Inspector { get; set; }
	// @ref
	public Panel QueueLayout { get; set; }
	public ItemCategory CurrentCategory { get; set; }

	public CraftingMenuPanel()
	{
	}

	public void SetItem( ItemAsset item, CraftingItemButton button )
	{
		Inspector.SetItem( item );
	}

	public void SetCategory( ItemCategory category )
	{
		CurrentCategory = category;
		ItemsLayout.DeleteChildren( true );

		foreach ( var item in ItemAsset.FromCategory( category ) )
		{
			if ( category != item.Category )
				continue;

			var itemButton = new CraftingItemButton( item );
			itemButton.Parent = ItemsLayout;
			itemButton.AddEventListener( "onclick", () => SetItem( item, itemButton ) );
			itemButton.BindClass( "current", () => Inspector.ItemAsset == item );

			if ( Inspector.ItemAsset is null )
				SetItem( item, itemButton );
		}
	}

	protected async Task RunCategoryDelayHack()
	{
		await GameTask.DelaySeconds( 0.1f );

		SetCategory( ItemCategory.Tools );
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

			button.BindClass( "current", () => CurrentCategory == category );
			button.SetCount( ItemAsset.FromCategory( category ).Count );
		}

		_ = RunCategoryDelayHack();
	}
}
