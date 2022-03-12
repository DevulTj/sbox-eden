// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;
using System.Linq;

namespace Eden;

[UseTemplate]
public partial class ContainerPanel : Panel
{
	public Container Container { get; set; }

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

		Log.Info( $"{container.ID}" );

		Refresh();
	}

	protected void Refresh()
	{
		ItemLayout.DeleteChildren( true );

		Container.Items.ToList().ForEach( x => AddSlot( x ) );
	}

	protected ItemPanel AddSlot( Slot slot )
	{
		var itemPanel = ItemLayout.AddChild<ItemPanel>();
		itemPanel.SetItem( slot.Item );

		return itemPanel;
	}
}
