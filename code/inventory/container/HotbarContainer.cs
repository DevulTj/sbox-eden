// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class HotbarContainer : Container
{
	public HotbarContainer()
	{
	}

	public HotbarContainer( Player owner )
	{
		Owner = owner;
	}

	[Net]
	public Player Owner { get; set; }

	[Net]
	public Slot ActiveSlot { get; set; }

	public void SetActiveSlot( int index )
	{
		if ( index >= Items.Count ) return;

		var slot = Items[index];
		SetActiveSlot( slot );
	}

	public void SetActiveSlot( Slot slot )
	{
		if ( slot is null )
		{
			SetActiveItem( null );
			return;
		}

		// Allow to toggle by hitting the same slot
		if ( slot == ActiveSlot )
			ActiveSlot = null;
		else
			ActiveSlot = slot;

		SetActiveItem( slot.Item );
	}

	public void SetActiveChild( BaseCarriable carriable )
	{
		( Owner.ActiveChild as BaseCarriable )?.OnCarryDrop( Owner );
		Owner.ActiveChild?.Delete();
		//
		Owner.ActiveChild = carriable;
		carriable?.OnCarryStart( Owner );
	}

	public void SetActiveItem( Item item )
	{
		if ( Host.IsClient ) return;

		if ( item is not null && item.Type == ItemType.Weapon )
		{
			var weaponItemAsset = item.Asset as WeaponItemAsset;
			var weaponEntity = Library.Create<Weapon>( weaponItemAsset.WeaponClassName );

			Owner.ActiveChild = weaponEntity;
			( Owner.ActiveChild as BaseCarriable )?.OnCarryStart( Owner );
		}
		else if ( item is not null && item.Type == ItemType.Deployable )
		{
			// Some generic deployable weapon here
		}
		else
		{
			// Fall back to hands
			SetActiveChild( new Hands() );
			( Owner.ActiveChild as BaseCarriable )?.OnCarryStart( Owner );
		}
	}
}
