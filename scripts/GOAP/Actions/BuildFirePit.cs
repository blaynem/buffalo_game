using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class BuildFirePit : GoapAction
    {
        public BuildFirePit(Func<double, bool> action_callback) : base(action_callback) { }

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new(){
            {Condition.HasWood, true}
        };

        public static ConditionDict StaticEffects => new(){
            {Condition.HasFirepit, true}
        };

        public override ConditionDict GetEffects() { return StaticEffects; }
    }
}