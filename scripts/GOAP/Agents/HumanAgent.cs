using System.Collections.Generic;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    // TODO: Maybe alter the potential actions depending on the personality.
    public partial class HumanAgent : GoapAgent
    {
        // Items that are required to complete the current quests.
        private readonly List<Node3D> goal_items = new();
        public HumanAgent() {}
        public override void _Ready()
        {
             AvailableActions = new(){
                new Actions.PickUpItem(PickUpItemCb),
                new Actions.CompleteHike(CompleteHikeCb),
            };

            var item = (Node3D) GetTree().GetFirstNodeInGroup("TempItem");
            goal_items.Add(item);
            AvailableGoals =  new(){
                new Goals.CompleteHike(),
                new Goals.PickUpItemGoal(item),
            };

            base._Ready();
        }

        // Each agent kinda needs to create their own build that maps the action to the callback.
        public override GoapAction CreateAction(string action_name)
        {
            GoapAction built_action = action_name.ToLower() switch
            {
                "pickupitem" => new Actions.PickUpItem(PickUpItemCb),
                "completehike" => new Actions.CompleteHike(CompleteHikeCb),
                _ => null,
            };
            return built_action;
        }

        private bool CompleteHikeCb(double delta)
        {
            ApplyEffectsToState(Actions.TakeInTheSights.StaticEffects);
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
                    var _effects = Actions.PickUpItem.StaticEffects;
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