
namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class StaySafe : GoapGoal
    {
        public override ConditionDict DesiredState => new(){
            {Condition.IsScared, false}
        };

        public override int GetPriority(GoapAgent agent)
        {
            return 10;
        }

        public override bool IsValid(GoapAgent agent)
        {
            return agent.State.MatchesCondition(Condition.IsScared, true);
        }
    }
}