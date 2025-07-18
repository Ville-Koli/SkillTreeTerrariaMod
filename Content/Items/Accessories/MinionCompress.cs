using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Runeforge.Content.Buffs;

namespace Runeforge.Content.Items.Accessories
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class MinionCompress : ModItem
	{
		public Projectile minion;
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.Runeforge.hjson' file.
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.maxMinions += 10;
			player.statDefense -= 50;
			player.GetAttackSpeed(DamageClass.Melee) = 0.5f;

			if (minion == null)
			{
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile proj = Main.projectile[i];

					if (proj.active && proj.owner == player.whoAmI && (proj.minion || proj.sentry))
					{
						// Found the first minion
						minion = proj;
						break;
					}
				}
			}
			if (minion != null)
				{
					for (int i = 0; i < Main.maxProjectiles; i++)
					{
						Projectile proj = Main.projectile[i];

						if (proj.active && proj.owner == player.whoAmI && (proj.minion || proj.sentry))
						{
							proj.Center = (Vector2)minion.Center;
						}
					}
				}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.MeteoriteBar, 10);
			recipe.AddIngredient(ItemID.Sapphire, 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}

	public class MinionCompressorPlayer : ModPlayer
	{
		public bool compressMinions = false;
		public override void PostUpdate()
		{
			base.PostUpdate();
        }
	}
}