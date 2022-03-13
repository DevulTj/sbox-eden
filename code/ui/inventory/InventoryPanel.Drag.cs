using Sandbox;
using Sandbox.UI;

namespace Eden;

public partial class InventoryPanel
{
	public ItemPanel FocusedItem { get; set; }
	public DraggedItemPanel Dummy { get; set; }

	public Vector2 StartMousePos { get; set; }
	public bool IsDragging { get; set; }

	protected override void OnMouseDown( MousePanelEvent e )
	{
		base.OnMouseDown( e );

		if ( e.Button != "mouseleft" )
			return;

		var hoveredItem = FindHoveredItem();
		if ( hoveredItem == null )
			return;

		IsDragging = false;
		StartMousePos = Mouse.Position / Screen.Size;
		FocusedItem = hoveredItem;
	}

	protected void StartDragging( ItemPanel item )
	{
		IsDragging = true;

		Dummy?.Delete( true );

		var draggedItem = new DraggedItemPanel();
		draggedItem.SetSlot( item.Slot );

		Dummy = draggedItem;
		AddChild( draggedItem );

		item.SetClass( "being-dragged", true );
	}

	protected void StopDragging()
	{
		IsDragging = false;

		if ( Dummy == null )
			return;

		var item = FocusedItem;
		if ( item != null )
		{
			var destination = FindHoveredItem();

			if ( destination != null )
			{
				var originPanel = item.ContainerPanel;

				var destinationPanel = destination.ContainerPanel;

				if ( originPanel.Container.ID == destinationPanel.Container.ID )
				{
					ContainerNetwork.ContainerMove(
						destinationPanel.Container.ID.ToString(),
						destinationPanel.GetSlotIndex( item ),
						destinationPanel.GetSlotIndex( destination )
					);
				}
				else
				{
					ContainerNetwork.ContainerMoveExternal(
						originPanel.Container.ID.ToString(),
						originPanel.GetSlotIndex( item ),
						//
						destinationPanel.Container.ID.ToString(),
						destinationPanel.GetSlotIndex( destination )
					);
				}
			}
		}

		Dummy.Delete();
		FocusedItem?.SetClass( "being-dragged", false );
		FocusedItem = null;
	}

	protected ItemPanel FindHoveredItem()
	{
		var inventoryItem = Inventory.FindHoveredItem();
		if ( inventoryItem != null ) return inventoryItem;

		var hotbarItem = Hotbar.FindHoveredItem();
		if ( hotbarItem != null ) return hotbarItem;

		return null;
	}

	protected override void OnMouseUp( MousePanelEvent e )
	{
		base.OnMouseUp( e );

		if ( e.Button == "mouseleft" )
			StopDragging();
		else if ( e.Button == "mouseright" )
		{
			var hoveredItem = FindHoveredItem();
			// Do stuff?
		}
	}

	[Event.Tick.ClientAttribute]
	protected void TickInternalDrag()
	{
		var mousePos = Mouse.Position / Screen.Size;

		if ( FocusedItem == null )
		{
			IsDragging = false;
			return;
		}

		// Have we moved a little, and are not dragging already?
		if ( mousePos.Distance( StartMousePos ) > .005 && !IsDragging )
			StartDragging( FocusedItem );
	}
}
