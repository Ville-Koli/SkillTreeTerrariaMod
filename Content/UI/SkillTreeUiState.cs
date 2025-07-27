using Microsoft.Xna.Framework;
using Terraria.UI;
using Runeforge.Content.SkillTree;
using Terraria.ModLoader;
using Terraria;
using Runeforge.Content.SkillTree.NodeScripts;
using System.Collections.Generic;
using System;


namespace Runeforge.Content.UI
{
	class SkillTreeUIState : UIState
	{
		private SkillTreePanel panel;
		public StatBlockPlayer statBlockPlayer;
		public static NodeManager nodeManager;
		public static ConnectionManager connectionManager;
		public StatBlock statBlock = new(); // distribute an empty statblock
		public SkillTreeUIState()
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void OnInitialize()
		{
			TextureManager textureManager = SkillTreeGraphModSystem.textureManager;
			HoverOverUI hoverOverUI = new HoverOverUI(textureManager);
			nodeManager = new NodeManager(textureManager, hoverOverUI, statBlock);
			panel = new SkillTreePanel();
			connectionManager = new ConnectionManager(panel, textureManager);
			panel.Width.Set(0, 1);
			panel.Height.Set(0, 1);
			panel.BackgroundColor = Color.Black;
			panel.SetHoverOverUI(hoverOverUI);
			panel.SetNodeManager(nodeManager);
			panel.SetConnectionManager(connectionManager);
			nodeManager.SetPanel(panel);
			Append(panel);
			
			SkillTreeLoader loader = new SkillTreeLoader(connectionManager, nodeManager);
			loader.LoadData();

			foreach (var pair in nodeManager.GetNodes())
			{
				NodeUI node = pair.Value;
				node.basePosition = node.location;
			}
			foreach (var pair in connectionManager.GetConnections())
			{
				ConnectionUI conn = pair.Value;
				conn.basePosition = conn.location;
			}
		}
	}
}