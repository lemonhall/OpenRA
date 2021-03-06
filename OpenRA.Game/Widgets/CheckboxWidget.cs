#region Copyright & License Information
/*
 * Copyright 2007-2011 The OpenRA Developers (see AUTHORS)
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation. For more information,
 * see COPYING.
 */
#endregion

using System;
using System.Drawing;
using OpenRA.Graphics;

namespace OpenRA.Widgets
{
	public class CheckboxWidget : ButtonWidget
	{
		public string CheckType = "checked";
		public Func<string> GetCheckType;
		public Func<bool> IsChecked = () => false;
		public int BaseLine = 1;
		public int CheckOffset = 2;
		public bool HasPressedState = ChromeMetrics.Get<bool>("CheckboxPressedState");

		public CheckboxWidget()
		{
			GetCheckType = () => CheckType;
		}

		protected CheckboxWidget(CheckboxWidget other)
			: base(other)
		{
			CheckType = other.CheckType;
			GetCheckType = other.GetCheckType;
			IsChecked = other.IsChecked;
			BaseLine = other.BaseLine;
			CheckOffset = other.CheckOffset;
			HasPressedState = other.HasPressedState;
		}

		public override void Draw()
		{
			var disabled = IsDisabled();
			var font = Game.Renderer.Fonts[Font];
			var color = GetColor();
			var colordisabled = GetColorDisabled();
			var contrast = GetContrastColor();
			var rect = RenderBounds;
			var textSize = font.Measure(Text);
			var check = new Rectangle(rect.Location, new Size(Bounds.Height, Bounds.Height));
			var state = disabled ? "checkbox-disabled" :
						Depressed && HasPressedState ? "checkbox-pressed" :
						Ui.MouseOverWidget == this ? "checkbox-hover" :
						"checkbox";

			WidgetUtils.DrawPanel(state, check);
			var position = new float2(rect.Left + rect.Height * 1.5f, RenderOrigin.Y - BaseLine + (Bounds.Height - textSize.Y)/2);

			if (Contrast)
				font.DrawTextWithContrast(Text, position,
					disabled ? colordisabled : color, contrast, 2);
			else
				font.DrawText(Text, position,
					disabled ? colordisabled : color);

			if (IsChecked() || (Depressed && HasPressedState && !disabled))
			{
				var checkType = GetCheckType();
				if (HasPressedState && (Depressed || disabled))
					checkType += "-disabled";

				var offset = new float2(rect.Left + CheckOffset, rect.Top + CheckOffset);
				WidgetUtils.DrawRGBA(ChromeProvider.GetImage("checkbox-bits", checkType), offset);
			}
		}

		public override Widget Clone() { return new CheckboxWidget(this); }
	}
}
