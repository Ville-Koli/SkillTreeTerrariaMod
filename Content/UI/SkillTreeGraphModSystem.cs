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
			textureManager = new TextureManager();
			string pathToUIElements = @"Runeforge/Content/Assets/UIElements/";
			string pathToNodeAssets= @"Runeforge/Content/Assets/NodeAssets/";

			List<(List<ConnectionDirection> directions, string name)> connectionElements = new()
			{
				(new(){ConnectionDirection.UP, ConnectionDirection.DOWN}, "connection_vertical_long"),
				(new(){ConnectionDirection.RIGHT, ConnectionDirection.LEFT}, "connection_horizontal_long"),
				(new(){ConnectionDirection.DIAGONAL_BOTTOM_LEFT, ConnectionDirection.DIAGONAL_TOP_RIGHT}, "connection_diagonal_top"),
				(new(){ConnectionDirection.DIAGONAL_BOTTOM_RIGHT, ConnectionDirection.DIAGONAL_TOP_LEFT}, "connection_diagonal_bottom")
			};
			
			List<(NodeType type, string name)> nodeElements = new()
			{
				(NodeType.Empty, "emptynode"),
				(NodeType.Defence, "defencenode")
			};

			foreach (var uiElement in connectionElements)
			{
				(Asset<Texture2D> active, Asset<Texture2D> inactive) elementAsset = (
				ModContent.Request<Texture2D>($"{pathToUIElements}active_{uiElement.name}"),
				ModContent.Request<Texture2D>($"{pathToUIElements}inactive_{uiElement.name}"));
				ModContent.GetInstance<Runeforge>().Logger.Info($"Loading element: {pathToUIElements}active_{uiElement.name}");

				foreach (var direction in uiElement.directions)
				{
					textureManager.AddDirection(direction, elementAsset.active, elementAsset.inactive);
					ModContent.GetInstance<Runeforge>().Logger.Info($"\tDirection: {direction}");
				}
			}

			foreach (var uiElement in nodeElements)
			{
				(Asset<Texture2D> active, Asset<Texture2D> inactive) elementAsset = (
				ModContent.Request<Texture2D>($"{pathToNodeAssets}active_{uiElement.name}"),
				ModContent.Request<Texture2D>($"{pathToNodeAssets}inactive_{uiElement.name}"));
				ModContent.GetInstance<Runeforge>().Logger.Info($"Loading element: {pathToNodeAssets}{uiElement.name}");

				textureManager.AddNode(uiElement.type, elementAsset.active, elementAsset.inactive);
			}

			Asset<Texture2D> levelBar = ModContent.Request<Texture2D>($"{pathToUIElements}level_bar");

			(Asset<Texture2D> active, Asset<Texture2D> inactive) hoverOver = (
			ModContent.Request<Texture2D>($"{pathToUIElements}active_hoverover_text_box"),
			ModContent.Request<Texture2D>($"{pathToUIElements}inactive_hoverover_text_box"));
			textureManager.transparent_box = ModContent.Request<Texture2D>($"{pathToUIElements}transparent_hoverover_text_box");

			textureManager.AddUI(UIType.hoverOver, hoverOver.active, hoverOver.inactive);
			textureManager.AddUI(UIType.levelBar, levelBar, levelBar); // level bar is always active
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