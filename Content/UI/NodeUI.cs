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

namespace Runeforge.Content.UI
{
	public class NodeUI : UIElement
	{
		private SkillTreePanel mainPanel;
		private List<ConnectionUI> connections = new();
		private NodeType type;
		private ModifyPlayer modification;
		public UIImage node_image;
		public Asset<Texture2D> active_node_image;
		public Asset<Texture2D> inactive_node_image;
		public Vector2 location;
		public bool active;
		private int id;
		private static int global_id = 0;
		public NodeUI(SkillTreePanel panel, Asset<Texture2D> inactive, Asset<Texture2D> active, Vector2 location, ModifyPlayer modification, NodeType type)
		{
			mainPanel = panel;
			node_image = new UIImage(inactive);
			inactive_node_image = inactive;
			this.location = location;
			this.modification = modification;
			this.type = type;
			active_node_image = active;
			id = global_id;
			global_id++;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch) {
			base.DrawSelf(spriteBatch); // This ensures the Draw call is propagated to the children(s)
			if (node_image.IsMouseHovering) {
				Main.hoverItemName = "Click to see what happens\n+0.5 Defence";
			}
		}

		public static bool DoesPossiblePathExistToEmpty(NodeUI start, Dictionary<int, bool> visited)
		{
			if (start.type == NodeType.Empty) return true;
			ModContent.GetInstance<Runeforge>().Logger.Info("\tNOT EMPTY");
			bool path = false;
			if (!visited.ContainsKey(start.GetID())) { visited.Add(start.GetID(), true); ModContent.GetInstance<Runeforge>().Logger.Info("\tADDED TO VISITED!"); }
			foreach (var conn in start.GetConnections())
			{
				NodeUI node = GetNeighbourNode(conn, start);
				if (node.type == NodeType.Empty) return true;
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

		public bool CanNodeBeDeActivated(List<ConnectionUI> activeNeighbours)
		{
			if (!active) return false;
			bool deactivation = true;
			active = false;
			ModContent.GetInstance<Runeforge>().Logger.Info("MAPPING PATHING! Current connections: " + connections.Count);
			foreach (var conn in activeNeighbours)
			{
				NodeUI node = GetNeighbourNode(conn);
				deactivation = deactivation && DoesPossiblePathExistToEmpty(node, new());
				ModContent.GetInstance<Runeforge>().Logger.Info("Trying neigbhour: " + deactivation);
			}
			active = true;
			return deactivation;
		}

		public NodeUI GetNeighbourNode(ConnectionUI connection)
		{
			if (connection.connectedNodeA.GetID() != id)
			{
				return connection.connectedNodeA;
			}
			return connection.connectedNodeB;
		}
		public static NodeUI GetNeighbourNode(ConnectionUI connection, NodeUI node)
		{
			if (connection.connectedNodeA.GetID() != node.GetID())
			{
				return connection.connectedNodeA;
			}
			return connection.connectedNodeB;
		}

		public List<ConnectionUI> GetActiveNeigbhourNodeList()
		{
			List<ConnectionUI> activeNeighbours = new();
			foreach (var conn in connections)
			{
				NodeUI neigbhour = GetNeighbourNode(conn);
				bool isEmpty = neigbhour.type == NodeType.Empty;

				if (neigbhour.active || isEmpty)
				{
					activeNeighbours.Add(conn);
				}
			}
			return activeNeighbours;
		}

		public void SetLocation(Vector2 location)
		{
			this.location = location;
		}

		public Vector2 GetLocation()
		{
			return location;
		}
		public bool HandleEmptyNodeCase()
		{
			if (!active && type == NodeType.Empty)
			{
				active = !active;
				ModContent.GetInstance<Runeforge>().Logger.Info("\tEMPTY NODE ACTIVE");
				node_image.SetImage(active_node_image);
				return true;
			}
			else if (active && type == NodeType.Empty)
			{
				ModContent.GetInstance<Runeforge>().Logger.Info("\tEMPTY NODE INACTIVE");
				active = !active;
				node_image.SetImage(inactive_node_image);
				return true;
			}
			return false;
		}
		

		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
			List<ConnectionUI> connections = GetActiveNeigbhourNodeList();
			bool isLeafNode = connections.Count == 1;

			ModContent.GetInstance<Runeforge>().Logger.Info("START");

			if (HandleEmptyNodeCase()) return;

			ModContent.GetInstance<Runeforge>().Logger.Info("PAST EMPTY NODE: " + connections.Count + " NODE ACTIVITY: " + active);
			if (!active && connections.Count >= 1)
			{
				ModContent.GetInstance<Runeforge>().Logger.Info("\tACTIVATE!");
				node_image.SetImage(active_node_image);

				foreach (var activeNeigbhourConn in connections)
				{
					activeNeigbhourConn.SetActive();
				}

				active = !active;
			}
			else
			{
				ModContent.GetInstance<Runeforge>().Logger.Info("\tMAYBE INACTIVATE!");
				ModContent.GetInstance<Runeforge>().Logger.Info("\t\tIS LEAFNODE: " + isLeafNode + " is deactivatable: " + CanNodeBeDeActivated(connections));
				if (active && (isLeafNode || CanNodeBeDeActivated(connections)))
				{
					ModContent.GetInstance<Runeforge>().Logger.Info("\t\tINACTIVATE!");
					active = !active;

					node_image.SetImage(inactive_node_image);
					foreach (var conn in connections)
					{
						conn.SetInActive();
					}
				}
			}
			Recalculate();
		}

		public override void OnInitialize()
		{
			base.OnInitialize();
			Width.Set(50, 0f);
			Height.Set(50, 0f);
			node_image.Width.Set(50, 0);
			node_image.Height.Set(50, 0);
			Append(node_image);
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mainPanel.isDragging)
			{
				mainPanel.isHoveringOverUI = true;
			}
		}
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mainPanel.isDragging)
			{
				mainPanel.isHoveringOverUI = false;
			}
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}

		public override int GetHashCode()
		{
			return node_image.GetHashCode();
		}

        public int GetID() { return id; }
        public NodeType GetNodeType() { return type; }
        public List<ConnectionUI> GetConnections() { return connections; }
        public ModifyPlayer GetModification() { return modification; }
        public void AddConnection(ConnectionUI conn) { connections.Add(conn); }
	}
}