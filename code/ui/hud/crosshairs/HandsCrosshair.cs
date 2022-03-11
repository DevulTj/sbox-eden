
using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Threading.Tasks;

namespace Eden;

public partial class HandsCrosshair : Panel
{
	public HandsCrosshair()
	{
		StyleSheet.Load( "/ui/hud/crosshairs/HandsCrosshair.scss" );
	}

	float scale = 1;

	public override void Tick()
	{
		base.Tick();

		Style.Width = 32 * scale;
		Style.Height = 20 * scale;
		Style.Dirty();

		scale = scale.LerpTo( 1, Time.Delta * 5 );

	}

	[PanelEvent]
	public void OnAttackEvent()
	{
		scale = 10;
	}
}
