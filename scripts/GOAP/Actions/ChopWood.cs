using System;
namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class ChopWood : GoapAction
    {
        public ChopWood(Func<double, bool> action_callback) : base(action_callback) {}

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new(){
            {Condition.HasAxe, true}
        };

        public override ConditionDict Effects => new(){
            {Condition.HasWood, true}
        };
    }
}