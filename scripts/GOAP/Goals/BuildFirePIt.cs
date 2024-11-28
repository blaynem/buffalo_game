using Buffalobuffalo.scripts.GOAP.Agents;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class BuildFirePit : GoapGoal
    {
        protected override int Priority => 1;
        public override ConditionDict DesiredState => new(){
            { Condition.HasFirepit, true }
        };

        public override int GetPriority()
        {
            return base.GetPriority();
        }

        public override bool IsValid(GoapAgent agent)
        {
            return agent.State.HasState(Condition.HasFirepit, false);
        }
    }
}