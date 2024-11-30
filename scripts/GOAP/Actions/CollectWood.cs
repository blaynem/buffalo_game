using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class CollectWood : GoapAction
    {
        public CollectWood(Func<double, bool> action_callback) : base(action_callback) {}

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new() { };

        public static ConditionDict StaticEffects => new(){
            {Condition.HasWood, true}
        };

        public override ConditionDict GetEffects() { return StaticEffects; }
    }
}