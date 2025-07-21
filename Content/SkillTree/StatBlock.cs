
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
                Player.statDefense += (int)statBlock.GetDefenceIncrease();
                Player.GetDamage(DamageClass.Melee) *= statBlock.GetMeleeDamageIncrease();
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
                tag["defenceIncrease"] = statBlock.GetDefenceIncrease();
                tag["meleeDamageIncrease"] = statBlock.GetMeleeDamageIncrease();
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
            statBlock.SetDefenceIncrease(tag.GetFloat("defenceIncrease"));
            statBlock.SetMeleeDamageIncrease(tag.GetFloat("meleeDamageIncrease"));
            statBlock.SetBuffIDs(tag.Get<List<int>>("buffIDs"));
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: Defence: " + tag.GetFloat("defenceIncrease"));
        }
    }
    public class StatBlock
    {
        private List<int> buffIDs = new(); // list of buffs that the statblock provides to SBP
        private float defenceIncrease = 0;
        private float meleeDamageIncrease = 1;
        public List<int> GetBuffIDs()
        {
            return buffIDs;
        }
        public void SetBuffIDs(List<int> newBuffIDs)
        {
            buffIDs = newBuffIDs;
        }
        public bool ApplyBuff(int buffid)
        {
            if (!buffIDs.Contains(buffid))
            {
                buffIDs.Add(buffid);
                return true;
            }
            return false;
        }
        public bool RemoveBuff(int buffid)
        {
            return buffIDs.Remove(buffid);
        }
        public float GetDefenceIncrease()
        {
            return defenceIncrease;
        }
        public float GetMeleeDamageIncrease()
        {
            return meleeDamageIncrease;
        }
        public void SetMeleeDamageIncrease(float meleeDamageIncrease)
        {
            this.meleeDamageIncrease = meleeDamageIncrease;
        }
        public void AddMeleeDamageIncrease(float meleeDamageIncrease)
        {
            this.meleeDamageIncrease += meleeDamageIncrease;
        }
        public void SetDefenceIncrease(float newDefenceIncrease)
        {
            defenceIncrease = newDefenceIncrease;
        }
        public void AddDefenceIncrease(float newDefenceIncrease)
        {
            defenceIncrease += newDefenceIncrease;
        }
    }
}