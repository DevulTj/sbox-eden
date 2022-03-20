// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public abstract class ItemActionBaseAttribute : LibraryMethod
{
	public ItemActionType Type { get; set; } = ItemActionType.Invalid;
}

public class ItemActionExecAttribute : ItemActionBaseAttribute
{
	public ItemActionExecAttribute( ItemActionType type, string displayName )
	{
		Type = type;
		Title = displayName;
	}
}

public class ItemActionCheckAttribute : ItemActionBaseAttribute
{
	public ItemActionCheckAttribute( ItemActionType type )
	{
		Type = type;
	}
}
