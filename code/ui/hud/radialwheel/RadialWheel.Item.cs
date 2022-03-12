// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using System;

namespace Eden;

public partial class RadialWheel
{
	public struct Item
	{
		public string Icon { get; set; }
		public string Text { get; set; }
		public Action OnSelected { get; set; }

		public Item( string text, string icon, Action onSelected )
		{
			Text = text;
			Icon = icon;
			OnSelected = onSelected;
		}
	}
}
