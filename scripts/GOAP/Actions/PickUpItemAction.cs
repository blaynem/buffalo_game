using Buffalobuffalo.scripts.GOAP.Agents;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class PickUpItemAction : GoapAction
    {
        protected override int Cost => 1;
        public override ConditionDict Preconditions => new();

        public override ConditionDict Effects => new();

        public override bool Perform(GoapAgent agent, double delta)
        {
            GD.Print("----can't pickup because no method implemented");
            throw new System.NotImplementedException();
        }
    }
}