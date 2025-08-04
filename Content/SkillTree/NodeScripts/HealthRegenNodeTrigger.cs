using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class HealthRegenNodeTrigger : INodeTrigger
    {
        private float healthRegenIncrease;
        public HealthRegenNodeTrigger(float healthRegenIncrease)
        {
            this.healthRegenIncrease = healthRegenIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddLifeRegenIncrease(healthRegenIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddLifeRegenIncrease(-healthRegenIncrease);
        }

        public float ReturnTriggerElement()
        {
            return healthRegenIncrease;
        }
    }
}