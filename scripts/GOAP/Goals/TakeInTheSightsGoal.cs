
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class TakeInTheSightsGoal : GoapGoal
    {
        public override ConditionDict GetDesiredState()
        {
            return new ConditionDict() {
                { Condition.WantsToViewSights, false }
            };
        }

        public override string GetGoalName()
        {
            return "TakeInTheSights";
        }

        public override bool IsValid(GoapAgent agent)
        {
            return agent.StateHasDesire(Condition.WantsToViewSights, true);
        }

        public override int Priority()
        {
            return 1;
        }
    }
}