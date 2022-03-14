// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Eden;

[Library( "eden_held_item", Title = "Held Item", Spawnable = false )]
partial class HeldItem : Weapon
{
	// 
	public Item Item { get; set; }

	[Net]
	public int Quantity { get; set; } = 1;

	public int HotbarSlotIndex { get; set; } = -1;

	public override float PrimaryRate => 1.0f;

	public override bool CanReload()
	{
		return false;
	}

	public override void Spawn()
	{
		base.Spawn();

		Model = ItemAsset.FallbackWorldModel;
	}

	public override void CreateHudElements()
	{
		Crosshair.SetCrosshair( new HandsCrosshair() );
	}

	// @TODO: hack central, find a better way for this
	public async Task DelayedDelete( Player player, int slotIndex )
	{
		await GameTask.DelaySeconds( 0.2f );

		player.Hotbar.Remove( slotIndex );
		ContainerNetwork.UpdatePlayer( To.Single( player.Client ), player.Hotbar.ID.ToString() );
	}

	protected void Throw()
	{
		ViewModelEntity?.SetAnimParameter( "fire", true );
		( Owner as AnimEntity )?.SetAnimParameter( "b_attack", true );

		if ( IsServer )
		{
			var entity = WorldItemEntity.Instantiate( Item, Quantity );
			entity.Position = Position + Owner.EyeRotation.Forward * 10f;
			entity.ApplyAbsoluteImpulse( Owner.EyeRotation.Forward * 1000f + Vector3.Up * 100f );

			_ = DelayedDelete( Owner as Player, HotbarSlotIndex );


			// @TODO: rethink, this is shit

		}
	}

	public override void AttackPrimary()
	{
		Throw();
	}

	protected override void OnPlayerUse()
	{
		// ViewModelEntity?.SetAnimParameter( "grab", true );
	}

	public override void OnCarryStart( Entity carrier )
	{
		base.OnCarryStart( carrier );

		SetParent( carrier, null );
	}

	public override void OnCarryDrop( Entity dropper )
	{
		//
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", HoldType.Item.ToInt() );
		anim.SetAnimParameter( "holdtype_handedness", HoldHandedness.RightHand.ToInt() );
		anim.SetAnimParameter( "holdtype_pose_hand", 0.07f );
		anim.SetAnimParameter( "holdtype_attack", 1f );
	}

	public override void CreateViewModel()
	{
		ViewModelData = new()
		{
			SwingInfluence = 0.03f,
			Offset = new Vector3( 1f, -1f, -4f ),
		};

		base.CreateViewModel();
	}

	public override void Simulate( Client owner )
	{
		base.Simulate( owner );

		var player = Owner as Player;
		if ( player.IsValid() )
		{
			var boneId = player.GetBoneIndex( "hand_R" );
			var bone = player.GetBoneTransform( boneId, true );

			Position = bone.Position;
		}
	}
}
