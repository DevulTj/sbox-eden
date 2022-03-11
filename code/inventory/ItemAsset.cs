// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

[Library( "item" ), AutoGenerate]
public partial class ItemAsset : Asset
{
	[Property]
	public string ItemName { get; set; }

	[Property]
	public string ItemDescription { get; set; }

	[Property, ResourceType( "png" )]
	public string IconPath { get; set; }
}
