
using Buffalobuffalo.scripts.GOAP.Agents;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class PickUpItemGoal : GoapGoal
    {
        protected override int Priority => 1;
        public override ConditionDict DesiredState => new(){
            { Condition.HasItemInHand, target_item }
        };

        private GodotObject target_item = null;
        public void SetTargetItem(GodotObject _target_item) {
            target_item = _target_item;
        }

        public override bool IsValid(GoapAgent agent)
        {
            return agent.State.HasState(Condition.HasItemInHand, target_item);
        }
    }
};