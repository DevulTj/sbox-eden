// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

namespace Eden;

public enum ResourceType
{
	Wood,
	Stone
}

static class ResourceTypeExtensions
{
	public static string GetName( this ResourceType type )
	{
		return type switch
		{
			ResourceType.Wood => "Wood",
			ResourceType.Stone => "Stone",
			_ => "N/A"
		};
	}
}
