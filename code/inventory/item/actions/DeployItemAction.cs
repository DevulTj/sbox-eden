// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using System;

namespace Eden;

public partial class DeployItemAction : ItemAction
{
	public override string ID => Deploy;
	public override string DisplayName => "Deploy";

	public override bool CanDo( Player player, Slot slot )
	{
		return true;
	}

	public override int Execute( Player player, Slot slot )
	{
		var itemAsset = slot.Item.Asset as DeployableItemAsset;
		if ( itemAsset is null )
			throw new Exception( "We somehow have the wrong item asset type here. Fix this." );

		var entity = Library.Create<Entity>( itemAsset.EntityClassName );
		if ( entity.IsValid() )
		{
			var tr = Trace.Ray( player.EyePosition, player.EyePosition + player.EyeRotation.Forward * 85 )
				.WorldAndEntities()
				.Ignore( player )
				.Radius( 4 )
				.Run();

			entity.Position = tr.EndPosition;

			return 1;
		}

		return 0;
	}
}
