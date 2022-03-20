// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using System;

namespace Eden;

public partial class ConsumeItemAction : ItemAction
{
	public override string ID => Consume;
	public override string DisplayName => "Consume";

	public override bool CanDo( Player player, Slot slot )
	{
		return true;
	}

	public override int Execute( Player player, Slot slot )
	{
		var itemAsset = slot.Item.Asset as ConsumableItemAsset;
		if ( itemAsset is null )
			throw new Exception( "We somehow have the wrong item asset type here. Fix this." );

		if ( itemAsset.Hunger > 0f )
		{
			var vital = player.GetVital( "Hunger" );
			vital.Value = Math.Clamp( vital.Value + itemAsset.Hunger, 0, vital.MaxValue );
		}
		if ( itemAsset.Thirst > 0f )
		{
			var vital = player.GetVital( "Thirst" );
			vital.Value = Math.Clamp( vital.Value + itemAsset.Thirst, 0, vital.MaxValue );
		}
		if ( itemAsset.Health > 0f )
		{
			player.Health = Math.Clamp( player.Health + itemAsset.Health, 0, player.MaxHealth );
		}

		return 1;
	}
}
