
using Buffalobuffalo.scripts.GOAP.Agents;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class TakeInTheSightsGoal : GoapGoal
    {
        protected override int Priority => 1;
        public override ConditionDict DesiredState => new(){
            { Condition.WantsToViewSights, false }
        };

        public override bool IsValid(GoapAgent agent)
        {
            return !agent.State.HasState(Condition.WantsToViewSights, true);
        }
    }
}