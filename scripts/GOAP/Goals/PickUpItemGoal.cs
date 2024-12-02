using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class PickUpItemGoal : GoapGoal
    {
        public override int GetPriority(GoapAgent agent)
        {
            // If within range of the item, let's make this a priority of 2.
            var distance = agent.Actor.GlobalPosition.DistanceSquaredTo(target_item.GlobalPosition);
            if (distance <= 50) {
                return 2;
            }
            return 1;
        }

        public override ConditionDict DesiredState => new(){
            { Condition.HasItemInHand, true }
        };

        private Node3D target_item = null;
        public PickUpItemGoal(Node3D _target_item) {
            target_item = _target_item;
        }

        public override bool IsValid(GoapAgent agent)
        {
            // If we've matched these conditions, then the goal is no longer valid.
            return !agent.State.MatchesCondition(Condition.HasItemInHand, target_item);
        }
    }
};