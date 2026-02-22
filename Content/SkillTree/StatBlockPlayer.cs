
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Xna.Framework;
using Runeforge.Content.Buffs;
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
        }

        public override void ModifyMaxStats(out StatModifier health, out StatModifier mana)
        {
            base.ModifyMaxStats(out health, out mana);
            health.Base += statBlock.MaxHealthIncrease;
            mana.Base += statBlock.MaxManaIncrease;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            base.ModifyHitNPC(target, ref modifiers);
            modifiers.CritDamage += statBlock.CritDamageIncrease;
        }

        public override bool Shoot(Item item, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float spread = 5; // degree of angle for spread
            int spreadCount = 10;
            int counter = 0;
            for (int i = 0; i < statBlock.ExtraProjectiles; ++i)
            {
                // rotate velocity by the angle of spread
                float angle = spread * ((counter + 1) % spreadCount) / 180 * MathF.PI;
                float cosVal = MathF.Cos(angle);
                float sinVal = MathF.Sin(angle);
                int sign;
                if (i % 2 == 0)
                {
                    if (i != 0)
                        counter++;
                    sign = 1;
                }
                else
                {
                    sign = -1;
                }
                float newX = velocity.X * cosVal + velocity.Y * sinVal * sign;
                float newY = velocity.Y * cosVal - velocity.X * sinVal * sign;
                Projectile.NewProjectile(source, position, new Vector2(newX, newY), type, damage, knockback);
            }
            return base.Shoot(item, source, position, velocity, type, damage, knockback);
        }

        public override void GetHealLife(Item item, bool quickHeal, ref int healValue)
        {
            base.GetHealLife(item, quickHeal, ref healValue);
            healValue *= (int)statBlock.HealingIncrease;
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

                Player.GetAttackSpeed(DamageClass.Melee) += statBlock.MeleeAttackSpeedIncrease;
                Player.GetAttackSpeed(DamageClass.Ranged) += statBlock.RangedAttackSpeedIncrease;
                Player.lifeSteal *= statBlock.LifestealIncrease;

                Player.statDefense += (int)statBlock.DefenceIncrease;
                Player.lifeRegenCount += (int)statBlock.LifeRegenIncrease;

                Player.moveSpeed += statBlock.MovementSpeedIncrease / 100;

                Player.GetCritChance(DamageClass.Generic) += statBlock.CritChanceIncrease;

                //Player.wingsLogic = 0; you can use this to disable wings

                // apply buffs
                foreach (var buffid in statBlock.GetBuffIDs())
                {
                    Player.AddBuff(buffid, 100);
                }
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

            if (statBlock != null)
            {
                tag["currentExp"] = statBlock.CurrentExperience;
                tag["currentLevel"] = statBlock.CurrentLevel;
                tag["requiredExp"] = statBlock.RequiredExperienceForLevel;
                tag["skillpoints"] = statBlock.SkillPoints;
                return;
            }
        }
        public override void OnEnterWorld()
        {
            // Apply the saved data to global UI / manager here
            if (SkillTreeUIState.nodeManager != null)
            {
                NodeManager.ActivateNodesFromStringBuilder(new StringBuilder(activeNodes), statBlock); // activate the previously active nodes
                ConnectionManager.ActivateNodesFromStringBuilder(new StringBuilder(activeConnections)); // activate previously active connections
                SkillTreeUIState.nodeManager.ApplyLoadedStatBlock(statBlock); // apply the new loaded statblock to be the used one to all nodes
                statBlock.UpdateLevelUI();
                statBlock.UpdateSkillPointAmount();
            }
        }
        public override void LoadData(TagCompound tag)
        {
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: LOADING SAVE DATA: " + Player.name);
            activeNodes = tag.GetString("activeNodes");
            activeConnections = tag.GetString("activeConnections");

            statBlock.CurrentExperience = tag.GetFloat("currentExp");
            statBlock.CurrentLevel = tag.GetFloat("currentLevel");
            statBlock.RequiredExperienceForLevel = tag.GetFloat("requiredExp");
            statBlock.SkillPoints = tag.GetInt("skillpoints");

            statBlock.SetBuffIDs(tag.Get<List<int>>("buffIDs"));
        }
    }
}