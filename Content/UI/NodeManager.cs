using Microsoft.Xna.Framework.Graphics;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Microsoft.Xna.Framework;
using Runeforge.Content.SkillTree.NodeScripts;
using System.Collections.Generic;
using Terraria.ModLoader;
using System.Text;
using System.Linq;
using Terraria;

namespace Runeforge.Content.UI
{
	public class NodeManager
	{
		public SkillTreePanel panel;
		public TextureManager textureManager;
		public HoverOverUI hoverOverUI;
		public StatBlock statBlock;
		public NodeUI rootNode;
		private static Dictionary<int, NodeUI> nodeContainer = new();
		public NodeManager(SkillTreePanel panel, TextureManager textureManager, HoverOverUI hoverOverUI, StatBlock statBlock)
		{
			this.statBlock = statBlock;
			this.panel = panel;
			this.textureManager = textureManager;
			this.hoverOverUI = hoverOverUI;
		}
		public NodeManager(TextureManager textureManager, HoverOverUI hoverOverUI, StatBlock statBlock)
		{
			this.statBlock = statBlock;
			this.textureManager = textureManager;
			this.hoverOverUI = hoverOverUI;
		}

		public void SetPanel(SkillTreePanel panel)
		{
			this.panel = panel;
		}
		public static StringBuilder GetActiveNodesAsStringBuilder()
		{
			StringBuilder activeNodes = new StringBuilder("Nodes: ");
			foreach (var pair in nodeContainer)
			{
				NodeUI node = pair.Value;
				if (node.active)
				{
					activeNodes.Append($"{node.GetID()},");
				}
			}
			return activeNodes;
		}
		public static bool ActivateNodesFromStringBuilder(StringBuilder activeNodes)
		{
			string activeNodesString = activeNodes.ToString();
			string[] splittedString = activeNodesString.Split(":");
			if (splittedString.Length == 2)
			{
				string[] activeIDs = splittedString[1].Split(",");
				foreach (var id in activeIDs)
				{
					if (int.TryParse(id, out int intID))
					{
						if (nodeContainer.ContainsKey(intID))
						{
							NodeUI node = nodeContainer[intID];
							node.SetActive();
						}
					}
				} 
			}
			else
			{
				return false;
			}
			return true;
		}
		public static void DeActivateAll()
		{
			foreach (var pair in nodeContainer)
			{
				NodeUI node = pair.Value;
				node.SetInActive();
			}
		}
		public NodeUI CreateNode(INodeTrigger trigger, NodeType type, string description)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) texture = textureManager.GetNode(type);
			NodeUI nodeUI = new NodeUI(panel, texture.inactive, texture.active, Vector2.Zero, trigger, type, hoverOverUI, statBlock, description);
			if (!nodeContainer.ContainsKey(nodeUI.GetID()))
				nodeContainer.Add(nodeUI.GetID(), nodeUI);
			if (rootNode == null && type == NodeType.Empty)
				rootNode = nodeUI;
			return nodeUI;
		}

		public void ApplyLoadedStatBlock(StatBlock loadedStatBlock)
		{
			statBlock = loadedStatBlock;
			foreach (var pair in nodeContainer)
			{
				pair.Value.statBlock = loadedStatBlock;
			}
		}

		public StatBlock GetStatBlockReference()
		{
			return statBlock;
		}

		public Dictionary<int, NodeUI> GetNodes()
		{
			return nodeContainer;
		}
	}
}