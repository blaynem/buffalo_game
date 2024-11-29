using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class PickUpItem : GoapAction
    {
        public PickUpItem(Func<double, bool> action_callback) : base(action_callback) {}

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new();

        public new static ConditionDict Effects => new(){
            {Condition.HasItemInHand, true}
        };
    }
}