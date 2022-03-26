// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;

namespace Eden;

public partial class HotbarContainer : Container
{
	public static HotbarContainer Current { get; set; }

	public HotbarContainer()
	{
		if ( Host.IsClient )
			Current = this;
	}

	public HotbarContainer( Player owner )
	{
		Owner = owner;
	}

	[Net]
	public int ActiveSlotIndex { get; set; } = -1;

	public Slot ActiveSlot => Items[ActiveSlotIndex];

	public void SetActiveSlot( int index )
	{
		if ( index >= Items.Count )
			return;

		// Nothing interesting here
		if ( index == -1 && ActiveSlotIndex == -1 )
			return;

		Slot slot = null;
		if ( index != -1 )
			slot = Items[index];

		if ( slot is null )
		{
			SetActiveItem( null );
			return;
		}

		// Allow to toggle by hitting the same slot
		if ( index == ActiveSlotIndex )
		{
			ActiveSlotIndex = -1;
			SetActiveItem( null );
		}
		else
		{
			ActiveSlotIndex = index;
			SetActiveItem( slot.Item );
		}
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

			SetActiveChild( weaponEntity );
		}
		else if ( item is not null && item.Type == ItemType.Deployable )
		{
			// Some generic deployable weapon here
		}
		else if ( item is not null )
		{
			var heldItemWeapon = new HeldItem();

			SetActiveChild( heldItemWeapon );
			heldItemWeapon.Item = item;
			heldItemWeapon.Quantity = Items[ActiveSlotIndex].Quantity;
			heldItemWeapon.HotbarSlotIndex = ActiveSlotIndex;
		}
		else
		{
			// Fall back to hands
			SetActiveChild( new Hands() );
		}
	}

	public int Add( Item item, bool makeActive = false, int quantity = 1 )
	{
		var slot = Add( item, quantity );

		if ( makeActive )
			SetActiveSlot( slot );

		return slot;
	}

	protected override void OnItemMoved( int slotA, int slotB, Container destination = null )
	{
		base.OnItemMoved( slotA, slotB, destination );

		if ( slotA == ActiveSlotIndex )
			SetActiveSlot( ActiveSlotIndex );
	}

	protected override void OnItemRemoved( int slotA )
	{
		base.OnItemRemoved( slotA );

		if ( slotA == ActiveSlotIndex )
			SetActiveSlot( ActiveSlotIndex );
	}

	protected override void OnItemAdded( Item item, int slot )
	{
		base.OnItemAdded( item, slot );

		if ( ActiveSlotIndex == slot )
			SetActiveSlot( ActiveSlotIndex );
	}
}
