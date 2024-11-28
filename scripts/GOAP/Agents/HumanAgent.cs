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
                new Actions.TakeInTheSights(TakeInTheSightsCb),
                new Actions.CollectWood(CollectWoodCb),
                new Actions.CollectAxe(CollectAxeCb),
                new Actions.ChopWood(ChopWoodCb),
                new Actions.BuildFirePit(BuildFirePitCb),
            };
        }

        protected override GoapGoal[] DefineDefaultGoals()
        {
            return new GoapGoal[]{
                new Goals.TakeInTheSightsGoal(),
                new Goals.BuildFirePit(),
            };
        }

        
        public static bool TakeInTheSightsCb(double delta) {
            return true;
        }
        
        public static bool CollectWoodCb(double delta) {
            return true;
        }
        
        public static bool CollectAxeCb(double delta) {
            return true;
        }
        
        public static bool ChopWoodCb(double delta) {
            return true;
        }
        
        public static bool BuildFirePitCb(double delta) {
            return true;
        }
    }
}