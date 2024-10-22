﻿// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using System.Collections.Generic;

namespace Eden;

[UseTemplate]
public partial class MainMenuPanel : Panel
{
	public static MainMenuPanel Instance { get; protected set; }

	public bool IsOpen { get; protected set; } = false;

	public List<MainMenuPage> Pages { get; set; } = new();
	public List<Panel> SectionDots { get; set; } = new();

	public MainMenuPage CurrentPage { get; set; }
	public int CurrentPageIndex { get; set; } = 0;


	// @ref
	public Panel PageLayout { get; set; }
	// @ref
	public Panel SectionDotsPanel { get; set; }
	// @ref
	public Label CurrentPageLabel { get; set; }
	// @ref
	public Label PreviousPageLabel { get; set; }
	// @ref
	public Label NextPageLabel { get; set; }

	public MainMenuPanel()
	{
		Instance = this;
	}

	public void SetOpen( bool state = true )
	{
		IsOpen = state;
	}

	protected void Setup()
	{
		SectionDotsPanel.DeleteChildren( true );
		PageLayout.DeleteChildren( true );

		Pages = new();
		SectionDots = new();
		CurrentPage = null;

		BindClass( "open", () => IsOpen );

		// 0
		AddPage( new SkillsPage() );
		// 1
		AddPage( new BackpackPage() );
		// 2
		AddPage( new CraftingPage() );
		// @TODO: better way to handle the default page?

		SetPageByName( "inventory" );
	}

	public int GetNextPageIndex()
	{
		var index = CurrentPageIndex + 1;
		if ( index > Pages.Count - 1 )
			index = 0;

		return index;
	}

	public int GetPreviousPageIndex()
	{
		var index = CurrentPageIndex - 1;
		if ( index < 0 )
			index = Pages.Count - 1;

		return index;
	}

	public MainMenuPage GetNextPage() => Pages[GetNextPageIndex()];
	public MainMenuPage GetPreviousPage() => Pages[GetPreviousPageIndex()];

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();

		Setup();
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
			Event.Run( IsOpen ? GameEvent.Client.MainMenuOpened : GameEvent.Client.MainMenuClosed );
		}

		if ( !IsOpen ) return;

		if ( input.Pressed( InputButton.Menu ) )
		{
			SetPageIndex( GetPreviousPageIndex() );
			input.ClearButton( InputButton.Menu );
		}

		if ( input.Pressed( InputButton.Use ) )
		{
			SetPageIndex( GetNextPageIndex() );
			input.ClearButton( InputButton.Use );
		}
	}

	public void SetPageByName( string identifier )
	{
		for ( int i = 0; i < Pages.Count; i++ )
		{
			var page = Pages[i];
			if ( page.PageName.ToLower() == identifier.ToLower() )
			{
				SetPageIndex( i );
				break;
			}
		}
	}

	public void SetPageIndex( int index )
	{
		var previousIndex = CurrentPageIndex;

		var page = Pages[index];
		CurrentPage = page;
		CurrentPageIndex = index;

		CurrentPageLabel.Text = page.PageName;
		PreviousPageLabel.Text = GetPreviousPage().PageName;
		NextPageLabel.Text = GetNextPage().PageName;

		SectionDots[previousIndex].SetClass( "activesection", false );
		SectionDots[index].SetClass( "activesection", true );
	}

	public void AddPage( MainMenuPage page )
	{
		page.Parent = PageLayout;

		Pages.Add( page );

		page.BindClass( "show", () => CurrentPage == page );

		var panel = SectionDotsPanel.AddChild<Panel>( "section" );
		SectionDots.Add( panel );
	}
}
