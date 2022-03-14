// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

[Library( "eden_hatchet_stone", Title = "Stone Hatchet", Spawnable = false )]
partial class StoneHatchet : MeleeWeapon
{
	public override string ViewModelPath => "models/tools/hatchet/hatchet_v.vmdl";
	public override float PrimaryRate => 1.0f;

	public override Dictionary<ResourceType, int> ResourceYield => new()
	{
		{ ResourceType.Wood, 80 },
		{ ResourceType.Stone, 0 }
	};

	public override void Spawn()
	{
		base.Spawn();

		Model = Model.Load( "models/tools/hatchet/hatchet_w.vmdl" );
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
}
