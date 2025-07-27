using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class EmptyNodeTrigger : INodeTrigger
    {
        public EmptyNodeTrigger()
        {
        }
        public void Activate(StatBlock statBlock)
        {
        }
        public void DeActivate(StatBlock statBlock)
        {
        }

        public float ReturnTriggerElement()
        {
            return 0;
        }
    }
}