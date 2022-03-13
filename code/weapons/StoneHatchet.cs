// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

[Library( "eden_hatchet_stone", Title = "Stone Hatchet", Spawnable = false )]
partial class StoneHatchet : Weapon
{
	public override string ViewModelPath => "models/tools/hatchet/hatchet_v.vmdl";
	public override float PrimaryRate => 1.0f;

	public override bool CanReload()
	{
		return false;
	}

	public override void Spawn()
	{
		base.Spawn();

		Model = Model.Load( "models/tools/hatchet/hatchet_w.vmdl" );
	}

	public override void CreateHudElements()
	{
		Crosshair.SetCrosshair( new HandsCrosshair() );
	}

	public override void AttackPrimary()
	{
		if ( MeleeAttack() )
			OnMeleeHit();
		else
			OnMeleeMiss();

		ViewModelEntity?.SetAnimParameter( "fire", true );
		( Owner as AnimEntity )?.SetAnimParameter( "b_attack", true );
	}

	protected override void OnPlayerUse()
	{
		ViewModelEntity?.SetAnimParameter( "grab", true );
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
		anim.SetAnimParameter( "holdtype_attack", 1 );
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

	private bool MeleeAttack()
	{
		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		bool hit = false;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * 80, 20.0f ) )
		{
			if ( !tr.Entity.IsValid() ) continue;

			tr.Surface.DoBulletImpact( tr );

			hit = true;

			if ( !IsServer ) continue;

			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * 100, 25 )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}

		return hit;
	}

	[ClientRpc]
	private void OnMeleeMiss()
	{
		Host.AssertClient();

		if ( IsLocalPawn )
		{
			_ = new Sandbox.ScreenShake.Perlin();
		}
	}

	[ClientRpc]
	private void OnMeleeHit()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimParameter( "hit", true );

		if ( IsLocalPawn )
		{
			_ = new Sandbox.ScreenShake.Perlin( 1.0f, 1.0f, 3.0f );
		}
	}
}
