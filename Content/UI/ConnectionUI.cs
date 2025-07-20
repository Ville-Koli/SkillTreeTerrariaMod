using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria.ModLoader;

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
			connectedNodeA = connectedA;
			connectedNodeB = connectedB;
			this.direction_from_node = direction_from_node;
			id = global_id;
			global_id++;
		}

        public override string ToString()
        {
            return $"({connectedNodeA.GetID()}, {connectedNodeB.GetID()}, {active})";
        }
		public int GetID()
		{
			return id;
		}

		public void SetActive()
		{
			connection_image.SetImage(active_connection_image);
			active = true;
		}
		public void SetInActive()
		{
			connection_image.SetImage(inactive_connection_image);
			active = false;
		}

		public void SetLocation(Vector2 location)
		{
			this.location = location;
		}

		public Vector2 GetLocation()
		{
			return location;
		}

		public override void OnInitialize()
		{
			base.OnInitialize();
			Width.Set(50, 0f);
			Height.Set(50, 0f);
			connection_image.Width.Set(50, 0);
			connection_image.Height.Set(50, 0);
			Append(connection_image);
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
			return connection_image.GetHashCode();
        }
	}
}