using Buffalobuffalo.scripts.GOAP.Agents;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public class CollectAxe : GoapAction
    {
        protected override int Cost => 1;
        public override ConditionDict Preconditions => new() { };

        public override ConditionDict Effects => new(){
            {Condition.HasAxe, true}
        };

        public override bool Perform(GoapAgent agent, double delta)
        {
            throw new System.NotImplementedException();
        }
    }
}