using Godot;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public partial class PickUpItemAction : GoapAction
    {
        private GodotObject target_item = null;

        public PickUpItemAction(GodotObject _item) {
            target_item = _item;
        }


        public override string GetActionName()
        {
            return "PickUpItem";
        }

        public override int GetCost(GoapAgent agent)
        {
            return 1;
        }

        public override ConditionDict GetEffects()
        {
            return new ConditionDict() {
                { Condition.HasItemInHand, target_item }
            };
        }

        public override ConditionDict GetPreconditions()
        {
            return new ConditionDict() {
                { Condition.HasItemInHand, false }
            };
        }

        public override bool IsValid()
        {
            return target_item != null;
        }

        public override bool Perform(GoapAgent agent, double delta)
        {
            if (target_item == null)
            {
                GD.PrintErr("No target item set for PickUpItem action!");
                return false;
            }
            GD.Print("----can't pickup because no method implemented");
            throw new System.NotImplementedException();
        }
    }
}