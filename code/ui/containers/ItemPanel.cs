// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class ItemPanel : Panel
{
	// @ref
	public Panel IconPanel { get; set; }
	// @text
	public int Quantity { get; set; } = 1;

	protected Item Item { get; set; }

	public ItemPanel()
	{
	}

	public void SetItem( Item item )
	{
		Item = item;

		IconPanel.Style.SetBackgroundImage( item.Asset.IconPath );
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
}
