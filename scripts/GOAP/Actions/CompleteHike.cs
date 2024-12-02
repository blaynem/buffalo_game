using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class CompleteHike : GoapAction
    {
        public CompleteHike(Func<double, bool> action_callback) : base(action_callback) { }

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new();

        public static ConditionDict StaticEffects => new(){
            { Condition.CompletedHike, true }
        };

        public override ConditionDict GetEffects(){ return StaticEffects; }
    }
}