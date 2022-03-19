// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
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

	public override void Spawn()
	{
		base.Spawn();

		Model = ResourceAsset.FallbackWorldModel;

		// Shitty hack, couldn't get specifying the asset directly through hammer to work properly. 
		ResourceAsset = ResourceAsset.All.FirstOrDefault( x => x.ResourceType == ResourceType );

		if ( ResourceAsset is null )
			Delete();

		AvailableItems.AddRange( ResourceAsset.ItemsToGather );

		MoveType = MoveType.None;
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
		var assetEquivalent = ResourceAsset.ItemsToGather.FirstOrDefault( x => x.ItemAssetName == gatherableResource.ItemAssetName );

		var quantityToTake = assetEquivalent.TotalQuantity / 5 * weaponResourceYield;
		gatherableResource.TotalQuantity -= quantityToTake;

		var item = Item.FromAsset( assetEquivalent.ItemAssetName );

		if ( !InventoryHelpers.GiveItem( player, item, quantityToTake ) )
		{
			ResourceNotifications.AddResource( To.Single( player.Client ), quantityToTake, item.Asset.ItemName );

			var entity = ItemEntity.Instantiate( item, quantityToTake );
			entity.Position = Position + Vector3.Up * 10f;
		}

		if ( gatherableResource.TotalQuantity <= 0 )
			AvailableItems.Remove( gatherableResource );

		if ( !AvailableItems.Any() )
			Explode();
	}

	public override void TakeDamage( DamageInfo info )
	{
		base.TakeDamage( info );

		Gather( info.Attacker as Player, info.Weapon as MeleeWeapon );
	}

	protected virtual void Explode()
	{
		// @todo: stuff
		Delete();
	}
}
