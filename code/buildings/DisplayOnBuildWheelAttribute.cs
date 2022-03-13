// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public class DisplayOnBuildWheelAttribute : LibraryAttribute
{
	public DisplayOnBuildWheelAttribute( string title, string icon )
	{
		this.Title = title;
		this.Icon = icon;
	}
}
