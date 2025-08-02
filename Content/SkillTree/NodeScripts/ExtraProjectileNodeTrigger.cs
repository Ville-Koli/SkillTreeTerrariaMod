using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class ExtraProjectileNodeTrigger : INodeTrigger
    {
        private float extraProjectiles;
        public ExtraProjectileNodeTrigger(float extraProjectiles)
        {
            this.extraProjectiles = extraProjectiles;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddExtraProjectiles(extraProjectiles);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddExtraProjectiles(-extraProjectiles);
        }

        public float ReturnTriggerElement()
        {
            return extraProjectiles;
        }
    }
}