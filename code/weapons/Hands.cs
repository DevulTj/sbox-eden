// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

[Library( "eden_hands", Title = "Hands", Spawnable = false )]
partial class Hands : MeleeWeapon
{
	public override string ViewModelPath => "models/arms/toon_arms.vmdl";
	public override float PrimaryRate => 2.0f;
	public override float SecondaryRate => 2.0f;
	public override float PrimaryAttackRange => 60f;
	public override float BaseDamage => 10f;

	private void Attack( bool leftHand )
	{
		if ( MeleeAttack() )
			RpcOnMeleeHit( leftHand );
		else
			RpcOnMeleeMiss( leftHand );

		( Owner as AnimEntity )?.SetAnimParameter( "b_attack", true );
	}

	public override void AttackPrimary()
	{
		Attack( true );
	}

	public override void AttackSecondary()
	{
		Attack( false );
	}

	public override bool CanPrimaryAttack()
	{
		return base.CanPrimaryAttack() && TimeSinceSecondaryAttack > ( 1 / SecondaryRate );
	}

	public override bool CanSecondaryAttack()
	{
		return base.CanSecondaryAttack() && TimeSincePrimaryAttack > ( 1 / PrimaryRate );
	}

	public override void SimulateAnimator( PawnAnimator anim )
	{
		anim.SetAnimParameter( "holdtype", 5 );
		anim.SetAnimParameter( "aim_body_weight", 1.0f );

		ViewModelEntity?.SetAnimParameter( "cangrab", ( Owner as Player ).WantToGrab );
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

	[ClientRpc]
	protected void RpcOnMeleeMiss( bool leftHand )
	{
		OnMeleeMiss();

		var attackAnimName = leftHand ? "attack_l" : "attack_r";
		ViewModelEntity?.SetAnimParameter( attackAnimName, true );
	}

	[ClientRpc]
	protected void RpcOnMeleeHit( bool leftHand )
	{
		OnMeleeHit();

		var attackAnimName = leftHand ? "attack_l" : "attack_r";
		ViewModelEntity?.SetAnimParameter( attackAnimName, true );
	}
}
