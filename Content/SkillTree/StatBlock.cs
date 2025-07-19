
using Runeforge.Content.UI;
using Terraria.ModLoader;

namespace Runeforge.Content.SkillTree
{
    public class StatBlockPlayer : ModPlayer
    {
        public int defenceIncrease;

        public void ApplyStatBlock(StatBlock statBlock)
        {
            defenceIncrease = statBlock.defenceIncrease;
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void UpdateEquips()
        {
            base.UpdateEquips();
            Player.statDefense += defenceIncrease;
        }
    }
    public class StatBlock
    {
        public int defenceIncrease;
    }
}