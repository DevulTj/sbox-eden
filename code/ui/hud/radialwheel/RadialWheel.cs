// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using System;
using System.Collections.Generic;

namespace Eden;

[UseTemplate]
public partial class RadialWheel : Panel
{
	//
	// @text
	//
	public string CurrentName { get; set; }
	public string CurrentDescription { get; set; }

	//
	// @ref
	//
	public Panel Wrapper { get; set; }
	public Image CurrentImage { get; set; }
	public Panel Inner { get; set; }

	//
	// Runtime
	//
	private PieSelector selector;
	public int SelectedIndex { get; set; }

	private List<Item> Items { get; } = new();

	private RadialWheel()
	{
		SetTemplate( "ui/hud/radialwheel/RadialWheel.html" );
		AddClass( "pie-menu" );
	}

	public static RadialWheel Create()
	{
		Host.AssertClient();

		var radialWheel = new RadialWheel();
		radialWheel.Parent = Local.Hud;

		return radialWheel;
	}

	public void AddOption( string text, string icon, Action onSelected )
	{
		Items.Add( new Item( text, icon, onSelected ) );
		BuildIcons();
	}

	protected override void PostTemplateApplied()
	{
		base.PostTemplateApplied();
		BuildIcons();

		//
		// Create pie selector here so that it regenerates
		// when there's changes etc.
		//
		selector?.Delete();
		selector = new PieSelector( this );
		selector.Parent = Wrapper;
	}

	public float AngleIncrement => 360f / Items.Count;
	private List<Panel> icons = new();

	/// <summary>
	/// Puts icons on the wheel so the player knows what they're selecting
	/// </summary>
	public void BuildIcons()
	{
		Inner.DeleteChildren();

		int index = 0;
		foreach ( var item in Items )
		{
			Vector2 frac = MathExtension.InverseAtan2( AngleIncrement * index );

			frac = ( 1.0f + frac ) / 2.0f; // Normalize from -1,1 to 0,1

			var panel = Inner.Add.Image( item.Icon, "item-icon" );

			panel.Style.Left = Length.Fraction( frac.x );
			panel.Style.Top = Length.Fraction( frac.y );

			icons.Add( panel );

			index++;
		}
	}

	/// <summary>
	/// Get the current angle based on the mouse position, relative to the center of the menu.
	/// Returns <see langword="null"/> if we're not really looking at anything
	/// </summary>
	private float GetCurrentAngle()
	{
		Vector2 relativeMousePos = Mouse.Position - Wrapper.Box.Rect.Center;

		float ang = MathF.Atan2( relativeMousePos.y, relativeMousePos.x )
			.RadianToDegree();

		ang = ang.SnapToGrid( AngleIncrement );

		return ang;
	}

	protected int GetCurrentIndex()
	{
		var ang = GetCurrentAngle();
		return ( ang.UnsignedMod( 360.0f ) / AngleIncrement ).FloorToInt();
	}

	/// <summary>
	/// Get the current <see cref="Item"/> based on the value returned from <see cref="GetCurrentAngle"/>
	/// </summary>
	protected Item? GetCurrentItem()
	{
		int selectedIndex = GetCurrentIndex();

		if ( selectedIndex < 0 || selectedIndex > Items.Count )
			return null;

		var selectedItem = Items[selectedIndex];
		return selectedItem;
	}

	public override void Tick()
	{
		base.Tick();
		UpdateWheel();
	}

	private void UpdateWheel()
	{
		var newSelectedItem = GetCurrentItem();
		int newSelectedIndex = GetCurrentIndex();

		for ( int i = 0; i < icons.Count; i++ )
		{
			icons[i].SetClass( "active", i == newSelectedIndex );
		}

		if ( newSelectedIndex != SelectedIndex )
		{
			SelectedIndex = newSelectedIndex;

			CurrentImage.SetTexture( newSelectedItem?.Icon ?? "" );
			CurrentName = newSelectedItem?.Text ?? "None";

			OnChange();
		}
	}

	protected virtual void OnChange() { }
}
