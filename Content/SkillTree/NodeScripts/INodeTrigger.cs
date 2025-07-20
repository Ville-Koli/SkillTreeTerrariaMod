using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public interface INodeTrigger
    {
        void Activate(StatBlock statBlock);
        void DeActivate(StatBlock statBlock);
    }
}