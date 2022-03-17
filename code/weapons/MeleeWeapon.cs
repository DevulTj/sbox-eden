// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

partial class MeleeWeapon : Weapon
{
	public virtual float PrimaryAttackRange => 160f;
	public virtual float AttackRadius => 20f;
	public virtual float BaseDamage => 50f;

	public virtual Dictionary<ResourceType, int> ResourceYield => new()
	{
		{ ResourceType.Wood, 0 },
		{ ResourceType.Stone, 0 }
	};

	public virtual int GetResourceYield( ResourceType type ) => ResourceYield[type];

	public override bool CanReload()
	{
		return false;
	}

	public override void CreateHudElements()
	{
		Crosshair.SetCrosshair( new HandsCrosshair() );
	}

	protected virtual void DoAttackAnimation()
	{
		ViewModelEntity?.SetAnimParameter( "fire", true );
		( Owner as AnimEntity )?.SetAnimParameter( "b_attack", true );
	}

	public override void AttackPrimary()
	{
		if ( MeleeAttack() )
			RpcOnMeleeHit();
		else
			RpcOnMeleeMiss();

		DoAttackAnimation();
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
	}

	protected virtual float CalculateDamage()
	{
		return BaseDamage;
	}

	protected virtual bool MeleeAttack()
	{
		var forward = Owner.EyeRotation.Forward;
		forward = forward.Normal;

		var hit = false;

		foreach ( var tr in TraceBullet( Owner.EyePosition, Owner.EyePosition + forward * PrimaryAttackRange, AttackRadius ) )
		{
			if ( !tr.Entity.IsValid() ) continue;

			tr.Surface.DoBulletImpact( tr );

			hit = true;

			if ( !IsServer ) continue;

			using ( Prediction.Off() )
			{
				var damageInfo = DamageInfo.FromBullet( tr.EndPosition, forward * PrimaryAttackRange, CalculateDamage() )
					.UsingTraceResult( tr )
					.WithAttacker( Owner )
					.WithWeapon( this );

				tr.Entity.TakeDamage( damageInfo );
			}
		}

		return hit;
	}

	[ClientRpc]
	private void RpcOnMeleeMiss()
	{
		OnMeleeMiss();
	}

	protected virtual void OnMeleeMiss()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimParameter( "hit", true );

		if ( IsLocalPawn )
			_ = new Sandbox.ScreenShake.Perlin();
	}

	[ClientRpc]
	private void RpcOnMeleeHit()
	{
		OnMeleeHit();
	}

	protected virtual void OnMeleeHit()
	{
		Host.AssertClient();

		ViewModelEntity?.SetAnimParameter( "hit", true );

		if ( IsLocalPawn )
			_ = new Sandbox.ScreenShake.Perlin( 1.0f, 1.0f, 3.0f );
	}
}
