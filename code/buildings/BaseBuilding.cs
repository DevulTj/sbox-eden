// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public class BaseBuilding
{
	public virtual void PlaceBuilding()
	{

	}

	public virtual Vector3 FindSnapPoint()
	{
		return Vector3.Zero;
	}

	public virtual bool IsPlacementValid()
	{
		return true;
	}

	public virtual bool CanAfford( Player player )
	{
		return true;
	}
}
