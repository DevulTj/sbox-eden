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
	protected int TimesGathered { get; set; }
	private int CurrentModelIndex { get; set; }

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

		UpdateModel( ResourceAsset.WorldModels[0] is not null ? ResourceAsset.WorldModels[0] : ResourceAsset.FallbackWorldModel );
		CurrentModelIndex = 0;

		ResourceAsset.ItemsToGather.ForEach( x => AvailableItems.Add( new( x ) ) );

		return this;
	}

	protected void UpdateModel( Model model )
	{
		Model = model;
	}

	protected void Gather( Player player, MeleeWeapon weapon )
	{
		if ( !player.IsValid() )
			return;

		if ( !weapon.IsValid() )
			return;

		var weaponResourceYield = weapon.GetResourceYield( ResourceAsset.ResourceType );

		var basePenalty = ResourceAsset.BaseDurabilityPenalty;
		var badYieldPenalty = 1 - weaponResourceYield;
		var collectableMultiplier = ResourceAsset.IsCollectable ? 20 : 10;

		weapon.UpdateDurability( basePenalty - (int)( badYieldPenalty * collectableMultiplier ) );

		if ( ResourceAsset.IsCollectable )
			return;

		var gatherableResource = AvailableItems.FirstOrDefault();
		var quantityToTake = MathX.CeilToInt( gatherableResource.InitialAmount / ( ResourceAsset.RequiredHitsPerItem < 1 ? 1 : ResourceAsset.RequiredHitsPerItem ) * weaponResourceYield );

		gatherableResource.AmountRemaining -= quantityToTake;

		OnGather( player, gatherableResource.ItemAssetName, quantityToTake );

		TimesGathered++;

		if ( gatherableResource.AmountRemaining <= 0 )
			AvailableItems.Remove( gatherableResource );

		if ( !AvailableItems.Any() )
			Destroy();
	}

	protected virtual void OnGather( Player player, string itemAssetName, int quantity )
	{
		if ( quantity < 1 )
			return;

		GiveItem( player, itemAssetName, quantity );

		if ( !ResourceAsset.ResourceHasMultipleModels )
			return;

		var totalRequiredHits = ResourceAsset.RequiredHitsPerItem * ResourceAsset.ItemsToGather.Count;
		var modelCount = ResourceAsset.WorldModelPath.Length;

		var hitsPerChange = totalRequiredHits / modelCount;

		if ( TimesGathered % hitsPerChange != 0 || TimesGathered < 1 )
			return;

		CurrentModelIndex++;

		if ( CurrentModelIndex >= modelCount )
			return;

		UpdateModel( ResourceAsset.WorldModels[CurrentModelIndex] );

		PlayModelChangeEffects();
	}

	[ClientRpc]
	protected void PlayModelChangeEffects()
	{
		Log.Info( $"PlayModelChangeEffects {ResourceAsset}" );

		if ( !string.IsNullOrEmpty( ResourceAsset.ModelChangeSound ) )
			PlaySound( ResourceAsset.ModelChangeSound );

		Log.Info( $"Trying to play sound {ResourceAsset.ModelChangeSound}" );

		if ( !string.IsNullOrEmpty( ResourceAsset.ModelChangeParticle ) )
			Particles.Create( ResourceAsset.ModelChangeParticle, this );

		Log.Info( $"Trying to use particle {ResourceAsset.ModelChangeParticle}" );

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
		return ResourceAsset.IsCollectable;
	}
}
