
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class CompleteActivityZone : GoapGoal
    {

        public override ConditionDict DesiredState => new(){
            {Condition.CompleteActivity, true}
        };

        private bool zone_interacted_with = false;
        public Vector3 target_location { get; private set; }
        public string interaction_animation {get; private set;}
        public Node3D Activity_zone { get; private set; } = null;
        public CompleteActivityZone(Node3D _target)
        {
            Activity_zone = _target;

            target_location = (Vector3)Activity_zone.Call("get_target_location");
            interaction_animation = (string)Activity_zone.Call("get_interaction_animation");
        }

        public void CompleteGoal() {
            zone_interacted_with = true;
        }
        public bool IsCompleted() {
            return zone_interacted_with;
        }

        public override int GetPriority(GoapAgent agent)
        {
            // We want the distance to the zone
            var distance = agent.Actor.GlobalPosition.DistanceSquaredTo(Activity_zone.GlobalPosition);
            if (distance <= 100)
            {
                return 2;
            }
            return 1;
        }

        public override bool IsValid(GoapAgent agent)
        {
            // If zone has not been interacted with.
            return !zone_interacted_with;
        }
    }
}