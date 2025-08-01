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
		public SkillTreePanel panel;
		public TextureManager textureManager;
		public static Dictionary<int, ConnectionUI> connectionContainer = new();

		public Dictionary<int, ConnectionUI> GetConnections()
		{
			return connectionContainer;
		}
		/**
		<summary> 
		<para>Function, which returns currently active connections as a stringbuilder</para>
		Note that the connections, which are returned are formatted as such:

		Connections: ConnectionID,ConnectionID,ConnectionID,ConnectionID,...,

		</summary>

		<returns> StringBuilder, which contains a string of the id's of currently active connections </returns>
		**/
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
		/**
		<summary> 
		Function, which deactivates all connections
		</summary>
		**/
		public static void DeActivateAll()
		{
			foreach (var pair in connectionContainer)
			{
				ConnectionUI connection = pair.Value;
				connection.SetInActive();
			}
		}
		/**
		<summary> 
		<para>Function, which activates connection given a StringBuilder</para>
		Note that the connections should are be formatted as such:

		Connections: ConnectionID,ConnectionID,ConnectionID,ConnectionID,...,
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
					//ModContent.GetInstance<Runeforge>().Logger.Info("Trying to activate a connection with id: " + id);
					if (int.TryParse(id, out int intID) && connectionContainer.ContainsKey(intID))
					{
						ConnectionUI connection = connectionContainer[intID];
						connection.SetActive();
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
		<para>Function, which is used to create a connection between two nodes, where node A is assumed
		to be at the correct location and then B is adjusted accordingly </para>
		</summary>

		<param name="a"> node A which is going to be connected to node B </param>
		<param name="b"> node B which is going to be connected to node A </param>
		<param name="dir_asset"> the image two images used to show the connection on the panel</param>
		<param name="resulting_location_for_dir"> the location of the connection</param>
		<param name="resulting_location_for_b"> the location of the node B</param>
		<returns> 
		Function returns the created connection
		</returns>
		**/
		public ConnectionUI ConnectNodes(NodeUI a, NodeUI b,
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
		/**
		<summary> 
		Function, which generates new locations for connection and node B assuming
		that node fixed_node is at the correct location
		</summary>
		<param name="fixed_node"> node, which location is assumed to be correct </param>
		<param name="b"> node b, which is going to be connected to fixed_node </param>
		<param name="dir"> the direction of where b node is going to exist respect to fixed_node </param>
		<returns> 

		Function returns new locations for connection and b in the manner of
		(new connection location, new node b location)

		</returns>
		**/
		public (Vector2, Vector2) GetNewLocations(NodeUI fixed_node, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			Vector2 nodeALocation = fixed_node.GetLocation();
			Vector2 dir_dimensions = dir_asset.inactive.Size();
			Vector2 fixed_node_dimensions = fixed_node.inactive_node_image.Size();
			Vector2 b_dimensions = b.inactive_node_image.Size();
			Vector2 resulting_location_for_b;
			Vector2 resulting_location_for_dir;

			// probably could use a matrix or something to describe this
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
				case ConnectionDirection.LEFT:
					resulting_location_for_dir = nodeALocation + new Vector2(-dir_dimensions.X, MathF.Abs(fixed_node_dimensions.Y / 2 - dir_dimensions.Y / 2));
					resulting_location_for_b = nodeALocation + new Vector2(-b_dimensions.X - dir_dimensions.X, MathF.Abs(fixed_node_dimensions.Y / 2 - b_dimensions.Y / 2));
					return (resulting_location_for_dir, resulting_location_for_b);
				case ConnectionDirection.DIAGONAL_TOP_RIGHT:
					resulting_location_for_dir = nodeALocation + new Vector2(fixed_node_dimensions.X / 2, fixed_node_dimensions.Y / 2 - dir_dimensions.Y);
					resulting_location_for_b = resulting_location_for_dir + new Vector2(dir_dimensions.X - b_dimensions.X / 2, -b_dimensions.Y / 2);
					return (resulting_location_for_dir, resulting_location_for_b);
				case ConnectionDirection.DIAGONAL_TOP_LEFT:
					resulting_location_for_dir = nodeALocation + fixed_node_dimensions / 2 - dir_dimensions;
					resulting_location_for_b = resulting_location_for_dir - b_dimensions / 2;
					return (resulting_location_for_dir, resulting_location_for_b);
				case ConnectionDirection.DIAGONAL_BOTTOM_RIGHT:
					resulting_location_for_dir = nodeALocation + fixed_node_dimensions / 2;
					resulting_location_for_b = resulting_location_for_dir + dir_dimensions - b_dimensions / 2;
					return (resulting_location_for_dir, resulting_location_for_b);
				case ConnectionDirection.DIAGONAL_BOTTOM_LEFT:
					resulting_location_for_dir = nodeALocation + new Vector2(fixed_node_dimensions.X - dir_dimensions.X - fixed_node_dimensions.X / 2, fixed_node_dimensions.Y / 2);
					resulting_location_for_b = resulting_location_for_dir + new Vector2(0, dir_dimensions.Y) - b_dimensions / 2;
					return (resulting_location_for_dir, resulting_location_for_b);
			}
			return (Vector2.Zero, Vector2.Zero);
		}
		/**
		<summary> 
		Function, which applies delta vector to nodes location
		</summary>
		<param name="node"> node, which location you wish to edit by delta </param>
		<param name="delta"> the amount you want to edit the nodes location by </param>
		**/
		public void ApplyDelta(NodeUI node, Vector2 delta)
		{
			node.SetLocation(node.GetLocation() + delta);
		}
		/**
		<summary> 
		Function, which applies delta vector to connections location
		</summary>
		<param name="conn"> connection, which location you wish to edit by delta </param>
		<param name="delta"> the amount you want to edit the connections location by </param>
		**/
		public void ApplyDelta(ConnectionUI conn, Vector2 delta)
		{
			conn.SetLocation(conn.GetLocation() + delta);
		}
		/**
		<summary> 
		Function, which corrects node locations from the perspective of node b
		</summary>
		<param name="b"> node and all nodes connected to it of which locations you wish to edit by delta </param>
		<param name="delta"> the amount you want to edit the nodes and connections location by </param>
		**/
		public void AutoSyncNodes(NodeUI b, Vector2 delta)
		{
			PathingAlgorithms.ApplyFunctionToConnectedNodes(b, new(), delta, ApplyDelta, ApplyDelta);
		}
		/**
		<summary> 
		Function, which corrects node locations from the perspective of node b
		</summary>
		<param name="b"> node and all nodes and connections connected to it of which locations you wish to correct </param>
		**/
		public void AutoSync(NodeUI b)
		{
			PathingAlgorithms.CorrectNodeLocations(b, new(), GetNewLocations);
		}
		/**
		<summary> 
		Function, which creates a connection between a and b and corrects the locations connected to b such that ui
		isn't broken
		</summary>
		<param name="a"> node, which you wish to connect node b to with direction of dir </param>
		<param name="b"> node, which you connect to a</param>
		<param name="dir"> connection direction </param>

		<returns> returns the created connection and may return null if the direction does not exist in textureManager </returns>
		**/
		public ConnectionUI AutoConnectWithSync(NodeUI a, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			if (dir_asset.inactive != null && dir_asset.active != null)
			{
				(Vector2 resDir, Vector2 resB) results = GetNewLocations(a, b, dir);
				AutoSyncNodes(b, results.resB - b.location);
				return ConnectNodes(a, b, dir_asset, dir, results.resDir, results.resB);
			}
			return null;
		}
		/**
		<summary> 
		Function, which creates a connection between a and b and only sets node B and the connection to have correct locations respect to node A
		(if A already has connections connected to it, those are not broken, but the connections connected to B might be)
		</summary>
		<param name="a"> node, which you wish to connect node b to with direction of dir </param>
		<param name="b"> node, which you connect to a</param>
		<param name="dir"> connection direction </param>

		<returns> returns the created connection and may return null if the direction does not exist in textureManager </returns>
		**/
		public ConnectionUI AutoConnect(NodeUI a, NodeUI b, ConnectionDirection dir)
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) dir_asset = textureManager.GetDirection(dir);
			if (dir_asset.inactive != null && dir_asset.active != null)
			{
				(Vector2 resDir, Vector2 resB) results = GetNewLocations(a, b, dir);
				return ConnectNodes(a, b, dir_asset, dir, results.resDir, results.resB);
			}
			return null;
		}
		public ConnectionManager(SkillTreePanel panel, TextureManager textureManager)
		{
			this.textureManager = textureManager;
			this.panel = panel;
		}
	}
}