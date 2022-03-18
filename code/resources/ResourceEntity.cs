// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

[Library( "eden_resource_single", Spawnable = true )]
public partial class ResourceEntity : ModelEntity
{
	public TimeSince LastRefresh { get; set; }
}
