using Sandbox.UI;
using Sandbox.UI.Construct;

namespace Eden;

public partial class WipText : Panel
{
	public WipText()
	{
		StyleSheet.Load( "/ui/hud/WipText.scss" );
		Add.Label( "EDEN ALPHA - EVERYTHING IS SUBJECT TO CHANGE", "subtitle wip" );
		Add.Label( "https://apetavern.com", "website" );
		Add.Image( "ui/misc/logo.png", "logo" );
	}
}
