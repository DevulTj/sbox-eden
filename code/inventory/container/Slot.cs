// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class Slot : BaseNetworkable, INetworkSerializer
{
	public override string ToString() => Item is null ? "Empty" : Item.ToString();

	public Item Item { get; set; } = null;

	public void SetItem( ItemAsset asset )
	{
		Item = asset.Type.Instantiate();
		Item.Asset = asset;
	}

	void INetworkSerializer.Read( ref NetRead read )
	{
		var hasItem = read.Read<bool>();
		if ( !hasItem )
			return;

		var type = read.Read<ItemType>();
		var newItem = type.Instantiate();
		newItem.Read( read );

		Item = newItem;
	}

	void INetworkSerializer.Write( NetWrite write )
	{
		write.Write( Item is not null );

		if ( Item is not null )
		{
			write.Write( Item.Type );
			Item.Write( write );
		}
	}
}
