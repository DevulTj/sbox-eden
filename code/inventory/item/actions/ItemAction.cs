// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public partial class ItemAction
{
	public virtual string ID { get; set; } = "action";
	public virtual string DisplayName { get; set; } = "Action";

	public virtual bool CanDo( Player player, Slot slot )
	{
		return true;
	}

	public virtual bool Execute( Player player, Slot slot )
	{
		return true;
	}
}
