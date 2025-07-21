using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class MeleeDamageNodeTrigger : INodeTrigger
    {
        private float meleeDamageIncrease;
        public MeleeDamageNodeTrigger(float meleeDamageIncrease)
        {
            this.meleeDamageIncrease = meleeDamageIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddMeleeDamageIncrease(meleeDamageIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddMeleeDamageIncrease(-meleeDamageIncrease);
        }
    }
}