// without permission of its author (gustaf@thrivingventures.com).

using Sandbox;

namespace Eden;

public partial class InventoryHelpers
{
	public static void PickupItem( Player player, WorldItemEntity worldItem )
	{
		if ( GiveItem( player, worldItem.Item, worldItem.Quantity ) )
			worldItem.Delete();
	}

	public static bool GiveItem( Player player, Item item, int quantity = 1 )
	{
		var container = player.Backpack;
		// If the item is a weapon, try to prioritize the hotbar.
		if ( item.Type == ItemType.Weapon )
		{
			var hotbar = player.Hotbar;
			var emptySlot = hotbar.FindEmptySlot();
			// If we have space in our hotbar
			if ( emptySlot != -1 )
				container = hotbar;
		}

		var desiredSlot = container.Add( item, quantity );

		if ( desiredSlot != -1 )
		{
			ResourceNotifications.AddResource( To.Single( player.Client ), quantity, item.Asset.ItemName );
			ContainerNetwork.UpdatePlayer( To.Single( player.Client ), container.ID.ToString() );
		}

		return desiredSlot != -1;
	}

}
