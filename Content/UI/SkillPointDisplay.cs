
using System.Collections.Generic;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader;
using Terraria;

namespace Runeforge.Content.UI
{
	public class SkillPointDisplay : UIElement
	{
		public static UIText skillPointAmountDisplay;
		public UIImage skillPointIcon;
		public SkillPointDisplay(Asset<Texture2D> skillPointIcon)
		{
			this.skillPointIcon = new UIImage(skillPointIcon);
			skillPointAmountDisplay = new UIText("0", 1);
			skillPointAmountDisplay.Left.Set(skillPointIcon.Width(), 0f);
			skillPointAmountDisplay.Top.Set(skillPointIcon.Height() / 2 - 10, 0f);
		}
		public override void OnInitialize()
		{
			base.OnInitialize();
			HAlign = 1;
			MarginRight = 100;
			Append(skillPointIcon);
			Append(skillPointAmountDisplay);
			ModContent.GetInstance<Runeforge>().Logger.Info("DIMENSIONS: " + skillPointIcon.GetDimensions().Width);
		}

		public static void EditSkillPointAmount(int amount)
		{
			skillPointAmountDisplay.SetText($"{amount}");
		}
	}
}