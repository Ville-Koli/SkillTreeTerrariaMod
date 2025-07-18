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
			HoverOverUI hoverOverUI = new HoverOverUI(panel, textureManager.GetUI(UIType.hoverOver).inactive, textureManager.GetUI(UIType.hoverOver).active, textureManager.transparent_box);
			panel.hoverOverUI = hoverOverUI;
			string temp_text = "+0.5 Defence";
			NodeUI button1 = new NodeUI(panel, textureManager.GetNode(NodeType.Empty).inactive, textureManager.GetNode(NodeType.Empty).active, new Vector2(50 * 0, 0), delegate { }, NodeType.Empty, hoverOverUI, "");
			panel.nodes.Add(button1);
			NodeUI button2 = new NodeUI(panel, textureManager.GetNode(NodeType.Defence).inactive, textureManager.GetNode(NodeType.Defence).active, new Vector2(50 * 2, 0), delegate { }, NodeType.Defence, hoverOverUI, temp_text);
			panel.nodes.Add(button2);
			NodeUI button3 = new NodeUI(panel, textureManager.GetNode(NodeType.Defence).inactive, textureManager.GetNode(NodeType.Defence).active, new Vector2(50 * 5, 0), delegate { }, NodeType.Defence, hoverOverUI, temp_text);
			panel.nodes.Add(button3);
			NodeUI button4 = new NodeUI(panel, textureManager.GetNode(NodeType.Defence).inactive, textureManager.GetNode(NodeType.Defence).active, new Vector2(50 * 5, 0), delegate { }, NodeType.Defence, hoverOverUI, temp_text);
			panel.nodes.Add(button4);
			NodeUI button5 = new NodeUI(panel, textureManager.GetNode(NodeType.Defence).inactive, textureManager.GetNode(NodeType.Defence).active, new Vector2(50 * 5, 0), delegate { }, NodeType.Defence, hoverOverUI, temp_text);
			panel.nodes.Add(button5);
			NodeUI button6 = new NodeUI(panel, textureManager.GetNode(NodeType.Defence).inactive, textureManager.GetNode(NodeType.Defence).active, new Vector2(50 * 5, 0), delegate { }, NodeType.Defence, hoverOverUI, temp_text);
			panel.nodes.Add(button6);
			NodeUI button7 = new NodeUI(panel, textureManager.GetNode(NodeType.Defence).inactive, textureManager.GetNode(NodeType.Defence).active, new Vector2(50 * 5, 0), delegate { }, NodeType.Defence, hoverOverUI, temp_text);
			panel.nodes.Add(button7);

			// add connection
			ac.AutoConnect(panel, button1, button2, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button2, button3, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button2, button4, ConnectionDirection.UP);
			ac.AutoConnect(panel, button3, button5, ConnectionDirection.UP);
			ac.AutoConnect(panel, button4, button5, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button5, button6, ConnectionDirection.RIGHT);
			ac.AutoConnect(panel, button6, button7, ConnectionDirection.DOWN);
		}
	}
}