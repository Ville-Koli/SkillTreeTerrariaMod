using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class HealingNodeTrigger : INodeTrigger
    {
        private float healingIncrease;
        public HealingNodeTrigger(float healingIncrease)
        {
            this.healingIncrease = healingIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddHealingIncrease(healingIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddHealingIncrease(-healingIncrease);
        }

        public float ReturnTriggerElement()
        {
            return healingIncrease;
        }
    }
}