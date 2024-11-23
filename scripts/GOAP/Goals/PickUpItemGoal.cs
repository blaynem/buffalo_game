
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public partial class PickUpItemGoal : GoapGoal
    {
        private GodotObject target_item = null;
        
        public PickUpItemGoal(GodotObject item) {
            target_item = item;
        }

        public override ConditionDict GetDesiredState()
        {
            ConditionDict conditionDict = new()
            {
                { Condition.HasItemInHand, target_item }
            };

            return conditionDict;
        }

        public override string GetGoalName()
        {
            return "PickUpItem";
        }

        public override bool IsValid(GoapAgent agent)
        {
            return target_item != null;
        }

        public override int Priority()
        {
            return 1;
        }
    }
};