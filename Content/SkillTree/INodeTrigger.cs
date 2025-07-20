using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree
{
    public interface INodeTrigger
    {
        void Activate(StatBlock statBlock);
        void DeActivate(StatBlock statBlock);
    }
}