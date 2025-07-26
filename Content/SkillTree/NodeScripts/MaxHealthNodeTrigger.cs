using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class MaxHealthNodeTrigger : INodeTrigger
    {
        private float maxHealth;
        public MaxHealthNodeTrigger(float maxHealth)
        {
            this.maxHealth = maxHealth;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddMaxHealthIncrease(maxHealth);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddMaxHealthIncrease(-maxHealth);
        }

        public float ReturnTriggerElement()
        {
            return maxHealth;
        }
    }
}