using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Goals;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    public partial class HumanAgent : GoapAgent
    {
        protected override GoapAction[] DefineDefaultActions()
        {
            return new GoapAction[]{
                // new Actions.TakeInTheSightsAction(),
                new Actions.CollectWood(),
                new Actions.CollectAxe(),
                new Actions.ChopWood(),
                new Actions.BuildFirePit(),
            };
        }

        protected override GoapGoal[] DefineDefaultGoals()
        {
            return new GoapGoal[]{
                // new Goals.TakeInTheSightsGoal(),
                new Goals.BuildFirePit(),
            };
        }
    }
}