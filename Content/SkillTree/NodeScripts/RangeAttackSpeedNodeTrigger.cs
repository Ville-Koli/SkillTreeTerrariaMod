using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class RangedAttackSpeedNodeTrigger : INodeTrigger
    {
        private float attackSpeed;
        public RangedAttackSpeedNodeTrigger(float attackSpeed)
        {
            this.attackSpeed = attackSpeed;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddRangedAttackSpeedIncrease(attackSpeed);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddRangedAttackSpeedIncrease(-attackSpeed);
        }

        public float ReturnTriggerElement()
        {
            return attackSpeed;
        }
    }
}