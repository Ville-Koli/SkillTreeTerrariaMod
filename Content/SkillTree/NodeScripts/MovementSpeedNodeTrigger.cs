using Runeforge.Content.UI;

namespace Runeforge.Content.SkillTree.NodeScripts
{
    public class MovementSpeedNodeTrigger : INodeTrigger
    {
        private float movementSpeedIncrease;
        public MovementSpeedNodeTrigger(float movementSpeedIncrease)
        {
            this.movementSpeedIncrease = movementSpeedIncrease;
        }
        public void Activate(StatBlock statBlock)
        {
            statBlock.AddMovementSpeedIncreasee(movementSpeedIncrease);
        }
        public void DeActivate(StatBlock statBlock)
        {
            statBlock.AddMovementSpeedIncreasee(-movementSpeedIncrease);
        }

        public float ReturnTriggerElement()
        {
            return movementSpeedIncrease;
        }
    }
}