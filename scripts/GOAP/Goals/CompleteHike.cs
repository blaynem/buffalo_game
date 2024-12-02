
namespace Buffalobuffalo.scripts.GOAP.Goals {
    public partial class CompleteHike : GoapGoal
    {

        public override ConditionDict DesiredState => new(){
            {Condition.CompletedHike, true}
        };

        public override bool IsValid(GoapAgent agent)
        {
            // If we've matched these conditions, then the goal is no longer valid.
            return !agent.State.MatchesCondition(Condition.CompletedHike, true);
        }
    }
}