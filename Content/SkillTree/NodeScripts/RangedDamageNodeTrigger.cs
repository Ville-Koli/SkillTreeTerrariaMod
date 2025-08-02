using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class RangedDamageNodeTrigger : INodeTrigger
    {
        private float rangeDamageIncrease;
        public RangedDamageNodeTrigger(float rangeDamageIncrease)
        {
            this.rangeDamageIncrease = rangeDamageIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddRangeDamageIncrease(rangeDamageIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddRangeDamageIncrease(-rangeDamageIncrease);
        }

        public float ReturnTriggerElement()
        {
            return rangeDamageIncrease;
        }
    }
}