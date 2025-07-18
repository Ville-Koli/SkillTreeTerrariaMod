using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;
using Runeforge.Content.SkillTree;
using ReLogic.Content;

namespace Runeforge.Content.UI
{
	[Autoload(Side = ModSide.Client)]
	public class SkillTreeGraphUI : ModSystem
	{
		internal UserInterface MyInterface;
		internal TheUI MyUI;
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
				MyUI = new TheUI();
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
			if (Runeforge.ToggleMyUIKeybind.JustPressed)
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