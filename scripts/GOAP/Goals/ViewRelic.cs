using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class ViewRelic : GoapGoal
    {
        public override ConditionDict DesiredState => new(){
            { Condition.ViewedRelic, true }
        };

        public override int GetPriority(GoapAgent agent)
        {
            // If within range of the target, let's make this a priority of 2.
            var distance = agent.Actor.GlobalPosition.DistanceSquaredTo(target_relic.GlobalPosition);
            if (distance <= 1500) {
                return 2;
            }
            return 1;
        }

        public Node3D target_relic {get; private set;} = null;
        public ViewRelic(Node3D _target_relic) {
            target_relic = _target_relic;
        }

        public override bool IsValid(GoapAgent agent)
        {
            // If we've matched these conditions, then the goal is no longer valid.
            return !agent.State.MatchesCondition(Condition.ViewedRelic, true);
        }
    }
}