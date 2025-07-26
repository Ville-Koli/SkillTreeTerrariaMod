using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class DefenceNodeTrigger : INodeTrigger
    {
        private float defence;
        public DefenceNodeTrigger(float defence)
        {
            this.defence = defence;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddDefenceIncrease(defence);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddDefenceIncrease(-defence);
        }

        public float ReturnTriggerElement()
        {
            return defence;
        }
    }
}