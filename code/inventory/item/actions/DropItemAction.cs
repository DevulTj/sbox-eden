// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class DropItemAction : ItemAction
{
	public override string ID => Drop;
	public override string DisplayName => "Drop";

	public override bool CanDo( Player player, Slot slot )
	{
		return true;
	}

	public override int Execute( Player player, Slot slot )
	{
		var entity = ItemEntity.Create( player, slot.Item, slot.Quantity );
		return entity.IsValid() ? slot.Quantity : 0;
	}
}
