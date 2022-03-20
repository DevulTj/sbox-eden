// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class DeployableItem : Item
{
	public override ItemType Type => ItemType.Deployable;

	protected override void GatherActions( ref List<ItemAction> actions )
	{
		actions.Add( new DeployItemAction() );

		base.GatherActions( ref actions );
	}
}
