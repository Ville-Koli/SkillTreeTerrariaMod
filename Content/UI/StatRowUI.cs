using Microsoft.Xna.Framework;
using Terraria.UI;
using Runeforge.Content.SkillTree;
using Terraria.ModLoader;
using Terraria;
using Runeforge.Content.SkillTree.NodeScripts;
using System.Collections.Generic;
using System;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Runeforge.Content.UI
{
	class StatRowUI : UIElement
	{
		private UIImage statIcon;
		private UIText description;
		private NodeType type;
		public StatRowUI(Asset<Texture2D> statIcon, string statText, NodeType type)
		{
			description = new UIText(statText);
			this.statIcon = new UIImage(statIcon);
			this.type = type;
			description.Left.Set(statIcon.Width(), 0f);
			description.Top.Set(statIcon.Height() / 2 - 10, 0f);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void OnInitialize()
		{
			Append(statIcon);
			Append(description);
		}

		public void ChangeDescription(string description)
		{
			this.description.SetText(description);
		}
		public NodeType GetNodeType()
		{
			return type;
		}
	}
}