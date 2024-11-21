using System;
using System.Collections.Generic;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class TakeInTheSights : GoapAction
    {
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
            return true;
        }

        public override bool Perform(GoapAgent agent, double delta)
        {
            GD.Print();

            throw new System.NotImplementedException();
        }
    }
}