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
                new Actions.TakeInTheSights(TakeInTheSights),
                new Actions.CollectWood(CollectWood),
                new Actions.CollectAxe(CollectAxe),
                new Actions.ChopWood(ChopWood),
                new Actions.BuildFirePit(BuildFirePit),
            };
        }

        protected override GoapGoal[] DefineDefaultGoals()
        {
            return new GoapGoal[]{
                new Goals.TakeInTheSightsGoal(),
                new Goals.BuildFirePit(),
            };
        }

        public static bool TakeInTheSights(double delta) {
            return true;
        }
        public static bool CollectWood(double delta) {
            return true;
        }
        public static bool CollectAxe(double delta) {
            return true;
        }
        public static bool ChopWood(double delta) {
            return true;
        }
        public static bool BuildFirePit(double delta) {
            return true;
        }
    }
}