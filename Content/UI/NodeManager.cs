using Microsoft.Xna.Framework.Graphics;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Microsoft.Xna.Framework;

namespace Runeforge.Content.UI
{
	public class NodeManager
	{
		public SkillTreePanel panel;
		public TextureManager textureManager;
		public HoverOverUI hoverOverUI;
		public NodeManager(SkillTreePanel panel, TextureManager textureManager, HoverOverUI hoverOverUI)
		{
			this.panel = panel;
			this.textureManager = textureManager;
			this.hoverOverUI = hoverOverUI;
		}

		public NodeUI CreateNode(ModifyPlayer modifyPlayer, NodeType type, string description)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) texture = textureManager.GetNode(type);
			NodeUI nodeUI = new NodeUI(panel, texture.inactive, texture.active, Vector2.Zero, modifyPlayer, type, hoverOverUI, description);
			panel.nodes.Add(nodeUI);
			return nodeUI;
		}
	}
}