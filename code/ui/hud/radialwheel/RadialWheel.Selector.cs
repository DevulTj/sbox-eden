// Copyright (c) 2022 Ape Tavern, do not share, re-distribute or modify
// without permission of its author (insert_email_here)

using Sandbox;
using Sandbox.UI;
using System;

namespace Eden;

partial class RadialWheel
{
	private class PieSelector : Panel
	{
		private RadialWheel parentWheel;
		private float lerpedSelectionAngle = 0f;

		public PieSelector( RadialWheel parentWheel )
		{
			this.parentWheel = parentWheel;
		}

		public override void OnParentChanged()
		{
			base.OnParentChanged();
			GenerateTexture();
		}

		/// <summary>
		/// Generate our cool selection texture based on number of items in parent wheel
		/// </summary>
		private void GenerateTexture()
		{
			const int width = 512;
			const int height = 512;
			const int channels = 4;

			Vector2 circleSize = new Vector2( width, height );
			Vector2 circleCenter = circleSize / 2.0f;
			float circleRadius = width / 2f;

			// RGBA texture
			byte[] textureData = new byte[width * height * 4];
			void SetPixel( int x, int y, Color col )
			{
				textureData[( ( x + ( y * width ) ) * channels ) + 0] = col.r.ColorComponentToByte();
				textureData[( ( x + ( y * width ) ) * channels ) + 1] = col.g.ColorComponentToByte();
				textureData[( ( x + ( y * width ) ) * channels ) + 2] = col.b.ColorComponentToByte();
				textureData[( ( x + ( y * width ) ) * channels ) + 3] = col.a.ColorComponentToByte();
			}

			//
			// Is this pixel in a circle
			//
			bool InCircle( int x, int y, float radius )
			{
				return MathF.Pow( x - circleCenter.x, 2 ) + MathF.Pow( y - circleCenter.y, 2 ) < MathF.Pow( radius, 2 );
			}

			//
			// Is this pixel in a segment of the pie
			//
			bool InSegment( int x, int y )
			{
				float angle = MathF.Atan2( y - circleCenter.y, x - circleCenter.x ).RadianToDegree().NormalizeDegrees();

				// We do this to offset everything to match array indexing at 0
				return angle < parentWheel.AngleIncrement;
			}

			for ( int x = 0; x < width; x++ )
			{
				for ( int y = 0; y < height; y++ )
				{
					float pixelOpacity = 0;

					if ( InCircle( x, y, circleRadius ) && // Outer ring
						!InCircle( x, y, circleRadius * 0.6f ) && // Inner ring
						InSegment( x, y ) ) // Pie segment
					{
						pixelOpacity = 1.0f;
					}

					SetPixel( x, y, Color.White * pixelOpacity );
				}
			}

			var newTexture = Texture.Create( width, height )
				.WithStaticUsage()
				.WithData( textureData )
				.WithName( "PieSelector" )
				.Finish();

			Style.BackgroundImage = newTexture;
		}

		public override void Tick()
		{
			base.Tick();

			// Interpolate angle here because scss transition does a shit job of it
			float angle = parentWheel.ActiveIndex * parentWheel.AngleIncrement;
			lerpedSelectionAngle = lerpedSelectionAngle.LerpToAngle( angle, 50f * Time.Delta );

			float angleOffset = parentWheel.AngleIncrement / 2f; // Display icons in the middle of the selector

			var panelTransform = CreateStandardPanelTransform();
			panelTransform.AddRotation( 0, 0, lerpedSelectionAngle - angleOffset );
			Style.Transform = panelTransform;
		}

		/// <summary>
		/// Create a panel transform with all the shit we'd usually put in SCSS
		/// </summary>
		private PanelTransform CreateStandardPanelTransform()
		{
			var panelTransform = new PanelTransform();
			return panelTransform;
		}
	}
}
