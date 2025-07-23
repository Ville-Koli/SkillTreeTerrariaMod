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
	class TheUI : UIState
	{
		private SkillTreePanel panel;
		public StatBlockPlayer statBlockPlayer;
		public static NodeManager nodeManager;
		public static ConnectionManager connectionManager;
		public StatBlock statBlock = new(); // distribute an empty statblock
		public TheUI()
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override void OnInitialize()
		{
			TextureManager textureManager = SkillTreeGraphUI.textureManager;
			HoverOverUI hoverOverUI = new HoverOverUI(textureManager);
			connectionManager = new ConnectionManager(textureManager);
			nodeManager = new NodeManager(textureManager, hoverOverUI, statBlock);
			panel = new SkillTreePanel();

			panel.Width.Set(0, 1);
			panel.Height.Set(0, 1);
			panel.BackgroundColor = Color.Black;
			panel.SetHoverOverUI(hoverOverUI);
			panel.SetNodeManager(nodeManager);
			panel.SetConnectionManager(connectionManager);
			nodeManager.SetPanel(panel);
			Append(panel);

			string temp_text = "+0.5 Defence";
			MaxHealthNodeTrigger trigger = new MaxHealthNodeTrigger(2000);
			NodeUI root = nodeManager.CreateNode(new MaxHealthNodeTrigger(0), NodeType.Empty, "");
			List<NodeUI> nodeUIs = new List<NodeUI>() { root };
			for (int i = 0; i < 3000; ++i)
			{
				NodeUI button2 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
				nodeUIs.Add(button2);
			}
			for (int i = 1; i < 3001; ++i)
			{
				connectionManager.AutoConnect(panel, nodeUIs[i - 1], nodeUIs[i], ConnectionDirection.RIGHT);
			}
			connectionManager.AutoSync(root);
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