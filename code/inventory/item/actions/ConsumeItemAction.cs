// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public partial class ConsumeItemAction : ItemAction
{
	public override string ID => Consume;
	public override string DisplayName => "Consume";

	public override bool CanDo( Player player, Slot slot )
	{
		return true;
	}

	public override bool Execute( Player player, Slot slot )
	{
		return true;
	}
}
