using System;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class PickUpItem : GoapAction
    {
        public PickUpItem(Func<double, bool> callback) : base(callback) {}

        protected override int Cost => 1;
        public override ConditionDict Preconditions => new();

        public override ConditionDict Effects => new();
    }
}