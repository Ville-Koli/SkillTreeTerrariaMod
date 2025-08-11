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
		internal SkillTreeUIState SkillTreeUI;
		internal CharacterScreenState CharacterScreen;
		private bool ShowSkillTreeUI = false;
		private bool ShowCharacterScreenUI = false;
		public static TextureManager textureManager = new();
		public override void Load()
		{
			if (!Main.dedServ)
			{
				textureManager = new TextureManager();
				MyInterface = new UserInterface();
				SkillTreeUI = new SkillTreeUIState();
				CharacterScreen = new();
				SkillTreeUI.Activate(); // Activate calls Initialize() on the UIState if not initialized and calls OnActivate, then calls Activate on every child element.
				CharacterScreen.Activate();
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
			SkillTreeUI = null;
		}
		internal void ShowSkillTree()
		{
			MyInterface?.SetState(SkillTreeUI);
			ShowCharacterScreenUI = false;
		}

		internal void HideSkillTree()
		{
			MyInterface?.SetState(null);
		}
		internal void ShowCharacterScreen()
		{
			MyInterface?.SetState(CharacterScreen);
			ShowSkillTreeUI = false;
		}

		internal void HideCharacterScreen()
		{
			MyInterface?.SetState(null);
		}
		public override void PostUpdateInput()
		{
			if (Runeforge.ToggleMyUIKeybind != null && Runeforge.ToggleMyUIKeybind.JustPressed)
			{
				ShowSkillTreeUI = !ShowSkillTreeUI;
				if (ShowSkillTreeUI)
				{
					ShowSkillTree();
					Mod.Logger.Info("Show UI.");
				}
				else
				{
					HideSkillTree();
					Mod.Logger.Info("Stop showing UI.");
				}
			} else if (Runeforge.ToggleCharacterStatScreen != null && Runeforge.ToggleCharacterStatScreen.JustPressed) {
				ShowCharacterScreenUI = !ShowCharacterScreenUI;
				if (ShowCharacterScreenUI)
				{
					ShowCharacterScreen();
					Mod.Logger.Info("Show UI.");
				}
				else
				{
					HideCharacterScreen();
					Mod.Logger.Info("Stop showing UI.");
				}				
			}
		}
	}
}