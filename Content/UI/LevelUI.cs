using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using log4net.Core;

namespace Runeforge.Content.UI
{
	public class LevelUI : UIElement
	{
		public static UIText displayLevel;
		public Asset<Texture2D> levelBarHolder;
		public Vector2 location;
		public static Rectangle expBar;
		private Vector2 spriteOffset = new Vector2(18, 15);
		public LevelUI(Asset<Texture2D> levelBarHolder)
		{
			displayLevel = new UIText("Level 0", 1);
			this.levelBarHolder = levelBarHolder;
		}
		public override void OnInitialize()
		{
			base.OnInitialize();
			expBar.Width = 0;
			expBar.Height = 8;
			location = new Vector2(100, 0);
			Append(displayLevel);
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);
			expBar.X = (int)(location.X + spriteOffset.X);
			expBar.Y = (int)(location.Y + spriteOffset.Y);
			spriteBatch.Draw(levelBarHolder.Value, location, Color.White); // level bar holder
			spriteBatch.Draw(TextureAssets.MagicPixel.Value, expBar, Color.GreenYellow);
		}
		public static void SetExp(float percent)
		{
			expBar.Width = (int)(91 * percent);
		}
		public static void SetLevel(int level)
		{
			displayLevel.SetText($"Level {level}");
		}
	}
}