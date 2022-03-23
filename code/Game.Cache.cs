// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;
using System.Linq;

namespace Eden;

public partial class Game
{
	protected void CacheItemAsset( ItemAsset asset )
	{
		if ( asset.Recipe?.Items?.Count > 0 )
			asset.Recipe.Cache();
	}

	public void CacheAssets()
	{
		ItemAsset.All.ToList().ForEach( x => CacheItemAsset( x ) );
	}
}
