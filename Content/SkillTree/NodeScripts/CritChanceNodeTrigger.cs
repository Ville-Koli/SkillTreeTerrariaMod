using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class CritChanceNodeTrigger : INodeTrigger
    {
        private float critChanceIncrease;
        public CritChanceNodeTrigger(float critChanceIncrease)
        {
            this.critChanceIncrease = critChanceIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddCritChanceIncrease(critChanceIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddCritChanceIncrease(-critChanceIncrease);
        }

        public float ReturnTriggerElement()
        {
            return critChanceIncrease;
        }
    }
}