// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class ItemPanel : Panel
{
	// @ref
	public Image IconPanel { get; set; }
	// @text
	public int Quantity => Slot?.Quantity ?? 0;

	public Slot Slot { get; protected set; }
	public Item Item { get; protected set; }
	public ContainerPanel ContainerPanel { get; protected set; }

	public bool IsHovered { get; set; } = false;

	public ItemPanel()
	{
	}

	public void SetPanel( ContainerPanel panel )
	{
		ContainerPanel = panel;
	}

	public void SetSlot( Slot slot )
	{
		Slot = slot;

		SetItem( slot.Item );
	}

	public void SetItem( Item item )
	{
		SetClass( "empty", item is null );

		if ( item is null || item.Asset is null )
			return;

		Item = item;

		IconPanel.SetTexture( item.Asset.IconPath );

		if ( this is not DraggedItemPanel )
			Style.Set( "background-image", $"linear-gradient( 23deg, rgba( {Item.DefaultColor.Hex}, 0.4 ), rgba( #888888, 0 ) );" );
	}

	public override void SetProperty( string name, string value )
	{
		base.SetProperty( name, value );

		if ( name == "asset" )
		{
			var asset = ItemAsset.Classes[value];
			var item = asset.Type.Instantiate();
			item.Asset = asset;

			SetItem( item );
		}
	}

	protected override void OnRightClick( MousePanelEvent e )
	{
		base.OnRightClick( e );

		ContainerPanel.HandleDrop( this );
	}

	protected override void OnMouseOver( MousePanelEvent e )
	{
		base.OnMouseOver( e );

		AddClass( "hovered-item" );

		IsHovered = true;
	}

	protected override void OnMouseOut( MousePanelEvent e )
	{
		base.OnMouseOut( e );

		RemoveClass( "hovered-item" );
		IsHovered = false;
	}
}
