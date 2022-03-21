// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public enum CommandType
{
	Button,
	DropDown
}

public class DevCommandAttribute : LibraryMethod
{
	public string Category { get; set; } = "General";
	public CommandType Type { get; set; } = CommandType.Button;
}
