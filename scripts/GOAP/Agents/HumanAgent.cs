using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Goals;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    // TODO: Maybe alter the potential actions depending on the personality.
    public partial class HumanAgent : GoapAgent
    {
        public HumanAgent(){}
        protected override GoapAction[] DefineDefaultActions()
        {
            return new GoapAction[]{
                // new Actions.TakeInTheSights(),
                new Actions.CollectWood(TempPerform),
                new Actions.CollectAxe(TempPerform),
                new Actions.ChopWood(TempPerform),
                new Actions.BuildFirePit(TempPerform),
            };
        }

        protected override GoapGoal[] DefineDefaultGoals()
        {
            return new GoapGoal[]{
                // new Goals.TakeInTheSightsGoal(),
                new Goals.BuildFirePit(),
            };
        }

        public static bool TempPerform(double delta) {
            GD.Print("Performing action...");
            return true;
        }
    }
}