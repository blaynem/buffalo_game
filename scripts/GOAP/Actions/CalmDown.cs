
using System;

namespace Buffalobuffalo.scripts.GOAP.Actions {
    public partial class CalmDown : GoapAction
    {
        public CalmDown(Func<double, bool> action_callback) : base(action_callback)
        {
        }

        public override int GetCost(GoapAgent agent, Blackboard blackboard)
        {
            return 1;
        }

        public override ConditionDict Preconditions => new(){
            { Condition.IsSafeDistance, true }
        };

        public static ConditionDict StaticEffects => new(){
            { Condition.IsScared, false }
        };

        public override ConditionDict GetEffects()
        {
            return StaticEffects;
        }
    }
}