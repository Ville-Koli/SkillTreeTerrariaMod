using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria.ModLoader;
using Terraria;
using Terraria.GameContent;

namespace Runeforge.Content.UI
{
	public class ConnectionUI : UIElement
	{
		private SkillTreePanel mainPanel;
		public UIImage connection_image;
		public NodeUI connectedNodeA;
		public NodeUI connectedNodeB;
		public ConnectionDirection direction_from_node;
		public Asset<Texture2D> active_connection_image;
		public Asset<Texture2D> inactive_connection_image;
		public Vector2 location;
		public Vector2 basePosition;
		public bool active;
		private int id;
		private static int global_id = 0;
		public ConnectionUI(SkillTreePanel panel, Asset<Texture2D> inactive, Asset<Texture2D> active, NodeUI connectedA, NodeUI connectedB, ConnectionDirection direction_from_node, Vector2 location)
		{
			mainPanel = panel;
			connection_image = new UIImage(inactive);
			inactive_connection_image = inactive;
			active_connection_image = active;
			this.location = location;
			this.basePosition = location;
			connectedNodeA = connectedA;
			connectedNodeB = connectedB;
			this.direction_from_node = direction_from_node;
			id = global_id;
			global_id++;
		}

		/**
		<summary> 
		<para>Function that draws the connections image on the panel as active or not depending on activity </para>
		</summary>
		**/
		public override void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(active ? active_connection_image.Value : inactive_connection_image.Value, connection_image.GetDimensions().Position(), null, Color.White, 0f, Vector2.Zero, SkillTreePanel.zoom, SpriteEffects.None, 0f);
		}

		/**
		<summary> 
		<para>Function which initializes the connection node</para>
		</summary>
		**/
		public override void OnInitialize()
		{
			base.OnInitialize();
			connection_image.Width.Set(connection_image.Width.Pixels, 0);
			connection_image.Height.Set(connection_image.Height.Pixels, 0);
			Width.Set(connection_image.Width.Pixels, 0f);
			Height.Set(connection_image.Height.Pixels, 0f);
			Append(connection_image);
		}

		/**
		<summary> 
		<para>Event for mouse entering the connection. Used for disabling dragging</para>
		</summary>
		**/
		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mainPanel.isDragging)
			{
				mainPanel.isHoveringOverUI = true;
			}
		}

		/**
		<summary> 
		<para>Event for mouse leaving the connection.</para>
		</summary>
		**/
		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			if (!mainPanel.isDragging)
			{
				mainPanel.isHoveringOverUI = false;
			}
		}

		/**
		<summary> 
		<para>Function, which is ran every in-game tick when panel is open</para>
		</summary>
		**/
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			Width.Set(connection_image.Width.Pixels * SkillTreePanel.zoom, 0f);
			Height.Set(connection_image.Height.Pixels * SkillTreePanel.zoom, 0f);
			location = basePosition * SkillTreePanel.zoom + SkillTreePanel.panOffset;
			Vector2 newLocation = location;
			Top.Set(newLocation.Y, 0f);
			Left.Set(newLocation.X, 0f);
			Recalculate();
		}

		public override int GetHashCode()
		{
			return connection_image.GetHashCode();
		}
		/**
		<summary> 
		<para>Function that gets the id</para>
		</summary>
		<returns> the id of the connection </returns>
		**/
		public int GetID()
		{
			return id;
		}
		/**
		<summary> 
		<para>Function that sets the connection to be active</para>
		</summary>
		**/
		public void SetActive()
		{
			connection_image.SetImage(active_connection_image);
			active = true;
		}
		/**
		<summary> 
		<para>Function that sets the connection to be inactive</para>
		</summary>
		**/
		public void SetInActive()
		{
			connection_image.SetImage(inactive_connection_image);
			active = false;
		}
		/**
		<summary> 
		<para>Function that sets the connection's location </para>
		</summary>
		**/
		public void SetLocation(Vector2 location)
		{
			this.location = location;
		}
		/**
		<summary> 
		<para>Function that gets the connections location </para>
		</summary>
		<returns>get's location of the connection</returns>
		**/
		public Vector2 GetLocation()
		{
			return location;
		}
		public override string ToString()
		{
			return $"({connectedNodeA.GetID()}, {connectedNodeB.GetID()}, {active})";
		}
	}
}