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
            // There is a bug where the actor walks too far away and then this priority gets set back to 1.
            if (distance <= 200 || agent.Brain.current_goal == this) {
                return 2;
            }
            return 1;
        }

        private bool relic_viewed = false;

        public void CompleteGoal() {
            relic_viewed = true;
        }
        public bool IsCompleted() {
            return relic_viewed;
        }

        public Vector3 target_location { get; private set; }
        public Node3D target_relic {get; private set;} = null;
        public ViewRelic(Node3D _target_relic) {
            target_relic = _target_relic;
            target_location = (Vector3)_target_relic.Call("get_view_location");
        }

        public override bool IsValid(GoapAgent agent)
        {
            // If relic hasn't been viewed yet, it's valid.
            return !relic_viewed;
        }
    }
}