using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.ModLoader;
using System;

namespace Runeforge.Content.UI
{
	public class PathingAlgorithms
	{

		public static bool DoesPossiblePathExistToEmpty(NodeUI start, Dictionary<int, bool> visited)
		{
			if (start.GetNodeType() == NodeType.Empty) return true;
			ModContent.GetInstance<Runeforge>().Logger.Info("\tNOT EMPTY");
			bool path = false;
			if (!visited.ContainsKey(start.GetID())) { visited.Add(start.GetID(), true); ModContent.GetInstance<Runeforge>().Logger.Info("\tADDED TO VISITED!"); }
			foreach (var conn in start.GetConnections())
			{
				NodeUI node = NodeUI.GetNeighbourNode(conn, start);
				if (node.GetNodeType() == NodeType.Empty) return true;
				if (node.active)
				{
					ModContent.GetInstance<Runeforge>().Logger.Info("\t\tNEIGHBOUR NOT EMPTY");
					if (!visited.ContainsKey(node.GetID()))
					{
						ModContent.GetInstance<Runeforge>().Logger.Info("CHECKING NEIGBHOUR: " + path);
						path = path || DoesPossiblePathExistToEmpty(node, visited);
					}
				}
			}
			return path;
		}
		public static void ApplyFunctionToConnectedNodes(NodeUI start, Dictionary<int, bool> visited, Vector2 delta, Action<NodeUI, Vector2> applyToNode, Action<ConnectionUI, Vector2> applyToConnection)
		{
			ModContent.GetInstance<Runeforge>().Logger.Info("\tNOT EMPTY");
			if (!visited.ContainsKey(start.GetID())) { visited.Add(start.GetID(), true); ModContent.GetInstance<Runeforge>().Logger.Info("\tADDED TO VISITED!"); }
			foreach (var conn in start.GetConnections())
			{
				NodeUI node = NodeUI.GetNeighbourNode(conn, start);
				if (!visited.ContainsKey(node.GetID()))
				{
					applyToNode(node, delta);
					applyToConnection(conn, delta);
					ApplyFunctionToConnectedNodes(node, visited, delta, applyToNode, applyToConnection);
				}
			}
		}
		public static void CorrectNodeLocations(NodeUI start, Dictionary<int, bool> visited, Func<NodeUI, NodeUI, ConnectionDirection, (Vector2, Vector2)> getNewLocations)
		{
			ModContent.GetInstance<Runeforge>().Logger.Info("\tNOT EMPTY");
			if (!visited.ContainsKey(start.GetID())) { visited.Add(start.GetID(), true); ModContent.GetInstance<Runeforge>().Logger.Info("\tADDED TO VISITED!"); }
			foreach (var conn in start.GetConnections())
			{
				NodeUI node = NodeUI.GetNeighbourNode(conn, start);
				if (!visited.ContainsKey(node.GetID()))
				{
					(Vector2 dirLocation, Vector2 neighbourLocation) results = getNewLocations(start, node, conn.direction_from_node);
					node.SetLocation(results.neighbourLocation);
					conn.SetLocation(results.dirLocation);
					CorrectNodeLocations(node, visited, getNewLocations);
				}
			}
		}
	}
}