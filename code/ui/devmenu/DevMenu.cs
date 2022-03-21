using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;

namespace Eden;

public class DevMenu : Panel
{
	public Panel ButtonLayout { get; set; }

	protected Button AddButton( DevCommandAttribute attribute )
	{
		var button = ButtonLayout.Add.ButtonWithIcon( attribute.Title, attribute.Icon, "button" );
		button.AddEventListener( "onclick", () =>
		{
			attribute.InvokeStatic( button );
		} );

		return button;
	}

	protected DropDown AddDropDown( DevCommandAttribute attribute )
	{
		var dropdown = ButtonLayout.AddChild<DropDown>( "dropdown" );
		dropdown.Text = attribute.Title;

		attribute.InvokeStatic( dropdown );

		return dropdown;
	}

	public DevMenu()
	{
		StyleSheet.Load( "/ui/devmenu/DevMenu.scss" );
		Add.Label( "Developer Menu", "title" );

		ButtonLayout = Add.Panel( "buttons" );

		var attributes = Library.GetAttributes<DevCommandAttribute>();
		foreach ( var attribute in attributes )
		{
			switch ( attribute.Type )
			{
				case CommandType.Button:
					AddButton( attribute );
					break;
				case CommandType.DropDown:
					AddDropDown( attribute );
					break;
				default:
					break;
			}
		}

		BindClass( "visible", () => Input.Down( InputButton.Flashlight ) );
	}

	public override void Tick()
	{
		base.Tick();

		if ( Input.Pressed( InputButton.Flashlight ) )
		{
			CreateEvent( "onopen" );
		}
	}
}
