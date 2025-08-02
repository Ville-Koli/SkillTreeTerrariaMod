using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class MeleeAttackSpeedNodeTrigger : INodeTrigger
    {
        private float attackSpeed;
        public MeleeAttackSpeedNodeTrigger(float attackSpeed)
        {
            this.attackSpeed = attackSpeed;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddMeleeAttackSpeedIncrease(attackSpeed);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddMeleeAttackSpeedIncrease(-attackSpeed);
        }

        public float ReturnTriggerElement()
        {
            return attackSpeed;
        }
    }
}