using Buffalobuffalo.scripts.GOAP.Agents;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class TakeInTheSightsAction : GoapAction
    {
        protected override int Cost => 1;
        public override ConditionDict Preconditions => new() { };

        public override ConditionDict Effects => new(){
            {Condition.WantsToViewSights, false}
        };

        public override bool Perform(GoapAgent agent, double delta)
        {
            GD.Print("CANT PERFORM CUZ AINT IMPLEMENTATIONED!");

            throw new System.NotImplementedException();
        }
    }
}