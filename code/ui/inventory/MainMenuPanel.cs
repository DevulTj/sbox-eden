// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;

namespace Eden;

[UseTemplate]
public partial class MainMenuPanel : Panel
{
	public bool IsOpen { get; protected set; } = false;

	// @ref
	public Panel PageLayout { get; set; }

	public MainMenuPanel()
	{
		BindClass( "open", () => IsOpen );
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();
	}

	public override void Tick()
	{
		base.Tick();
	}

	[Event.BuildInput]
	public void BuildInput( InputBuilder input )
	{
		if ( input.Pressed( InputButton.Score ) )
		{
			IsOpen = !IsOpen;
		}
	}

	public void SetActivePage( MainMenuPage page )
	{
		//
	}
}
