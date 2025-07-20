
using System;
using System.Text;
using Runeforge.Content.UI;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Runeforge.Content.SkillTree
{
    public class StatBlockPlayer : ModPlayer
    {
        public StatBlock statBlock;
        public string activeNodes = "";
        public string activeConnections = "";

        public void UpdateStatBlock(StatBlock newStatBlock)
        {
            statBlock = newStatBlock;
        }
        public StatBlock GetStatBlock()
        {
            return statBlock;
        }
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
                //ModContent.GetInstance<Runeforge>().Logger.Info("Adding defence: " + statBlock.GetDefenceIncrease());
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
            ModContent.GetInstance<Runeforge>().Logger.Info("[LOADDATA]: Defence: " + tag.GetFloat("defenceIncrease"));
        }
    }
    public class StatBlock
    {
        private float defenceIncrease;
        public void ApplyStatBlock(StatBlock statBlock)
        {
            defenceIncrease = statBlock.GetDefenceIncrease();
        }
        public float GetDefenceIncrease()
        {
            return defenceIncrease;
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