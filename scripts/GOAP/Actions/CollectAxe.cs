using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class CollectAxe : GoapAction
    {
        public CollectAxe(Func<double, bool> callback) : base(callback) {}

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new() { };

        public override ConditionDict Effects => new(){
            {Condition.HasAxe, true}
        };
    }
}