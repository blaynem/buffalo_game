using System;
using System.Collections.Generic;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class TakeInTheSightsAction : GoapAction
    {
        public override string GetActionName()
        {
            return "TakeInTheSights";
        }

        public override int GetCost(GoapAgent agent)
        {
            return 1;
        }

        public override ConditionDict GetEffects()
        {
            return new ConditionDict(){
                {Condition.WantsToViewSights, false}
            };
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
            GD.Print("CANT PERFORM CUZ AINT IMPLEMENTATIONED!");

            throw new System.NotImplementedException();
        }
    }
}