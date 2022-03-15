// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System.Collections.Generic;
using System.Linq;

namespace Eden;

public partial class Player
{
	[Net, Local]
	public CraftingQueue CraftingQueue { get; set; }
}
