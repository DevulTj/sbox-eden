// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class WorldItemEntity : Prop
{
	[Net]
	public ItemAsset Asset { get; set; }

	public override void Spawn()
	{
		base.Spawn();
	}

	public void SetItem( Item item )
	{
		Asset = item.Asset;

		if ( item.Asset.WorldModel is not null )
			Model = item.Asset.WorldModel;
	}
}
