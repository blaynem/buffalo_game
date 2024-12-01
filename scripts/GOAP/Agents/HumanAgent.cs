using System.Collections.Generic;
using Buffalobuffalo.scripts.GOAP.Actions;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    // TODO: Maybe alter the potential actions depending on the personality.
    public partial class HumanAgent : GoapAgent
    {
        // Items that are required to complete the current quests.
        private List<Node3D> goal_items = new();
        public HumanAgent() { }
        protected override List<GoapAction> DefineDefaultActions()
        {
            return new(){
                new TakeInTheSights(TakeInTheSightsCb),
                new PickUpItem(PickUpItemCb),
            };
        }

        protected override List<GoapGoal> DefineDefaultGoals()
        {
            var item = GetTree().GetFirstNodeInGroup("TempItem");
            goal_items.Add((Node3D) item);
            return new(){
                // new Goals.TakeInTheSightsGoal(),
                new Goals.PickUpItemGoal(item),
            };
        }

        // Each agent kinda needs to create their own build that maps the action to the callback.
        public override GoapAction CreateAction(string action_name)
        {
            GoapAction built_action = action_name.ToLower() switch
            {
                "pickupitem" => new PickUpItem(PickUpItemCb),
                "takeinthesights" => new TakeInTheSights(TakeInTheSightsCb),
                _ => null,
            };
            return built_action;
        }

        private bool TakeInTheSightsCb(double delta)
        {
            ApplyEffectsToState(TakeInTheSights.StaticEffects);
            return true;
        }

        private bool PickUpItemCb(double delta)
        {
            var target = goal_items[0];
            var distance = Actor.GlobalPosition.DistanceTo(target.GlobalPosition);
            if (distance < 5) {
                // TODO: Should fire the animation in here for pickup, and ensure the item is actually picked up.
                // Probably need an animation timer 
                // Attempt to pick up the item
                target.Call("enemy_interact", Actor);
                // Ensure it actually got looted
                if (GetInventoryManager().Held_item == target) {
                    var _effects = PickUpItem.StaticEffects;
                    // Set the item thats being held.
                    _effects[Condition.HasItemInHand] = target;

                    ApplyEffectsToState(_effects);
                    return true;
                }
            }

            SetTargetLocation(target.GlobalPosition);

            return false;
        }
    }
}