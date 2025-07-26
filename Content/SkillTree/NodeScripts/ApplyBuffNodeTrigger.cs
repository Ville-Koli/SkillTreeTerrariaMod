using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class ApplyBuffNodeTrigger : INodeTrigger
    {
        private int buffID;
        public ApplyBuffNodeTrigger(int buffID)
        {
            this.buffID = buffID;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.ApplyBuff(buffID);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.RemoveBuff(buffID);
        }

        public float ReturnTriggerElement()
        {
            return buffID;
        }
    }
}