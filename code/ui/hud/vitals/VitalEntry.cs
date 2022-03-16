// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class VitalEntry : Panel
{
	// @ref
	public Panel Icon { get; set; }
	// @ref
	public Panel Bar { get; set; }
	// @ref
	public Label Label { get; set; }

	public float BoundValue { get; set; }
	public float BoundMaxValue { get; set; } = 100f;

	public string FormattedValue { get; set; }

	public VitalEntry()
	{
		AddClass( "vital" );
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();
	}

	protected virtual string GetLabelText()
	{
		return $"{FormattedValue}";
	}

	public override void Tick()
	{
		base.Tick();

		Label.Text = GetLabelText();

		Bar.Style.Width = Length.Percent( BoundValue / BoundMaxValue * 100f );
	}
}
