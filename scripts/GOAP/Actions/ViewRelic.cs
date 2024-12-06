using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class ViewRelic : GoapAction
    {
        public ViewRelic(Func<double, bool> action_callback) : base(action_callback) {}

        public override int GetCost(GoapAgent agent)
        {
            return 1;
        }
        public override ConditionDict Preconditions => new();

        public static ConditionDict StaticEffects => new(){
            {Condition.ViewedRelic, true}
        };

        public override ConditionDict GetEffects() { return StaticEffects; }
    }
}