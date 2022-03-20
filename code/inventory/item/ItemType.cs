// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using System;

namespace Eden;

public enum ItemType
{
	Item,
	Consumable,
	Deployable,
	Weapon
}

static class ItemTypeExtensions
{
	public static Item Create( this ItemType itemType )
	{
		return itemType switch
		{
			ItemType.Item => new Item(),
			ItemType.Consumable => new ConsumableItem(),
			ItemType.Deployable => new DeployableItem(),
			ItemType.Weapon => new WeaponItem(),
			_ => throw new Exception( "Invalid item type specified in ItemType.Create" ),
		};
	}
}
