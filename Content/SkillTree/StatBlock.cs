
using Runeforge.Content.UI;
using Terraria.ModLoader;

namespace Runeforge.Content.SkillTree
{
    public class StatBlockPlayer : ModPlayer
    {
        public StatBlock statBlock = new();

        public void ApplyStatBlock(StatBlock statBlock)
        {
            this.statBlock = statBlock;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void UpdateEquips()
        {
            base.UpdateEquips();
            Player.statDefense += (int)statBlock.GetDefenceIncrease();
        }
    }
    public class StatBlock
    {
        private float defenceIncrease;
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