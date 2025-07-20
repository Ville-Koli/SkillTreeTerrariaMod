using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Runeforge.Content.Buffs;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using System.Text;

namespace Runeforge.Content.UI
{
	public class ConnectionManager
	{
		public TextureManager textureManager;
		public static Dictionary<int, ConnectionUI> connectionContainer = new();

		public Dictionary<int, ConnectionUI> GetConnections()
		{
			return connectionContainer;
		}

		public static StringBuilder GetActiveConnectionsAsStringBuilder()
		{
			StringBuilder activeConnections = new StringBuilder("Connections: ");
			foreach (var pair in connectionContainer)
			{
				ConnectionUI connectionUI = pair.Value;
				if (connectionUI.active)
				{
					activeConnections.Append($"{connectionUI.GetID()},");
				}
			}
			return activeConnections;
		}
		public static void DeActivateAll()
		{
			foreach (var pair in connectionContainer)
			{
				ConnectionUI connection = pair.Value;
				connection.SetInActive();
			}
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
					//ModContent.GetInstance<Runeforge>().Logger.Info("Trying to activate a connection with id: " + id);
					if (int.TryParse(id, out int intID))
					{
						//ModContent.GetInstance<Runeforge>().Logger.Info("Activating connection");
						ConnectionUI connection = connectionContainer[intID];
						connection.SetActive();
					}
				} 
			}
			else
			{
				return false;
			}
			return true;
		}
		public ConnectionUI ConnectNodes(SkillTreePanel panel, NodeUI a, NodeUI b,
		(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset, ConnectionDirection dir,
		Vector2 resulting_location_for_dir, Vector2 resulting_location_for_b)
		{
			b.SetLocation(resulting_location_for_b);
			ConnectionUI conn = new ConnectionUI(panel, dir_asset.inactive, dir_asset.active, a, b, dir, resulting_location_for_dir);
			a.AddConnection(conn);
			b.AddConnection(conn);
			if (!connectionContainer.ContainsKey(conn.GetID()))
				connectionContainer.Add(conn.GetID(), conn);
			return conn;
		}
		public (Vector2, Vector2) GetNewLocations(NodeUI fixed_node, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			Vector2 nodeALocation = fixed_node.GetLocation();
			Vector2 dir_dimensions = dir_asset.inactive.Size();
			Vector2 fixed_node_dimensions = fixed_node.inactive_node_image.Size();
			Vector2 b_dimensions = b.inactive_node_image.Size();
			Vector2 resulting_location_for_b;
			Vector2 resulting_location_for_dir;
			switch (dir)
			{
				case ConnectionDirection.UP:
					resulting_location_for_dir = nodeALocation + new Vector2(MathF.Abs(fixed_node_dimensions.X / 2 - dir_dimensions.X / 2), -dir_dimensions.Y);
					resulting_location_for_b = nodeALocation + new Vector2(MathF.Abs(fixed_node_dimensions.X / 2 - b_dimensions.X / 2), -dir_dimensions.Y - b_dimensions.Y);
					return (resulting_location_for_dir, resulting_location_for_b);
				case ConnectionDirection.DOWN:
					resulting_location_for_dir = nodeALocation + new Vector2(MathF.Abs(fixed_node_dimensions.X / 2 - dir_dimensions.X / 2), fixed_node_dimensions.Y);
					resulting_location_for_b = nodeALocation + new Vector2(MathF.Abs(fixed_node_dimensions.X / 2 - b_dimensions.X / 2), fixed_node_dimensions.Y + dir_dimensions.Y);
					return (resulting_location_for_dir, resulting_location_for_b);
				case ConnectionDirection.RIGHT:
					resulting_location_for_dir = nodeALocation + new Vector2(fixed_node_dimensions.X, MathF.Abs(fixed_node_dimensions.Y / 2 - dir_dimensions.Y / 2));
					resulting_location_for_b = nodeALocation + new Vector2(fixed_node_dimensions.X + dir_dimensions.X, MathF.Abs(fixed_node_dimensions.Y / 2 - b_dimensions.Y / 2));
					return (resulting_location_for_dir, resulting_location_for_b);
			}
			return (Vector2.Zero, Vector2.Zero);
		}

		public void ApplyDelta(NodeUI node, Vector2 delta)
		{
			node.SetLocation(node.GetLocation() + delta);
		}
		public void ApplyDelta(ConnectionUI conn, Vector2 delta)
		{
			conn.SetLocation(conn.GetLocation() + delta);
		}

		public void AutoSyncNodes(NodeUI b, Vector2 delta)
		{
			PathingAlgorithms.ApplyFunctionToConnectedNodes(b, new(), delta, ApplyDelta, ApplyDelta);
		}
		public void AutoSync(NodeUI b)
		{
			PathingAlgorithms.CorrectNodeLocations(b, new(), GetNewLocations);
		}
		public ConnectionUI AutoConnectWithSync(SkillTreePanel panel, NodeUI a, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			if (dir_asset.inactive != null && dir_asset.active != null)
			{
				(Vector2 resDir, Vector2 resB) results = GetNewLocations(a, b, dir);
				AutoSyncNodes(b, results.resB - b.location);
				return ConnectNodes(panel, a, b, dir_asset, dir, results.resDir, results.resB);
			}
			return null;
		}
		public ConnectionUI AutoConnect(SkillTreePanel panel, NodeUI a, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			if (dir_asset.inactive != null && dir_asset.active != null)
			{
				(Vector2 resDir, Vector2 resB) results = GetNewLocations(a, b, dir);
				return ConnectNodes(panel, a, b, dir_asset, dir, results.resDir, results.resB);
			}
			return null;
		}
		public ConnectionManager(TextureManager textureManager)
		{
			this.textureManager = textureManager;
		}
	}
}