using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class MaxManaNodeTrigger : INodeTrigger
    {
        private float maxManaIncrease;
        public MaxManaNodeTrigger(float maxManaIncrease)
        {
            this.maxManaIncrease = maxManaIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddMaxManaIncrease(maxManaIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddMaxManaIncrease(-maxManaIncrease);
        }

        public float ReturnTriggerElement()
        {
            return maxManaIncrease;
        }
    }
}