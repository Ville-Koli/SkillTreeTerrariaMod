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
using System.IO;
using System;
using Terraria.UI;

namespace Runeforge.Content.UI
{
	public enum StatusCodes
	{
		Success,
		Failed
	}

	public class SkillTreeLoader
	{
		public ConnectionManager connectionManager;
		public NodeManager nodeManager;
		public static readonly string nodesLocation = "Runeforge/data/nodes.txt";
		public static readonly string connectionLocation = "Runeforge/data/connections.txt";

		public SkillTreeLoader(ConnectionManager connectionManager, NodeManager nodeManager)
		{
			this.connectionManager = connectionManager;
			this.nodeManager = nodeManager;
		}

		public void LoadData()
		{
			LoadNodes();
			LoadConnections();
		}

		public void LoadNodes()
		{
			try
			{
				byte[] nodeBytes = ModContent.GetFileBytes(nodesLocation);
				int i = 0;
				string line = "";
				ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING NODES] Byte found and its size is: {nodeBytes.Length}");
				while (nodeBytes.Length > i)
				{
					char currentCharacter = (char)nodeBytes[i];
					if (currentCharacter == '\n')
					{
						(StatusCodes status, UIElement element, string reason) result = ParseNode(line);
						ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING NODES] Parsed a node: {result.reason}");
						line = ""; // reset line
					}
					else
					{
						line += currentCharacter;
					}
					i++;
				}
				if (line != "")
				{
					(StatusCodes status, UIElement element, string reason) result = ParseNode(line);
					ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING NODES] Parsed a node: {result.reason}");
				}
			}
			catch (Exception e)
			{
				ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING NODES] error message: {e.Message}");
			}
		}
		public void LoadConnections()
		{
			try
			{
				byte[] nodeBytes = ModContent.GetFileBytes(connectionLocation);
				int i = 0;
				string line = "";
				ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING CONNECTION] Byte found and its size is: {nodeBytes.Length}");
				while (nodeBytes.Length > i)
				{
					char currentCharacter = (char)nodeBytes[i];
					if (currentCharacter == '\n')
					{
						(StatusCodes status, UIElement element, string reason) result = ParseConnection(line);
						ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING CONNECTION] Parsed a connection: {result.reason}");
						line = ""; // reset line
					}
					else
					{
						line += currentCharacter;
					}
					i++;
				}
				if (line != "")
				{
					(StatusCodes status, UIElement element, string reason) result = ParseConnection(line);
					ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING CONNECTION] Parsed a connection: {result.reason}");
				}
			}
			catch (Exception e)
			{
				ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [PARSING CONNECTION] error message: {e.Message}");
			}
		}

		public INodeTrigger GetTrigger(NodeType type, int element)
		{
			switch (type)
			{
				case NodeType.Defence:
					return new DefenceNodeTrigger(element);
				case NodeType.MeleeDamage:
					return new MeleeDamageNodeTrigger(element);
				case NodeType.MaxHealth:
					return new MaxHealthNodeTrigger(element);
				case NodeType.MaxMana:
					return new MaxManaNodeTrigger(element);
				case NodeType.ApplyBuff:
					return new ApplyBuffNodeTrigger(element);
			}
			return new EmptyNodeTrigger(); // default node
		}

		public (StatusCodes status, INodeTrigger trigger, string reason) ParseTrigger(NodeType type, string triggerElement)
		{
			if (int.TryParse(triggerElement, out int parsedTriggerElement))
			{
				INodeTrigger trigger = GetTrigger(type, parsedTriggerElement);
				if (trigger != null)
				{
					return (StatusCodes.Success, trigger, "Success");
				}
				return (StatusCodes.Failed, null, "Failed to get trigger");
			}
			return (StatusCodes.Failed, null, "Failed to parse trigger element");
		}
		public (StatusCodes status, NodeUI node, string reason) ParseNode(string node)
		{
			string[] nodeElements = node.Split("|"); // Type | Trigger Element | Description -> "Type", "Trigger Element", "Description" 
			if(nodeElements.Length != 3) return (StatusCodes.Failed, null, $"Failed at splitting as required length is 3 and the length is {nodeElements.Length}");

			string strNodeType = nodeElements[0];
			string strTrigger = nodeElements[1];
			string description = nodeElements[2];
			
			if (int.TryParse(strNodeType, out int type))
			{
				try
				{
					NodeType nodeType = (NodeType)type;
					(StatusCodes status, INodeTrigger trigger, string reason) result = ParseTrigger(nodeType, strTrigger);
					if (result.status == StatusCodes.Failed) return (StatusCodes.Failed, null, result.reason);
					NodeUI nodeui = nodeManager.CreateNode(result.trigger, nodeType, description);
					return (StatusCodes.Success, nodeui, "Success");
				}
				catch (Exception e)
				{
					return (StatusCodes.Failed, null, e.ToString());
				}
			}
			return (StatusCodes.Failed, null, "Failed at parsing");
		}
		public (StatusCodes status, ConnectionUI connection, string reason) ParseConnection(string connection)
		{
			string[] connectionElements = connection.Split(","); // a connection is "NodeID, NodeID, dir" -> "NodeID", " NodeID", "dir"
			if (connectionElements.Length != 3) return (StatusCodes.Failed, null, "array's length after splitting is not 3!");

			string strNodeID1 = connectionElements[0];
			string strNodeID2 = connectionElements[1];
			string strDirection = connectionElements[2];

			if (int.TryParse(strNodeID1, out int nodeID1) &&
				int.TryParse(strNodeID2, out int nodeID2) &&
				int.TryParse(strDirection, out int intDirection))
			{
				var nodes = nodeManager.GetNodes();
				if (nodes.ContainsKey(nodeID1) && nodes.ContainsKey(nodeID1))
				{
					try
					{
						// function expects that node's have already been parsed otherwise creating connections becomes impossible
						NodeUI node1 = nodes[nodeID1];
						NodeUI node2 = nodes[nodeID2];
						ConnectionDirection direction = (ConnectionDirection)intDirection;

						ConnectionUI connectionUI = connectionManager.AutoConnect(node1, node2, direction);
						return (StatusCodes.Success, connectionUI, "Success");

					}
					catch (Exception e)
					{
						return (StatusCodes.Failed, null, e.ToString());
					}
				}
				else
				{
					return (StatusCodes.Failed, null, "Node's dont exist in node manager");
				}
			}
			return (StatusCodes.Failed, null, $"Failed at parsing ids and directions: {strNodeID1}, {strNodeID2}, {strDirection}");
		}
	}
}