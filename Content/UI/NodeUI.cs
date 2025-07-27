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
		public Vector2 location; // current location
		public Vector2 basePosition; // starting position
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
		/**
		<summary> 
		<para>Function that draws the node's image on the panel as active or not depending on activity </para>
		</summary>
		**/
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(active ? active_node_image.Value : inactive_node_image.Value, node_image.GetDimensions().Position(), null, Color.White, 0f, Vector2.Zero, SkillTreePanel.zoom, SpriteEffects.None, 0);
		}
		/**
		<summary> 
		<para>Function, which handles the empty node edge case </para>
		</summary>
		<returns>returns a boolean depending on whether node interacted with was type empty or not</returns>
		**/
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
		/**
		<summary> 
		<para>Function, which handles left click interaction </para>
		</summary>
		**/
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
			List<ConnectionUI> connections = GetActiveNeigbhourNodeList();
			bool isLeafNode = connections.Count == 1; // node is a leaf node only and only if the amount of active connections are one

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
			EditHoverOverElement(); // edit hover over element depending on whether node got activated or deactivated
			Recalculate();
		}
		/**
		<summary> 
		<para>Function, which initializes the node </para>
		</summary>
		**/
		public override void OnInitialize()
		{
			base.OnInitialize();
			node_image.Width.Set(node_image.Width.Pixels, 0);
			node_image.Height.Set(node_image.Height.Pixels, 0);
			Width.Set(node_image.Width.Pixels, 0f);
			Height.Set(node_image.Height.Pixels, 0f);
			Append(node_image);
		}
		/**
		<summary> 
		<para>Function, which handles entering the element </para>
		</summary>
		**/
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
		/**
		<summary> 
		<para>Function, which handles leaving the element </para>
		</summary>
		**/
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
		/**
		<summary> 
		<para>Function, which is ran every in-game tick when panel is open</para>
		</summary>
		**/
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			// should relocate this to a more optimal location
			// this just applies scale of the UIImage hitbox and location
			Width.Set(node_image.Width.Pixels * SkillTreePanel.zoom, 0f);
			Height.Set(node_image.Height.Pixels * SkillTreePanel.zoom, 0f);
			location = basePosition * SkillTreePanel.zoom + SkillTreePanel.panOffset;
			Top.Set(location.Y, 0f);
			Left.Set(location.X, 0f);
			Recalculate();
		}

		/**
		<summary> 
		<para>Function, which calculates a boolean on whether node can be deactivated without breaking the
		active graph.</para>
		</summary>
		<param name="activeNeighbours">list of neighbouring elements deduced from connections that are active</param>
		<returns> a boolean whether node can be deactivated or not </returns>
		**/
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

		/**
		<summary> 
		<para>Function, which returns non-self node from connection</para>
		</summary>
		<param name="connection">a connection to be analyzed</param>
		<returns> a neigbhouring node </returns>
		**/
		public NodeUI GetNeighbourNode(ConnectionUI connection)
		{
			if (connection.connectedNodeA.GetID() != id)
			{
				return connection.connectedNodeA;
			}
			return connection.connectedNodeB;
		}
		/**
		<summary> 
		<para>Function, which returns non-self node from connection</para>
		</summary>
		<param name="connection">a connection to be analyzed</param>
		<param name="node">current node</param>
		<returns> a neigbhouring node </returns>
		**/
		public static NodeUI GetNeighbourNode(ConnectionUI connection, NodeUI node)
		{
			if (connection.connectedNodeA.GetID() != node.GetID())
			{
				return connection.connectedNodeA;
			}
			return connection.connectedNodeB;
		}
		/**
		<summary> 
		<para>Function, which returns an list of active neighbouring nodes</para>
		</summary>
		<returns> a list of active neighbouring nodes </returns>
		**/
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

		/**
		<summary> 
		<para>Function, which sets the location of then node</para>
		</summary>
		<returns> a list of active neighbouring nodes </returns>
		**/
		public void SetLocation(Vector2 location)
		{
			this.location = location;
		}

		/**
		<summary> 
		<para>Function, which returns the location of the node</para>
		</summary>
		<returns> location of the node </returns>
		**/
		public Vector2 GetLocation()
		{
			return location;
		}
		/**
		<summary> 
		<para>Function, which sets the node to be active</para>
		</summary>
		**/
		public void SetActive()
		{
			node_image.SetImage(active_node_image);
			active = true;
		}
		/**
		<summary> 
		<para>Function, which sets the node to be inactive</para>
		</summary>
		**/
		public void SetInActive()
		{
			node_image.SetImage(inactive_node_image);
			active = false;
		}
		/**
		<summary> 
		<para>Function, which edits the hover over element to display right information</para>
		</summary>
		**/
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

		public override int GetHashCode()
		{
			return node_image.GetHashCode();
		}
		public override string ToString()
		{
			//                                              Type   | Trigger Element| Description
			// to string method generates a string such as  2      | 1              | "does something"
			StringBuilder nodeString = new StringBuilder();
			nodeString.Append($"{type} | ");
			nodeString.Append($"{trigger.ReturnTriggerElement()} | {description}");
			return nodeString.ToString();
		}
		/**
		<summary> 
		<para>Function, which gets node activity</para>
		</summary>
		**/
		public bool GetNodeActivity()
		{
			return active;
		}
		/**
		<summary> 
		<para>Function, which gets node's linked statblock</para>
		</summary>
		**/
		public StatBlock GetStatBlock()
		{
			return statBlock;
		}
		/**
		<summary> 
		<para>Function, which sets node's linked statblock</para>
		</summary>
		<param name="statBlock"> statblock to be set to this node </param>
		**/
		public void SetStatBlock(StatBlock statBlock)
		{
			this.statBlock = statBlock;
		}
		/**
		<summary> 
		<para>Function, which returns the description of the node</para>
		</summary>
		<returns> a string, which describes the functionality of the node </returns>
		**/
		public string GetDescription()
		{
			return description;
		}
		/**
		<summary> 
		<para>Function, which returns the id of the node</para>
		</summary>
		<returns> unique id of the node </returns>
		**/
		public int GetID()
		{
			return id;
		}
		/**
		<summary> 
		<para>Function, which returns the type of the node</para>
		</summary>
		<returns> returns the type of the node </returns>
		**/
		public NodeType GetNodeType()
		{
			return type;
		}
		/**
		<summary> 
		<para>Function, which returns the list of connections of the node</para>
		</summary>
		<returns> returns the list of connections </returns>
		**/
		public List<ConnectionUI> GetConnections()
		{
			return connections;
		}
		/**
		<summary> 
		<para>Function, which returns the trigger of the node</para>
		</summary>
		<returns> returns node's trigger </returns>
		**/
		public INodeTrigger GetModification()
		{
			return trigger;
		}
		/**
		<summary> 
		<para>Function, which adds a connectio to the node</para>
		</summary>
		<param name="conn"> connection to be added to list of connections of this node</param>
		**/
		public void AddConnection(ConnectionUI conn)
		{
			connections.Add(conn);
		}
	}
}