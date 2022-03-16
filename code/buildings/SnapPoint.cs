// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class SnapPoint : BaseNetworkable
{
	[Net] public Transform Transform { get; set; }
	[Net] public BuildingEntity AttachedEntity { get; set; }

	public SnapPoint() { }

	public SnapPoint( Transform transform )
	{
		Transform = transform;
	}
}
