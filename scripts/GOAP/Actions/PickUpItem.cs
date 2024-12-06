using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class PickUpItem : GoapAction
    {
        public PickUpItem(Func<double, bool> action_callback) : base(action_callback) {}

        public override int GetCost(GoapAgent agent, Blackboard blackboard)
        {
            return 1;
        }
        public override ConditionDict Preconditions => new();

        public static ConditionDict StaticEffects => new(){
            {Condition.HasItemInHand, true}
        };

        public override ConditionDict GetEffects() { return StaticEffects; }
    }
}