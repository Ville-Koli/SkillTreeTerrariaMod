using Microsoft.Xna.Framework;
using Terraria.UI;
using Runeforge.Content.SkillTree;
using Terraria.ModLoader;
using Terraria;
using Runeforge.Content.SkillTree.NodeScripts;


namespace Runeforge.Content.UI
{
	class TheUI : UIState
	{
		private SkillTreePanel panel;
		public StatBlockPlayer statBlockPlayer;
		public static StatBlock statBlock = new();
		public TheUI()
		{
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			// probably can be fetched somewhere else to optimize further
			// let it be here for the time being
			if (statBlockPlayer == null && Main.LocalPlayer.active && Main.LocalPlayer != null)
			{
				statBlockPlayer = Main.LocalPlayer.GetModPlayer<StatBlockPlayer>();
				if (statBlockPlayer != null)
				{
					statBlockPlayer.ApplyStatBlock(statBlock);
					ModContent.GetInstance<Runeforge>().Logger.Info("UPDATING");
				}
			}
		}

		public void UpdatePlayerStatBlock()
		{
			if (Main.LocalPlayer.active && Main.LocalPlayer != null)
			{
				statBlockPlayer = Main.LocalPlayer.GetModPlayer<StatBlockPlayer>();
				statBlockPlayer.ApplyStatBlock(statBlock);
				ModContent.GetInstance<Runeforge>().Logger.Info("UPDATING");
			}
		}

		public override void OnInitialize()
		{
			TextureManager textureManager = SkillTreeGraphUI.textureManager;
			AutoConnector ac = new AutoConnector(textureManager);
			panel = new SkillTreePanel();
			panel.Width.Set(0, 1);
			panel.Height.Set(0, 1);
			panel.BackgroundColor = Color.Black;
			Append(panel);
			HoverOverUI hoverOverUI = new HoverOverUI(panel, textureManager);
			NodeManager nodeManager = new NodeManager(panel, textureManager, hoverOverUI, statBlock);

			string temp_text = "+0.5 Defence";
			DefenceNodeTrigger trigger = new DefenceNodeTrigger(1);
			NodeUI button1 = nodeManager.CreateNode(new DefenceNodeTrigger(0), NodeType.Empty, "");
			NodeUI button2 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
			NodeUI button3 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
			NodeUI button4 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
			NodeUI button5 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
			NodeUI button6 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
			NodeUI button7 = nodeManager.CreateNode(trigger, NodeType.Defence, temp_text);
			// add connections
			ac.AutoConnect(panel, button1, button2, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button2, button4, ConnectionDirection.UP);
			ac.AutoConnect(panel, button4, button5, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button5, button6, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button3, button5, ConnectionDirection.UP);
			ac.AutoConnect(panel, button7, button6, ConnectionDirection.DOWN);
			//ac.AutoConnect(panel, button2, button3, ConnectionDirection.RIGHT);
			ac.AutoSync(button1);
		}
	}
}