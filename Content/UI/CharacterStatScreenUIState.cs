using Microsoft.Xna.Framework;
using Terraria.UI;
using Runeforge.Content.SkillTree;
using Terraria.ModLoader;
using Terraria;
using Runeforge.Content.SkillTree.NodeScripts;
using System.Collections.Generic;
using System;
using Terraria.GameContent.UI.Elements;
using System.Linq;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;


namespace Runeforge.Content.UI
{
	class CharacterScreenState : UIState
	{
		private UIPanel panel;
		private List<StatRowUI> stats = new();
		public CharacterScreenState()
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
		public override void OnActivate()
		{
			base.OnActivate();
			foreach (var stat in stats)
			{
				stat.ChangeDescription(SkillTreeUIState.nodeManager.statBlock.GetStat(stat.GetNodeType()).ToString());
			}
        }

		public override void OnInitialize()
		{
			panel = new UIPanel();
			panel.Width.Set(300, 0f);
			panel.Height.Set(0, 1f);
			panel.HAlign = 1;
			panel.BackgroundColor = Color.Black;
			Append(panel);
			UIText header = new UIText("Character Screen", 1, false);
			panel.Append(header);
			int maxAmountOfNodeTypes = Enum.GetValues(typeof(NodeType)).Cast<int>().Max();
			int approxAssetHeight = SkillTreeGraphModSystem.textureManager.GetStat(NodeType.Defence).Height() + 6;
			for (int i = 0; i < maxAmountOfNodeTypes; ++i)
			{
				NodeType type = (NodeType)i;
				Asset<Texture2D> asset = SkillTreeGraphModSystem.textureManager.GetStat(type);
				if (asset != null)
				{
					StatRowUI statRowUI = new StatRowUI(asset, SkillTreeUIState.nodeManager.statBlock.GetStat(type).ToString(), type);
					statRowUI.Top.Set(approxAssetHeight * i, 0);
					ModContent.GetInstance<Runeforge>().Logger.Info($"trying to set a stat to character screen at: {approxAssetHeight * i + 10} {approxAssetHeight} {i} {asset.Name}");
					panel.Append(statRowUI);
					stats.Add(statRowUI);
				}
			}
		}
	}
}