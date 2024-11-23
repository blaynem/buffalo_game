using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Goals;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    public partial class HumanAgent : GoapAgent
    {
        protected override GoapAction[] DefineDefaultActions()
        {
            return new GoapAction[]{
                new TakeInTheSightsAction()
            };
        }

        protected override GoapGoal[] DefineDefaultGoals()
        {
            return new GoapGoal[]{
                new TakeInTheSightsGoal()
            };
        }
    }
}