// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class ItemActionExecAttribute : LibraryMethod
{
	public ItemActionType Type { get; set; } = ItemActionType.Invalid;

	public ItemActionExecAttribute( ItemActionType type )
	{
		Type = type;
	}
}

public partial class ItemActionCheckAttribute : LibraryMethod
{
	public ItemActionType Type { get; set; } = ItemActionType.Invalid;

	public ItemActionCheckAttribute( ItemActionType type )
	{
		Type = type;
	}
}
