// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class DeployableItem : Item
{
	public override ItemType Type => ItemType.Deployable;

	[ItemActionCheck( ItemActionType.Deploy )]
	public bool CanDeploy( Player player )
	{
		return true;
	}

	[ItemActionExec( ItemActionType.Deploy )]
	public bool Deploy( Player player )
	{
		return true;
	}
}
