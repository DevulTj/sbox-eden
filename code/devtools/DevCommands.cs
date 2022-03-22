using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace Eden;

public partial class DevCommands
{
	[DevCommand( Title = "Commit Suicide", Category = "Misc", Icon = "repeat" )]
	public static void Kill( Button button )
	{
		Game.KillCommand();
	}

	[DevCommand( Title = "Select Item", Category = "Inventory", Type = CommandType.DropDown )]
	public static void SpawnItem( DropDown dropdown )
	{
		foreach ( var itemAsset in ItemAsset.All )
		{
			dropdown.Options.Add( new Option( itemAsset.ItemName, itemAsset.Name ) );
		}

		var spawnButton = dropdown.Parent.Add.ButtonWithIcon( "Spawn Item", "backpack", "button", () =>
		{
			ContainerNetwork.SpawnItem( dropdown.Value, 1 );
		} );
	}
}
