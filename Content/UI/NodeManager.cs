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
		/**
		<summary> 
		<para>Function, which returns currently active nodes as a stringbuilder</para>
		Note that the nodes, which are returned are formatted as such:

		Nodes: NodeID,NodeID,NodeID,NodeID,...,

		</summary>

		<returns> StringBuilder, which contains a string of the id's of currently active nodes </returns>
		**/
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
		/**
		<summary> 
		<para>Function, which activates nodes given a StringBuilder</para>
		Note that the nodes should are be formatted as such:

		Nodes: NodeID,NodeID,NodeID,NodeID,...,
		</summary>

		<returns> 

		Function returns bool on whether it succesfully managed to parse through the string
		contained in the StringBuilder or not

		</returns>
		**/
		public static bool ActivateNodesFromStringBuilder(StringBuilder activeNodes)
		{
			string activeNodesString = activeNodes.ToString();
			string[] splittedString = activeNodesString.Split(":");
			if (splittedString.Length == 2)
			{
				string[] activeIDs = splittedString[1].Split(",");
				foreach (var id in activeIDs)
				{
					if (int.TryParse(id, out int intID) && nodeContainer.ContainsKey(intID))
					{
						NodeUI node = nodeContainer[intID];
						node.SetActive();
					}
					else
					{
						return false;
					}
				}
			}
			else
			{
				return false;
			}
			return true;
		}
		/**
		<summary> 
		Function, which deactivates all nodes
		</summary>
		**/
		public static void DeActivateAll()
		{
			foreach (var pair in nodeContainer)
			{
				NodeUI node = pair.Value;
				node.SetInActive();
			}
		}
		/**
		<summary> 
		<para>Function, which is used to create a node when given trigger, type and description. This node is then added to nodeContainer</para>
		</summary>

		<param name="trigger"> is the action that the node when the node is activated </param>
		<param name="type"> describes what type the node is </param>
		<param name="description"> describes what the node does and is shown on hover over ui </param>

		<returns> 
		Function returns the created node
		</returns>
		**/
		public NodeUI CreateNode(INodeTrigger trigger, NodeType type, string description, int cost)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) texture = textureManager.GetNode(type);
			NodeUI nodeUI = new NodeUI(panel, texture.inactive, texture.active, Vector2.Zero, trigger, type, hoverOverUI, statBlock, description, cost);
			if (!nodeContainer.ContainsKey(nodeUI.GetID()))
				nodeContainer.Add(nodeUI.GetID(), nodeUI);
			if (rootNode == null && type == NodeType.Empty)
				rootNode = nodeUI;
			return nodeUI;
		}
		/**
		<summary> 
		<para>Function, which changes the statblock reference to all given nodes</para>
		</summary>

		<param name="loadedStatBlock"> the statblock to be changed to all nodes </param>
		**/
		public void ApplyLoadedStatBlock(StatBlock loadedStatBlock)
		{
			statBlock = loadedStatBlock;
			foreach (var pair in nodeContainer)
			{
				pair.Value.SetStatBlock(loadedStatBlock);
			}
		}
		/**
		<summary> 
		<para>Function that gets statblock</para>
		</summary>

		<returns> statblock that is currently active on all nodes </returns>
		**/
		public StatBlock GetStatBlockReference()
		{
			return statBlock;
		}
		/**
		<summary> 
		<para>Function that gets nodeContainer</para>
		</summary>

		<returns> a dictionary where key is the node id and value is the node </returns>
		**/
		public Dictionary<int, NodeUI> GetNodes()
		{
			return nodeContainer;
		}
		/**
		<summary> 
		<para>Function that sets the panel for node manager</para>
		</summary>

		<param name="panel"> the panel for node manager</param>
		**/
		public void SetPanel(SkillTreePanel panel)
		{
			this.panel = panel;
		}
	}
}