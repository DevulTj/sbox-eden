// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

[UseTemplate]
public partial class ContainerPanel : Panel
{
	public static Dictionary<Guid, ContainerPanel> Panels { get; set; } = new();

	public static ContainerPanel GetFromID( Guid guid ) => Panels.GetValueOrDefault( guid );

	public Container Container { get; set; }

	protected List<ItemPanel> Slots { get; set; } = new();

	// @text
	public string Title { get; set; } = "Inventory";

	// @ref
	public Panel ItemLayout { get; set; }

	public ContainerPanel()
	{
		AddClass( "containerpanel" );
	}

	public void SetContainer( Container container )
	{
		Container = container;
		Title = container.ID.ToString();

		Panels[container.ID] = this;

		Log.Info( $"{container.ID}" );

		Refresh();
	}

	public ItemPanel FindHoveredItem()
	{
		return Slots.FirstOrDefault( x => x.IsHovered );
	}

	public override void OnDeleted()
	{
		Panels[Container.ID] = null;

		base.OnDeleted();
	}

	public void Refresh()
	{
		ItemLayout.DeleteChildren( true );

		Slots.Clear();

		Container.Items.ToList().ForEach( x => AddSlot( x ) );
	}

	protected ItemPanel AddSlot( Slot slot )
	{
		var itemPanel = ItemLayout.AddChild<ItemPanel>();
		itemPanel.SetSlot( slot );
		itemPanel.SetPanel( this );

		Slots.Add( itemPanel );

		return itemPanel;
	}

	protected int GetSlotIndex( ItemPanel panel )
	{
		return Slots.IndexOf( panel );
	}

	public void HandleDrop( ItemPanel itemPanel )
	{
		ContainerNetwork.ContainerDrop( Container.ID.ToString(), GetSlotIndex( itemPanel ) );
	}
}
