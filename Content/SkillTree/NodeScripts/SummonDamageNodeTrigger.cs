using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class SummonDamageNodeTrigger : INodeTrigger
    {
        private float summonDamageIncrease;
        public SummonDamageNodeTrigger(float summonDamageIncrease)
        {
            this.summonDamageIncrease = summonDamageIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddSummonDamageIncrease(summonDamageIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddSummonDamageIncrease(-summonDamageIncrease);
        }

        public float ReturnTriggerElement()
        {
            return summonDamageIncrease;
        }
    }
}