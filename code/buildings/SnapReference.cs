// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public struct SnapReference
{
	public BuildingEntity Entity { get; set; }
	public int SnapIndex { get; set; }

	public SnapPoint SnapPoint => Entity.SnapPoints[SnapIndex];

	public SnapReference( BuildingEntity entity, int snapIndex )
	{
		this.Entity = entity;
		this.SnapIndex = snapIndex;
	}
}
