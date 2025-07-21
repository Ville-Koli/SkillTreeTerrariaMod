
using System;
using System.Collections.Generic;
using System.Text;
using Runeforge.Content.UI;
using Terraria;
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
            statBlock = new StatBlock();
        }

        public override void UpdateEquips()
        {
            base.UpdateEquips();
            if (statBlock != null)
            {
                Player.statDefense += (int)statBlock.DefenceIncrease;
                Player.GetDamage(DamageClass.Melee) *= statBlock.MeleeDamageIncrease;
                //ModContent.GetInstance<Runeforge>().Logger.Info("Adding defence: " + statBlock.GetDefenceIncrease());
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
                tag["meleeAttackSpeedIncrease"] = statBlock.MeleeAttackSpeedIncrease;
                tag["rangedAttackSpeedIncrease"] = statBlock.RangedAttackSpeedIncrease;
                tag["extraProjectiles"] = statBlock.ExtraProjectiles;
                tag["lifeRegenIncrease"] = statBlock.LifeRegenIncrease;
                tag["maxHealthIncrease"] = statBlock.MaxHealthIncrease;
                tag["maxManaIncrease"] = statBlock.MaxManaIncrease;
                tag["buffIDs"] = statBlock.GetBuffIDs();
                return;
            }
        }
        public override void OnEnterWorld()
        {
            // Apply the saved data to global UI / manager here
            if (TheUI.nodeManager != null)
            {
                TheUI.nodeManager.ApplyLoadedStatBlock(statBlock);
                NodeManager.ActivateNodesFromStringBuilder(new StringBuilder(activeNodes));
                ConnectionManager.ActivateNodesFromStringBuilder(new StringBuilder(activeConnections));
            }
            else
            {
                ModContent.GetInstance<Runeforge>().Logger.Info("[ENTERWORLD]: NodeManager not ready.");
            }
        }
        public override void LoadData(TagCompound tag)
        {
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: LOADING SAVE DATA: " + Main.LocalPlayer.name);
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
            statBlock.SetBuffIDs(tag.Get<List<int>>("buffIDs"));
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: Defence: " + tag.GetFloat("defenceIncrease"));
        }
    }
}