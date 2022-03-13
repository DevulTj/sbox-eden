// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class DraggedItemPanel : ItemPanel
{
	public override void Tick()
	{
		base.Tick();

		var DMousePos = Mouse.Position / Screen.Size;
		var left = DMousePos.x;
		var top = DMousePos.y;

		Style.Left = Length.Fraction( left );
		Style.Top = Length.Fraction( top );

		var transform = new PanelTransform();
		transform.AddTranslateY( Length.Fraction( -0.5f ) );
		transform.AddTranslateX( Length.Fraction( -0.5f ) );

		Style.Transform = transform;
	}
}
