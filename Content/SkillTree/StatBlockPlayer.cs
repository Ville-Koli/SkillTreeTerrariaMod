
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Xna.Framework;
using Runeforge.Content.UI;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Runeforge.Content.SkillTree
{
    public class StatBlockPlayer : ModPlayer
    {
        public StatBlock statBlock;
        public string activeNodes = "";
        public string activeConnections = "";
        public override void Initialize()
        {
            statBlock = new StatBlock(); // initialize character based statblock
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            statBlock.AddExperience(damageDone);
            //ModContent.GetInstance<Runeforge>().Logger.Info($"Hit an enemy and gained {damageDone} exp!");
            if (hit.Crit && !hit.InstantKill)
            {
                hit.Damage = (int)(hit.Damage * statBlock.CritDamageIncrease);
            }
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spread = 1;
            int spreadCount = 10;
            for (int i = 0; i < statBlock.ExtraProjectiles; ++i)
            {
                // rotate velocity by the angle of spread
                float angle = spread * ((i + 1) % spreadCount) / 180 * MathF.PI;
                float newX = velocity.X * MathF.Cos(angle) - velocity.Y * MathF.Sin(angle);
                float newY = velocity.Y * MathF.Cos(angle) + velocity.X * MathF.Sin(angle);
                ModContent.GetInstance<Runeforge>().Logger.Info($"X AND Y: ({newX}, {newY}) velocity ({velocity.X}, {velocity.Y}) angle {angle}");
                Projectile.NewProjectile(source, position, new Vector2(newX, newY), type, damage, knockback);
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }

        public override void UpdateEquips()
        {
            base.UpdateEquips();
            if (statBlock != null)
            {
                Player.GetDamage(DamageClass.Melee) *= statBlock.MeleeDamageIncrease;
                Player.GetDamage(DamageClass.Ranged) *= statBlock.RangeDamageIncrease;
                Player.GetDamage(DamageClass.Magic) *= statBlock.MagicDamageIncrease;
                Player.GetDamage(DamageClass.Summon) *= statBlock.SummonDamageIncrease;
                Player.bulletDamage *= statBlock.BulletDamageIncrease;

                Player.GetAttackSpeed(DamageClass.Melee) *= statBlock.MeleeAttackSpeedIncrease;
                Player.GetAttackSpeed(DamageClass.Ranged) *= statBlock.RangedAttackSpeedIncrease;

                Player.statDefense += (int)statBlock.DefenceIncrease;
                Player.lifeRegenCount += (int)statBlock.LifeRegenIncrease;
                Player.statLifeMax2 += (int)statBlock.MaxHealthIncrease;
                Player.statManaMax2 += (int)statBlock.MaxManaIncrease;

                // apply buffs
                foreach (var buffid in statBlock.GetBuffIDs())
                {
                    Player.AddBuff(buffid, 100);
                }
            }
            else
            {
                ModContent.GetInstance<Runeforge>().Logger.Info("STATBLOCK IS NULL!!");
            }
        }
        public override void PreSavePlayer()
        {
            activeNodes = NodeManager.GetActiveNodesAsStringBuilder().ToString();
            activeConnections = ConnectionManager.GetActiveConnectionsAsStringBuilder().ToString();
        }
        public override void SaveData(TagCompound tag)
        {
            tag["activeNodes"] = activeNodes;
            tag["activeConnections"] = activeConnections;
            ModContent.GetInstance<Runeforge>().Logger.Info("[SAVEDATA] saving nodes: " + activeNodes);
            ModContent.GetInstance<Runeforge>().Logger.Info("[SAVEDATA] saving connections: " + activeConnections);
            if (statBlock != null)
            {
                tag["defenceIncrease"] = statBlock.DefenceIncrease;

                tag["meleeDamageIncrease"] = statBlock.MeleeDamageIncrease;
                tag["rangedDamageIncrease"] = statBlock.RangeDamageIncrease;
                tag["bulletDamageIncrease"] = statBlock.BulletDamageIncrease;
                tag["summonDamageIncrease"] = statBlock.SummonDamageIncrease;
                tag["magicDamageIncrease"] = statBlock.MagicDamageIncrease;

                tag["meleeAttackSpeedIncrease"] = statBlock.MeleeAttackSpeedIncrease;
                tag["rangedAttackSpeedIncrease"] = statBlock.RangedAttackSpeedIncrease;

                tag["extraProjectiles"] = statBlock.ExtraProjectiles;

                tag["lifeRegenIncrease"] = statBlock.LifeRegenIncrease;
                tag["maxHealthIncrease"] = statBlock.MaxHealthIncrease;
                tag["maxManaIncrease"] = statBlock.MaxManaIncrease;

                tag["currentExp"] = statBlock.CurrentExperience;
                tag["currentLevel"] = statBlock.CurrentLevel;
                tag["requiredExp"] = statBlock.RequiredExperienceForLevel;
                tag["skillpoints"] = statBlock.SkillPoints;
                ModContent.GetInstance<Runeforge>().Logger.Info($"[SAVING] [LEVEL STATS]: {statBlock.CurrentExperience}, {statBlock.CurrentLevel}, {statBlock.RequiredExperienceForLevel}");
                tag["buffIDs"] = statBlock.GetBuffIDs();
                return;
            }
        }
        public override void OnEnterWorld()
        {
            // Apply the saved data to global UI / manager here
            if (SkillTreeUIState.nodeManager != null)
            {
                SkillTreeUIState.nodeManager.ApplyLoadedStatBlock(statBlock); // apply the new loaded statblock to be the used one to all nodes
                NodeManager.ActivateNodesFromStringBuilder(new StringBuilder(activeNodes)); // activate the previously active nodes
                ConnectionManager.ActivateNodesFromStringBuilder(new StringBuilder(activeConnections)); // activate previously active connections
                statBlock.UpdateLevelUI();
                statBlock.UpdateSkillPointAmount();
                ModContent.GetInstance<Runeforge>().Logger.Info($"[ENTERWORLD] [LEVEL STATS]: {statBlock.CurrentExperience}, {statBlock.CurrentLevel}, {statBlock.RequiredExperienceForLevel}");
            }
            else
            {
                ModContent.GetInstance<Runeforge>().Logger.Info("[ENTERWORLD]: NodeManager not ready.");
            }
        }
        public override void LoadData(TagCompound tag)
        {
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: LOADING SAVE DATA: " + Player.name);
            activeNodes = tag.GetString("activeNodes");
            activeConnections = tag.GetString("activeConnections");

            statBlock.DefenceIncrease = tag.GetFloat("defenceIncrease");
            statBlock.MeleeDamageIncrease = tag.GetFloat("meleeDamageIncrease");
            statBlock.RangeDamageIncrease = tag.GetFloat("rangedDamageIncrease");
            statBlock.BulletDamageIncrease = tag.GetFloat("bulletDamageIncrease");
            statBlock.SummonDamageIncrease = tag.GetFloat("summonDamageIncrease");

            statBlock.MeleeAttackSpeedIncrease = tag.GetFloat("meleeAttackSpeedIncrease");
            statBlock.RangedAttackSpeedIncrease = tag.GetFloat("rangedAttackSpeedIncrease");

            statBlock.ExtraProjectiles = tag.GetFloat("extraProjectiles");

            statBlock.LifeRegenIncrease = tag.GetFloat("lifeRegenIncrease");
            statBlock.MaxHealthIncrease = tag.GetFloat("maxHealthIncrease");
            statBlock.MaxManaIncrease = tag.GetFloat("maxManaIncrease");

            statBlock.CurrentExperience = tag.GetFloat("currentExp");
            statBlock.CurrentLevel = tag.GetFloat("currentLevel");
            statBlock.RequiredExperienceForLevel = tag.GetFloat("requiredExp");
            statBlock.SkillPoints = tag.GetInt("skillpoints");
            ModContent.GetInstance<Runeforge>().Logger.Info($"[LOADING] [LEVEL STATS]: {tag.GetFloat("currentExp")}, {tag.GetFloat("currentLevel")}, {tag.GetFloat("requiredExp")}");

            statBlock.SetBuffIDs(tag.Get<List<int>>("buffIDs"));
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: Defence: " + tag.GetFloat("defenceIncrease"));
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: Melee Damage: " + tag.GetFloat("meleeDamageIncrease"));
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: Melee Attackspeed: " + tag.GetFloat("meleeAttackSpeedIncrease"));
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: MaxHealth: " + tag.GetFloat("maxHealthIncrease"));
        }
    }
}