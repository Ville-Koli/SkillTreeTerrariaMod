using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class CritDamageNodeTrigger : INodeTrigger
    {
        private float critDamageIncrease;
        public CritDamageNodeTrigger(float critDamageIncrease)
        {
            this.critDamageIncrease = critDamageIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddCritDamageIncrease(critDamageIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddCritDamageIncrease(-critDamageIncrease);
        }

        public float ReturnTriggerElement()
        {
            return critDamageIncrease;
        }
    }
}