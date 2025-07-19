using Microsoft.Xna.Framework;
using Terraria.UI;
using Runeforge.Content.SkillTree;


namespace Runeforge.Content.UI
{
	class TheUI : UIState
	{
		private SkillTreePanel panel;
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
			NodeManager nodeManager = new NodeManager(panel, textureManager, hoverOverUI);

			string temp_text = "+0.5 Defence";
			NodeUI button1 = nodeManager.CreateNode(delegate { }, NodeType.Empty, "");
			NodeUI button2 = nodeManager.CreateNode(delegate { }, NodeType.Defence, temp_text);
			NodeUI button3 = nodeManager.CreateNode(delegate { }, NodeType.Defence, temp_text);
			NodeUI button4 = nodeManager.CreateNode(delegate { }, NodeType.Defence, temp_text);
			NodeUI button5 = nodeManager.CreateNode(delegate { }, NodeType.Defence, temp_text);
			NodeUI button6 = nodeManager.CreateNode(delegate { }, NodeType.Defence, temp_text);
			NodeUI button7 = nodeManager.CreateNode(delegate { }, NodeType.Defence, temp_text);

			// add connection
			ac.AutoConnectWithSync(panel, button1, button2, ConnectionDirection.RIGHT);
			ac.AutoConnectWithSync(panel, button2, button4, ConnectionDirection.UP);
			ac.AutoConnectWithSync(panel, button4, button5, ConnectionDirection.RIGHT);
			ac.AutoConnectWithSync(panel, button5, button6, ConnectionDirection.RIGHT);
			ac.AutoConnectWithSync(panel, button3, button5, ConnectionDirection.UP);
			ac.AutoConnectWithSync(panel, button7, button6, ConnectionDirection.DOWN);
			ac.AutoConnectWithSync(panel, button2, button3, ConnectionDirection.RIGHT);
		}
	}
}