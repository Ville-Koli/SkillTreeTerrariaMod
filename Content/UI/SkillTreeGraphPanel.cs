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
	public class SkillTreePanel : UIElement
	{
		public Color BackgroundColor;
		private UIPanel _uiPanel;
		private Dictionary<int, NodeUI> node_list = new();
		public List<NodeUI> nodes = new();
		public Dictionary<int, ConnectionUI> conns = new();
		private Vector2 offset;
		private UIText debugText;
		public bool isDragging;
		public bool isHoveringOverUI;
		public override void LeftMouseDown(UIMouseEvent evt)
		{
			base.LeftMouseDown(evt);
			if (!isHoveringOverUI)
			{
				isDragging = true;	
			}
			offset = evt.MousePosition;

		}

		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);
			if (!isHoveringOverUI && isDragging)
			{
				isDragging = false;
				Vector2 mouseDelta = MouseDeltaVector();
				foreach (var pair in node_list)
				{
					NodeUI node = node_list[pair.Key];
					node.SetLocation(pair.Value.GetLocation() + mouseDelta);
				}
				foreach (var pair in conns)
				{
					ConnectionUI conn = pair.Value;
					conn.SetLocation(conn.GetLocation() + mouseDelta);
				}	
			}
		}

		public Vector2 MouseDeltaVector()
		{
			Vector2 mousePos = Main.MouseScreen;
			float xdif = mousePos.X - offset.X;
			float ydif = mousePos.Y - offset.Y;
			return new Vector2(xdif, ydif);
		}
		public void ApplyDragging()
		{
			if (isDragging && !isHoveringOverUI)
			{
				Vector2 delta = MouseDeltaVector();
				foreach (var pair in node_list)
				{
					NodeUI nodeUI = pair.Value;
					nodeUI.Left.Set(delta.X + nodeUI.GetLocation().X, 0.0f);
					nodeUI.Top.Set(delta.Y + nodeUI.GetLocation().Y, 0.0f);
				}
				foreach (var pair in conns)
				{
					ConnectionUI conn = pair.Value;
					conn.Left.Set(delta.X + conn.GetLocation().X, 0.0f);
					conn.Top.Set(delta.Y + conn.GetLocation().Y, 0.0f);
				}
				Recalculate();
			}
		}
		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			// If this code is in the panel or container element, check it directly
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			// Otherwise, we can check a child element instead
			if (ContainsPoint(Main.MouseScreen))
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			// If this code is in the scrollable element, check it directly
			if (IsMouseHovering) {
				PlayerInput.LockVanillaMouseScroll("Runeforge/ScrollListA"); // The passed in string can be anything.
			}
			// Otherwise, we can check a child element instead
			if (IsMouseHovering) {
				PlayerInput.LockVanillaMouseScroll("Runeforge/ScrollListB"); // The passed in string can be anything.
			}
		}
		public SkillTreePanel() : base()
		{

		}
		public UIPanel GetPanel()
		{
			return _uiPanel;
		}
		public override void OnInitialize()
		{
			_uiPanel = new UIPanel();
			_uiPanel.Width = StyleDimension.Fill;
			_uiPanel.Height = StyleDimension.Fill;
			_uiPanel.BackgroundColor = BackgroundColor;
			Append(_uiPanel);
			debugText = new UIText("hello");
			_uiPanel.Append(debugText);
			foreach (var n in nodes)
			{
				NodeUI nodeui = n;
				ModContent.GetInstance<Runeforge>().Logger.Info("NODE ID: " + n.GetID()  + " locations: " + nodeui.GetLocation().X + " , " + nodeui.GetLocation().Y);
				node_list.Add(n.GetID(), n);
				nodeui.Left.Set(nodeui.GetLocation().X, 0.0f);
				nodeui.Top.Set(nodeui.GetLocation().Y, 0.0f);
				foreach (var conn in nodeui.GetConnections())
				{
					if (!conns.ContainsKey(conn.GetID()))
					{
						conns.Add(conn.GetID(), conn);
						conn.Left.Set(conn.GetLocation().X, 0.0f);
						conn.Top.Set(conn.GetLocation().Y, 0.0f);
						_uiPanel.Append(conn);
					}
				}
				_uiPanel.Append(nodeui);
			}
			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			ApplyDragging();
		}	
	}
}