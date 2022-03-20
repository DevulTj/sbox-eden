// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class DropSingleItemAction : ItemAction
{
	public override string ID => DropSingle;
	public override string DisplayName => "Drop One";

	public override bool CanDo( Player player, Slot slot )
	{
		return slot.Quantity > 1;
	}

	public override int Execute( Player player, Slot slot )
	{
		var entity = ItemEntity.Create( player, slot.Item, 1 );
		return entity.IsValid() ? 1 : 0;
	}
}
