using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria.ModLoader.IO;

namespace Runeforge.Content.UI
{
	[Autoload(Side = ModSide.Client)]
	public class SkillTreeGraphModSystem : ModSystem
	{
		internal UserInterface MyInterface;
		internal SkillTreeUIState MyUI;
		private bool ShowUI = false;
		public static TextureManager textureManager = new();

		public void CreateTextureManager()
		{
			(Asset<Texture2D> active, Asset<Texture2D> inactive) vertical = (
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/active_connection_vertical_long"),
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/inactive_connection_vertical_long"));

			(Asset<Texture2D> active, Asset<Texture2D> inactive) horizontal = (
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/active_connection_horizontal_long"),
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/inactive_connection_horizontal_long"));

			(Asset<Texture2D> active, Asset<Texture2D> inactive) diagonalTop = (
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/active_connection_diagonal_top"),
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/inactive_connection_diagonal_top"));

			(Asset<Texture2D> active, Asset<Texture2D> inactive) diagonalBottom = (
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/active_connection_diagonal_bottom"),
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/inactive_connection_diagonal_bottom"));

			(Asset<Texture2D> active, Asset<Texture2D> inactive) hoverOver = (
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/active_hoverover_text_box"),
			ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/inactive_hoverover_text_box"));


			Asset<Texture2D> emptyNode = ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/EmptyNode");
			Asset<Texture2D> activeEmptyNode = ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/Active_EmptyNode");

			Asset<Texture2D> tankNodeInActive = ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/inactive_defencenode");
			Asset<Texture2D> tankNodeActive = ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/active_defencenode");
			textureManager = new TextureManager();
			
			textureManager.transparent_box = ModContent.Request<Texture2D>("Runeforge/Content/SkillTree/NodeAssets/transparent_hoverover_text_box");

			textureManager.AddDirection(ConnectionDirection.UP, vertical.active, vertical.inactive);
			textureManager.AddDirection(ConnectionDirection.DOWN, vertical.active, vertical.inactive);
			textureManager.AddDirection(ConnectionDirection.RIGHT, horizontal.active, horizontal.inactive);
			textureManager.AddDirection(ConnectionDirection.LEFT, horizontal.active, horizontal.inactive);
			textureManager.AddDirection(ConnectionDirection.DIAGONAL_BOTTOM_LEFT, diagonalTop.active, diagonalTop.inactive);
			textureManager.AddDirection(ConnectionDirection.DIAGONAL_BOTTOM_RIGHT, diagonalBottom.active, diagonalBottom.inactive);
			textureManager.AddDirection(ConnectionDirection.DIAGONAL_TOP_RIGHT, diagonalTop.active, diagonalTop.inactive);
			textureManager.AddDirection(ConnectionDirection.DIAGONAL_TOP_LEFT, diagonalBottom.active, diagonalBottom.inactive);

			textureManager.AddNode(NodeType.Empty, activeEmptyNode, emptyNode);
			textureManager.AddNode(NodeType.Defence, tankNodeActive, tankNodeInActive);

			textureManager.AddUI(UIType.hoverOver, hoverOver.active, hoverOver.inactive);
		}
		public override void Load()
		{
			if (!Main.dedServ)
			{
				CreateTextureManager();
				MyInterface = new UserInterface();
				MyUI = new SkillTreeUIState();
				MyUI.Activate(); // Activate calls Initialize() on the UIState if not initialized and calls OnActivate, then calls Activate on every child element.
			}
		}
		private GameTime _lastUpdateUiGameTime;

		public override void UpdateUI(GameTime gameTime)
		{
			_lastUpdateUiGameTime = gameTime;
			if (MyInterface?.CurrentState != null)
			{
				MyInterface.Update(gameTime);
			}
		}
		public override void OnWorldUnload()
		{
			base.OnWorldUnload();
			NodeManager.DeActivateAll();
			ConnectionManager.DeActivateAll();
        }
		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"Runeforge: Skilltree",
					delegate
					{
						if (_lastUpdateUiGameTime != null && MyInterface?.CurrentState != null)
						{
							MyInterface.Draw(Main.spriteBatch, _lastUpdateUiGameTime);
						}
						return true;
					},
					InterfaceScaleType.UI));
			}
		}

		public override void Unload()
		{
			//MyUI?.SomeKindOfUnload(); // If you hold data that needs to be unloaded, call it in OO-fashion
			MyUI = null;
		}
		internal void ShowMyUI()
		{
			MyInterface?.SetState(MyUI);
		}

		internal void HideMyUI()
		{
			MyInterface?.SetState(null);
		}
		public override void PostUpdateInput()
		{
			if (Runeforge.ToggleMyUIKeybind != null && Runeforge.ToggleMyUIKeybind.JustPressed)
			{
				ShowUI = !ShowUI;
				if (ShowUI)
				{
					ShowMyUI();
					Mod.Logger.Info("Show UI.");
				}
				else
				{
					HideMyUI();
					Mod.Logger.Info("Stop showing UI.");
				}
			}
		}
	}
}