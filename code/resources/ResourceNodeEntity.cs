// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

[Library( "eden_resource", Spawnable = true )]
[Hammer.EntityTool( "Resource", "Eden", "A spawn for resources." )]
public partial class ResourceNodeEntity : Prop, IUse
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

        MoveType = MoveType.None;
    }

    public ResourceNodeEntity SetResourceAs( ResourceAsset resourceAsset )
    {
        ResourceAsset = resourceAsset;

        if ( ResourceAsset is null )
            Destroy();

        Model = ResourceAsset.WorldModel is not null ? ResourceAsset.WorldModel : ResourceAsset.FallbackWorldModel;

        ResourceAsset.ItemsToGather.ForEach( x => AvailableItems.Add( new( x ) ) );

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

        if ( ResourceAsset.Collectable )
            return;

        var gatherableResource = AvailableItems.FirstOrDefault();
        var quantityToTake = gatherableResource.InitialAmount / ( ResourceAsset.RequiredHitsPerItem < 1 ? 1 : ResourceAsset.RequiredHitsPerItem ) * weaponResourceYield;

        gatherableResource.AmountRemaining -= quantityToTake;

        GiveItem( player, gatherableResource.ItemAssetName, quantityToTake );

        if ( gatherableResource.AmountRemaining <= 0 )
            AvailableItems.Remove( gatherableResource );

        if ( !AvailableItems.Any() )
            Destroy();
    }

    private void GiveItem( Player player, string itemAssetName, int quantity )
    {
        var item = Item.FromAsset( itemAssetName );

        if ( !InventoryHelpers.GiveItem( player, item, quantity ) )
        {
            ResourceNotifications.AddResource( To.Single( player.Client ), quantity, item.Asset.ItemName );

            var entity = ItemEntity.Create( item, quantity );
            entity.Position = Position + Vector3.Up * 10f;
        }
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

    protected virtual void Destroy()
    {
        // @todo: stuff
        Delete();
    }

    public bool OnUse( Entity user )
    {
        foreach ( var item in AvailableItems )
            GiveItem( user as Player, item.ItemAssetName, item.AmountRemaining );

        Destroy();

        return false;
    }

    public bool IsUsable( Entity user )
    {
        return ResourceAsset.Collectable;
    }
}
