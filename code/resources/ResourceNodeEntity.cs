// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

[Library( "eden_resource", Spawnable = true )]
[Hammer.EntityTool( "Resource", "Eden", "A spawn for resources." )]
public partial class ResourceNodeEntity : Prop
{
	[Property]
	public ResourceType ResourceType { get; set; }

	[Net]
	public ResourceAsset ResourceAsset { get; set; }
	public List<ResourceItemQuantity> AvailableItems { get; set; } = new();
	public event Action<ResourceNodeEntity> OnDestroyed;
	public TimeSince LastRefresh { get; set; }

	public override void Spawn()
	{
		base.Spawn();

		Model = ResourceAsset.FallbackWorldModel;
		MoveType = MoveType.None;
	}

	public ResourceNodeEntity SetResourceAs( ResourceType type )
	{
		ResourceAsset = ResourceAsset.All.FirstOrDefault( x => x.ResourceType == type );

		if ( ResourceAsset is null )
			Delete();

		AvailableItems.AddRange( ResourceAsset.ItemsToGather );

		return this;
	}

	protected void Gather( Player player, MeleeWeapon weapon )
	{
		if ( !player.IsValid() )
			return;

		if ( !weapon.IsValid() )
			return;

		var weaponResourceYield = weapon.GetResourceYield( ResourceAsset.ResourceType );

		if ( weaponResourceYield < 1 )
		{
			// TODO: Deal heavy damage to weapon
			return;
		}

		var gatherableResource = AvailableItems.FirstOrDefault();
		var quantityToTake = gatherableResource.InitialAmount / 5 * weaponResourceYield;

		gatherableResource.AmountRemaining -= quantityToTake;

		var item = Item.FromAsset( gatherableResource.ItemAssetName );

		if ( !InventoryHelpers.GiveItem( player, item, quantityToTake ) )
		{
			ResourceNotifications.AddResource( To.Single( player.Client ), quantityToTake, item.Asset.ItemName );

			var entity = ItemEntity.Instantiate( item, quantityToTake );
			entity.Position = Position + Vector3.Up * 10f;
		}

		if ( gatherableResource.AmountRemaining <= 0 )
			AvailableItems.Remove( gatherableResource );

		if ( !AvailableItems.Any() )
			Explode();
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		Gather( info.Attacker as Player, info.Weapon as MeleeWeapon );
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();

		OnDestroyed?.Invoke( this );
	}

	protected virtual void Explode()
	{
		// @todo: stuff
		Delete();
	}
}
