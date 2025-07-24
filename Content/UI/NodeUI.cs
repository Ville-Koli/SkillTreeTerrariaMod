using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using System;
using Runeforge.Content.SkillTree.NodeScripts;
using System.Text;
using Terraria.GameContent;

namespace Runeforge.Content.UI
{
	public class NodeUI : UIElement
	{
		private SkillTreePanel mainPanel;
		private List<ConnectionUI> connections = new();
		private NodeType type;
		private INodeTrigger trigger;
		public UIImage node_image;
		public Asset<Texture2D> active_node_image;
		public Asset<Texture2D> inactive_node_image;
		private StatBlock statBlock;
		public HoverOverUI hoverOverUI;
		public Vector2 location;
		public Vector2 basePosition;
		public bool active;
		private string description;
		private int id;
		private static int global_id = 0;
		public NodeUI(SkillTreePanel panel, Asset<Texture2D> inactive, Asset<Texture2D> active, Vector2 location, INodeTrigger trigger, NodeType type, HoverOverUI hoverOverUI, StatBlock statBlock, string description)
		{
			mainPanel = panel;
			node_image = new UIImage(inactive);
			inactive_node_image = inactive;
			this.location = location;
			this.basePosition = location;
			this.trigger = trigger;
			this.type = type;
			this.hoverOverUI = hoverOverUI;
			this.description = description;
			this.statBlock = statBlock;
			active_node_image = active;
			id = global_id;
			global_id++;
		}
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(active ? active_node_image.Value : inactive_node_image.Value, node_image.GetDimensions().Position(), null, Color.White, 0f, Vector2.Zero, SkillTreePanel.zoom, SpriteEffects.None, 0f);
        }
		public override string ToString()
		{
			//                                             NodeID | Type   | Connections                     | Description
			// to string method generates a string such as NodeID | TypeID | {(NodeID, NodeID, active), ...} | description
			StringBuilder nodeString = new StringBuilder();
			nodeString.Append($"{id} | {type} | ");
			nodeString.Append('{');
			foreach (var conn in connections)
			{
				nodeString.Append(conn);
				nodeString.Append(',');
			}
			nodeString.Append("} | ");
			nodeString.Append($"{description}");
			return nodeString.ToString();
		}
		public bool GetNodeActivity()
		{
			return active;
		}
		public StatBlock GetStatBlock()
		{
			return statBlock;
		}
		public void SetStatBlock(StatBlock statBlock)
		{
			this.statBlock = statBlock;
		}
		public string GetDescription()
		{
			return description;
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
				deactivation = deactivation && PathingAlgorithms.DoesPossiblePathExistToEmpty(node, new());
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
		public void SetActive()
		{
			node_image.SetImage(active_node_image);
			active = true;
		}

		public void SetInActive()
		{
			node_image.SetImage(inactive_node_image);
			active = false;
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
				trigger.Activate(statBlock);
			}
			else
			{
				ModContent.GetInstance<Runeforge>().Logger.Info("\tMAYBE INACTIVATE!");
				ModContent.GetInstance<Runeforge>().Logger.Info("\t\tIS LEAFNODE: " + isLeafNode + " is deactivatable: " + CanNodeBeDeActivated(connections));
				if (active && (isLeafNode || CanNodeBeDeActivated(connections)))
				{
					active = !active;

					node_image.SetImage(inactive_node_image);
					foreach (var conn in connections)
					{
						conn.SetInActive();
					}
					trigger.DeActivate(statBlock);
				}
			}
			EditHoverOverElement();
			Recalculate();
		}

		public override void OnInitialize()
		{
			base.OnInitialize();
			node_image.Width.Set(node_image.Width.Pixels, 0);
			node_image.Height.Set(node_image.Height.Pixels, 0);
			Width.Set(node_image.Width.Pixels, 0f);
			Height.Set(node_image.Height.Pixels, 0f);
			Append(node_image);
		}


		public void EditHoverOverElement()
		{
			hoverOverUI.ChangeDescription(description);
			if (active)
			{
				hoverOverUI.SetActive();
			}
			else
			{
				hoverOverUI.SetInActive();
			}
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mainPanel.isDragging)
			{
				mainPanel.isHoveringOverUI = true;
			}
			if (type != NodeType.Empty)
			{
				EditHoverOverElement();
			}
		}
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mainPanel.isDragging)
			{
				mainPanel.isHoveringOverUI = false;
			}
			hoverOverUI.SetTransparent();
			hoverOverUI.ChangeDescription("");
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Width.Set(node_image.Width.Pixels * SkillTreePanel.zoom, 0f);
			Height.Set(node_image.Height.Pixels * SkillTreePanel.zoom, 0f);
			location = basePosition * SkillTreePanel.zoom + SkillTreePanel.panOffset;
			Top.Set(location.Y, 0f);
			Left.Set(location.X, 0f);
			Recalculate();
		}

		public override int GetHashCode()
		{
			return node_image.GetHashCode();
		}

        public int GetID() { return id; }
        public NodeType GetNodeType() { return type; }
        public List<ConnectionUI> GetConnections() { return connections; }
        public INodeTrigger GetModification() { return trigger; }
        public void AddConnection(ConnectionUI conn) { connections.Add(conn); }
	}
}