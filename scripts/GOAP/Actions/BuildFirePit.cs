using Buffalobuffalo.scripts.GOAP.Agents;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class BuildFirePit : GoapAction
    {
        protected override int Cost => 1;
        public override ConditionDict Preconditions => new(){
            {Condition.HasWood, true}
        };

        public override ConditionDict Effects => new(){
            {Condition.HasFirepit, true}
        };

        public override bool Perform(GoapAgent agent, double delta)
        {
            throw new System.NotImplementedException();
        }
    }
}