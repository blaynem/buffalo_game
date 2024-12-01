using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class PickUpItemGoal : GoapGoal
    {
        protected override int Priority => 1;
        public override ConditionDict DesiredState => new(){
            { Condition.HasItemInHand, true }
        };

        private GodotObject target_item = null;
        public PickUpItemGoal(GodotObject _target_item) {
            target_item = _target_item;
        }

        public override bool IsValid(GoapAgent agent)
        {
            return !agent.State.HasState(Condition.HasItemInHand, target_item);
        }
    }
};