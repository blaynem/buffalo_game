using System.Collections.Generic;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class PickUpItem : GoapAction
    {
        public Node3D item = null;

        public override int GetCost(GoapAgent agent)
        {
            return 1;
        }

        public override ConditionDict GetEffects()
        {
            return new ConditionDict();
        }

        public override ConditionDict GetPreconditions()
        {
            return new ConditionDict();
        }

        public override bool IsValid()
        {
            throw new System.NotImplementedException();
        }

        public override bool Perform(GoapAgent agent, double delta)
        {
            throw new System.NotImplementedException();
        }
    }
}