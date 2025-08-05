using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Runeforge.Content.Buffs;

namespace Runeforge.Content.Items.Accessories
{
	// This is a basic item template.
	// Please see tModLoader's ExampleMod for every other example:
	// https://github.com/tModLoader/tModLoader/tree/stable/ExampleMod
	public class RighteousFire : ModItem
	{
		public int damage = 2;
		public int damageCooldown = 10;
		public double distance = 150;
		private int damageCooldownCounter = 0;
		// The Display Name and Tooltip of this item can be edited in the 'Localization/en-US_Mods.Runeforge.hjson' file.
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 28;
			Item.value = Item.buyPrice(10);
			Item.rare = ItemRarityID.Red;
			Item.accessory = true;
			Item.DamageType = DamageClass.Melee;
			Item.defense -= 10;
			Item.lifeRegen -= 10;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			if (Main.myPlayer == player.whoAmI && !player.inferno)
			{
				//player.GetModPlayer<RighteousFirePlayer>().isCustomInfernoEnabled = true;
				player.AddBuff(ModContent.BuffType<CustomInferno>(), 100);
				player.AddBuff(BuffID.OnFire3, 10);

				//player.inferno = true;
				//player.infernoCounter = 1;
				if (damageCooldownCounter == 0)
				{
					foreach (NPC npc in Main.npc)
					{
						if (!npc.friendly)
						{
							if (npc.Center.Distance(player.Center) < distance)
							{
								player.ApplyDamageToNPC(npc, damage * player.HeldItem.damage, 0, 0);
								npc.AddBuff(BuffID.OnFire3, 10);
							}
						}
					}
					damageCooldownCounter = damageCooldown;
				}
				if (damageCooldownCounter > 0)
				{
					damageCooldownCounter--;
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.HellstoneBar, 10);
			recipe.AddIngredient(ItemID.HellstoneBrick, 50);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}

		public class RighteousFirePlayer : ModPlayer
		{
			public bool isCustomInfernoEnabled = false;
			public override void ResetEffects()
			{
				isCustomInfernoEnabled = false;
			}

            public override void PreUpdateBuffs()
            {
				if (isCustomInfernoEnabled)
				{
					Player.AddBuff(ModContent.BuffType<CustomInferno>(), 100);	
				}
            }
		}
	}
}