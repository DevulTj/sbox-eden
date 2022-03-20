// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class ItemActionButton : Button
{
	public ItemActionButton( ItemAction type )
	{
		AddClass( "action" );
		Add.Label( type.DisplayName, "name" );
	}
}

[UseTemplate]
public partial class ItemInspectPanel : Panel
{
	public static ItemInspectPanel Instance { get; set; }

	public ItemInspectPanel()
	{
		Instance = this;
	}

	// @ref
	public Label ItemNameLabel { get; set; }
	// @ref
	public Label ItemDescLabel { get; set; }
	// @ref
	public Image ItemIcon { get; set; }
	// @ref
	public Panel ActionsLayout { get; set; }

	public void Clear()
	{
		RemoveClass( "visible" );
	}

	public void SetItem( ItemPanel itemPanel )
	{
		if ( itemPanel.Item is null )
		{
			Clear();
			return;
		}

		AddClass( "visible" );

		ItemIcon.SetTexture( itemPanel.Item.Asset.IconPath );
		ItemNameLabel.Text = itemPanel.Item.Asset.ItemName;
		ItemDescLabel.Text = itemPanel.Item.Asset.ItemDescription;

		ActionsLayout.DeleteChildren( true );

		var player = Local.Pawn as Player;
		var item = itemPanel.Item;
		var container = itemPanel.ContainerPanel.Container;
		var slotIndex = itemPanel.ContainerPanel.GetSlotIndex( itemPanel );

		var itemActions = new List<ItemAction>();
		item.GatherActions( ref itemActions );

		foreach ( var type in itemActions )
		{
			if ( !item.CanDoAction( player, type.ID, itemPanel.Slot ) )
				continue;

			var actionButton = new ItemActionButton( type );
			actionButton.Parent = ActionsLayout;
			actionButton.AddEventListener( "onclick", () =>
			{
				ContainerNetwork.DoItemAction( container.ID.ToString(), slotIndex, type.ID );
				Clear();
			} );
		}
	}
}
