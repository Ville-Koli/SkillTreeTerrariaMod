using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Runeforge.Content.SkillTree;
using ReLogic.Content;
using Terraria.ModLoader;

namespace Runeforge.Content.UI
{
	public class TextureManager
	{
		public Asset<Texture2D> transparentBox;
		public Dictionary<ConnectionDirection, (Asset<Texture2D> active, Asset<Texture2D> inactive)> directionTextures = new();
		public Dictionary<NodeType, (Asset<Texture2D> active, Asset<Texture2D> inactive)> nodeTextures = new();
		public Dictionary<UIType, (Asset<Texture2D> active, Asset<Texture2D> inactive)> generalUI = new();
		public Dictionary<NodeType, Asset<Texture2D>> statTextures = new();
		public static string pathToUIElements = @"Runeforge/Content/Assets/UIAssets/";
		public static string pathToNodeAssets = @"Runeforge/Content/Assets/NodeAssets/";
		public static string pathToStatAssets = @"Runeforge/Content/Assets/StatAssets/";
		public static List<(NodeType type, string name)> nodeElements = new()
			{
				(NodeType.Empty, "emptynode"),
				(NodeType.MovementSpeed, "movementspeednode"),
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
				(NodeType.Healing, "healingnode"),
				(NodeType.Poison, "poisondamagenode"),
				(NodeType.PoisonImbuement, "poisonimbuementnode"),
				(NodeType.CriticalHitChance, "critchancenode"),
				(NodeType.CriticalHitDamage, "critdamagenode"),
				(NodeType.MAJOR_RighteousFire, "righteousfirenode")
			};
			public static List<(NodeType type, string name)> statElements = new()
			{
				(NodeType.MovementSpeed, "stat_icon_movespeed"),
				(NodeType.Defence, "stat_icon_defence"),
				(NodeType.MeleeDamage, "stat_icon_melee_damage"),
				(NodeType.LifeSteal, "stat_icon_lifesteal"),
				(NodeType.RangedDamage, "stat_icon_range_damage"),
				(NodeType.SummonDamage, "stat_icon_summoner_damage"),
				(NodeType.BulletDamage, "stat_icon_bulletdamage"),
				(NodeType.MeleeAttackSpeed, "stat_icon_melee_attackspeed"),
				(NodeType.RangedAttackSpeed, "stat_icon_ranged_attackspeed"),
				(NodeType.ProjectileCount, "stat_icon_extraprojectiles"),
				(NodeType.CriticalHitChance, "stat_icon_critchance"),
				(NodeType.CriticalHitDamage, "stat_icon_critdamage"),
				(NodeType.MaxHealth, "stat_icon_maxhealth"),
				(NodeType.HealthRegen, "stat_icon_healthregen"),
				(NodeType.Healing, "stat_icon_healing"),
				(NodeType.MaxMana, "stat_icon_maxmana"),
				(NodeType.PoisonImbuement, "stat_icon_poisonimbuement"),
				(NodeType.Poison, "stat_icon_poison")
			};
			public static List<(List<ConnectionDirection> directions, string name)> connectionElements = new()
			{
				(new(){ConnectionDirection.UP, ConnectionDirection.DOWN}, "connection_vertical_long"),
				(new(){ConnectionDirection.RIGHT, ConnectionDirection.LEFT}, "connection_horizontal_long"),
				(new(){ConnectionDirection.DIAGONAL_BOTTOM_LEFT, ConnectionDirection.DIAGONAL_TOP_RIGHT}, "connection_diagonal_top"),
				(new(){ConnectionDirection.DIAGONAL_BOTTOM_RIGHT, ConnectionDirection.DIAGONAL_TOP_LEFT}, "connection_diagonal_bottom")
			};
		public TextureManager()
		{
			TextureManager textureManager = this;

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
			textureManager.transparentBox = ModContent.Request<Texture2D>($"{pathToUIElements}transparent_hoverover_text_box");

			textureManager.AddUI(UIType.HoverOver, hoverOver.active, hoverOver.inactive);
			textureManager.AddUI(UIType.LevelBar, levelBar, levelBar); // level bar is always active
			textureManager.AddUI(UIType.SkillPointOrb, skillPointOrb, skillPointOrb); // level bar is always active
		}
		public void AddNode(NodeType type, Asset<Texture2D> active, Asset<Texture2D> inactive)
		{
			if (nodeTextures.ContainsKey(type)) return;
			nodeTextures.Add(type, (active, inactive));
		}
		public void AddUI(UIType type, Asset<Texture2D> active, Asset<Texture2D> inactive)
		{
			if (generalUI.ContainsKey(type)) return;
			generalUI.Add(type, (active, inactive));
		}
		public void AddDirection(ConnectionDirection dir, Asset<Texture2D> active, Asset<Texture2D> inactive)
		{
			if (directionTextures.ContainsKey(dir)) return;
			directionTextures.Add(dir, (active, inactive));
		}
		public void AddStat(NodeType type, Asset<Texture2D> icon)
		{
			if (statTextures.ContainsKey(type)) return;
			statTextures.Add(type, icon);
		}
		public (Asset<Texture2D> active, Asset<Texture2D> inactive) GetNode(NodeType type)
		{
			if (nodeTextures.TryGetValue(type, out (Asset<Texture2D> active, Asset<Texture2D> inactive) tuple))
			{
				return tuple;
			}
			return (null, null);
		}
		public (Asset<Texture2D> active, Asset<Texture2D> inactive) GetUI(UIType type)
		{
			if (generalUI.TryGetValue(type, out (Asset<Texture2D> active, Asset<Texture2D> inactive) tuple))
			{
				return tuple;
			}
			return (null, null);
		}
		public (Asset<Texture2D> active, Asset<Texture2D> inactive) GetDirection(ConnectionDirection dir)
		{
			if (directionTextures.TryGetValue(dir, out (Asset<Texture2D> active, Asset<Texture2D> inactive) tuple))
			{
				return tuple;
			}
			return (null, null);
		}
		public Asset<Texture2D> GetStat(NodeType type)
		{
			if (statTextures.TryGetValue(type, out Asset<Texture2D> icon))
			{
				return icon;
			}
			return null;
		}
	}
}