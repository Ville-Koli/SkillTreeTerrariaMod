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

		public void CreateTextureManager()
		{
			textureManager = new TextureManager();
			string pathToUIElements = @"Runeforge/Content/Assets/UIAssets/";
			string pathToNodeAssets = @"Runeforge/Content/Assets/NodeAssets/";
			string pathToStatAssets = @"Runeforge/Content/Assets/StatAssets/";

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
				(NodeType.Defence, "defencenode"),
				(NodeType.MeleeDamage, "meleedamagenode"),
				(NodeType.RangedDamage, "rangeddamagenode"),
				(NodeType.BulletDamage, "bulletdamagenode"),
				(NodeType.SummonDamage, "summondamagenode"),
				(NodeType.MeleeAttackSpeed, "meleeattackspeednode"),
				(NodeType.RangedAttackSpeed, "rangedattackspeednode"),
				(NodeType.ProjectileCount, "extraprojectilesnode"),
				(NodeType.MaxHealth, "maxhealthnode"),
				(NodeType.MaxMana, "maxmananode"),
				(NodeType.HealthRegen, "healthregennode"),
				(NodeType.CriticalHitChance, "critchancenode"),
				(NodeType.CriticalHitDamage, "critdamagenode")
			};

			List<(NodeType type, string name)> statElements = new()
			{
				(NodeType.Defence, "stat_icon_defence"),
				(NodeType.MeleeDamage, "stat_icon_melee_damage"),
				(NodeType.LifeSteal, "stat_icon_lifesteal"),
				(NodeType.RangedDamage, "stat_icon_range_damage"),
				(NodeType.SummonDamage, "stat_icon_summoner_damage"),
				(NodeType.BulletDamage, "stat_icon_bulletdamage"),
				(NodeType.MeleeAttackSpeed, "stat_icon_melee_attackspeed"),
				(NodeType.RangedAttackSpeed, "stat_icon_ranged_attackspeed"),
				(NodeType.CriticalHitChance, "stat_icon_critchance"),
				(NodeType.CriticalHitDamage, "stat_icon_critdamage"),
				(NodeType.MaxHealth, "stat_icon_maxhealth"),
				(NodeType.HealthRegen, "stat_icon_healthregen"),
				(NodeType.MaxMana, "stat_icon_maxmana")
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

			foreach (var uiElement in statElements)
			{
				Asset<Texture2D> elementAsset = ModContent.Request<Texture2D>($"{pathToStatAssets}{uiElement.name}");
				ModContent.GetInstance<Runeforge>().Logger.Info($"Loading element: {pathToStatAssets}{uiElement.name}");
				textureManager.AddStat(uiElement.type, elementAsset);
			}
			Asset<Texture2D> levelBar = ModContent.Request<Texture2D>($"{pathToUIElements}level_bar");
			Asset<Texture2D> skillPointOrb = ModContent.Request<Texture2D>($"{pathToUIElements}skillpoint_orb");

			(Asset<Texture2D> active, Asset<Texture2D> inactive) hoverOver = (
			ModContent.Request<Texture2D>($"{pathToUIElements}active_hoverover_text_box"),
			ModContent.Request<Texture2D>($"{pathToUIElements}inactive_hoverover_text_box"));
			textureManager.transparent_box = ModContent.Request<Texture2D>($"{pathToUIElements}transparent_hoverover_text_box");

			textureManager.AddUI(UIType.HoverOver, hoverOver.active, hoverOver.inactive);
			textureManager.AddUI(UIType.LevelBar, levelBar, levelBar); // level bar is always active
			textureManager.AddUI(UIType.SkillPointOrb, skillPointOrb, skillPointOrb); // level bar is always active
		}
		public override void Load()
		{
			if (!Main.dedServ)
			{
				CreateTextureManager();
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