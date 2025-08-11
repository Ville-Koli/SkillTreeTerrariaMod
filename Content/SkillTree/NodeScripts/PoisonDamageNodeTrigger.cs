using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class PoisonDamageNodeTrigger : INodeTrigger
    {
        private float poisonDamageIncrease;
        public PoisonDamageNodeTrigger(float poisonDamageIncrease)
        {
            this.poisonDamageIncrease = poisonDamageIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddPoisonDamageIncrease(poisonDamageIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddPoisonDamageIncrease(-poisonDamageIncrease);
        }

        public float ReturnTriggerElement()
        {
            return poisonDamageIncrease;
        }
    }
}