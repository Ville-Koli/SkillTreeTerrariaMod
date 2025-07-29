using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Terraria.GameInput;
using Terraria.GameContent.UI.Elements;
using System;

namespace Runeforge.Content.UI
{
	public class SkillTreePanel : UIElement
	{
		public Color BackgroundColor;
		private UIPanel _uiPanel;
		private NodeManager nodeManager;
		public ConnectionManager connectionManager;
		public HoverOverUI hoverOverUI;
		private Vector2 offset;
		public static Vector2 panOffset;
		public static float zoom = 1;
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
		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			Vector2 mousePosBefore = (Main.MouseScreen - panOffset) / zoom;
			zoom = MathF.Max(MathF.Min(zoom + (evt.ScrollWheelValue > 0 ? 0.1f : -0.1f), 2), 0.1f);
			ModContent.GetInstance<Runeforge>().Logger.Info("Zoom: " + zoom);
			Vector2 mouseScreenPosAfter = mousePosBefore * zoom + panOffset;
			Vector2 offsetDelta = Main.MouseScreen - mouseScreenPosAfter;
			panOffset += offsetDelta;
        }
		public void SetHoverOverUI(HoverOverUI hoverOverUI)
		{
			this.hoverOverUI = hoverOverUI;
		}
		public void SetConnectionManager(ConnectionManager connectionManager)
		{
			this.connectionManager = connectionManager;
		}
		public void SetNodeManager(NodeManager nodeManager)
		{
			this.nodeManager = nodeManager;
		}
		public override void LeftMouseUp(UIMouseEvent evt)
		{
			base.LeftMouseUp(evt);
			if (!isHoveringOverUI && isDragging)
			{
				isDragging = false;
				Vector2 mouseDelta = MouseDeltaVector();
				panOffset += mouseDelta;
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
				Dictionary<int, NodeUI> nodeContainer = nodeManager.GetNodes();
				foreach (var pair in nodeContainer)
				{
					NodeUI nodeUI = pair.Value;
					nodeUI.location = nodeUI.basePosition * zoom + panOffset + delta; ;
					nodeUI.Left.Set(nodeUI.location .X, 0.0f);
					nodeUI.Top.Set(nodeUI.location .Y, 0.0f);
				}
				Dictionary<int, ConnectionUI> connectionContainer = connectionManager.GetConnections();
				foreach (var pair in connectionContainer)
				{
					ConnectionUI conn = pair.Value;
					conn.location = conn.basePosition * zoom + panOffset + delta;
					conn.Left.Set(conn.location.X, 0.0f);
					conn.Top.Set(conn.location.Y, 0.0f);
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
			if (IsMouseHovering)
			{
				PlayerInput.LockVanillaMouseScroll("Runeforge/ScrollListA"); // The passed in string can be anything.
			}
			// Otherwise, we can check a child element instead
			if (IsMouseHovering)
			{
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
			LevelUI levelUI = new LevelUI();
			levelUI.levelBarHolder = SkillTreeGraphModSystem.textureManager.GetUI(SkillTree.UIType.levelBar).active;
			_uiPanel.Append(levelUI);
			Dictionary<int, ConnectionUI> connectionContainer = connectionManager.GetConnections();
			foreach (var pair in connectionContainer)
			{
				ConnectionUI conn = pair.Value;
				conn.Left.Set(conn.GetLocation().X, 0.0f);
				conn.Top.Set(conn.GetLocation().Y, 0.0f);
				_uiPanel.Append(conn);
			}
			Dictionary<int, NodeUI> nodeContainer = nodeManager.GetNodes();
			foreach (var pair in nodeContainer)
			{
				NodeUI nodeui = pair.Value;
				ModContent.GetInstance<Runeforge>().Logger.Info("NODE ID: " + nodeui.GetID() + " locations: " + nodeui.GetLocation().X + " , " + nodeui.GetLocation().Y);
				nodeui.Left.Set(nodeui.GetLocation().X, 0.0f);
				nodeui.Top.Set(nodeui.GetLocation().Y, 0.0f);
				//ModContent.GetInstance<Runeforge>().Logger.Info("NODE STATUS: " + nodeui);
				_uiPanel.Append(nodeui);
			}
			_uiPanel.Append(hoverOverUI);
			Recalculate();
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			ApplyDragging();
			hoverOverUI.Left.Set(Main.MouseScreen.X, 0.0f);
			hoverOverUI.Top.Set(Main.MouseScreen.Y, 0.0f);
		}	
	}
}