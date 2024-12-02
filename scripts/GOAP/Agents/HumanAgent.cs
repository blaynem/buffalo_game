using Godot;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    // TODO: Maybe alter the potential actions depending on the personality.
    public partial class HumanAgent : GoapAgent
    {
        public HumanAgent() {}
        public override void _Ready()
        {
             AvailableActions = new(){
                new Actions.PickUpItem(PickUpItemCb),
                new Actions.CompleteHike(CompleteHikeCb),
                new Actions.ViewRelic(ViewRelicCb)
            };

            var item = (Node3D) GetTree().GetFirstNodeInGroup("TempItem");
            var relic = (Node3D) GetTree().GetFirstNodeInGroup("Relic");

            AvailableGoals =  new(){
                new Goals.CompleteHike(),
                new Goals.PickUpItemGoal(item),
                new Goals.ViewRelic(relic),
            };

            base._Ready();
        }

        // Each agent kinda needs to create their own build that maps the action to the callback.
        public GoapAction CreateAction(string action_name)
        {
            GoapAction built_action = action_name.ToLower() switch
            {
                "pickupitem" => new Actions.PickUpItem(PickUpItemCb),
                "completehike" => new Actions.CompleteHike(CompleteHikeCb),
                _ => null,
            };
            return built_action;
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            
            if (Brain.current_goal?.GetGoalName() == "CompleteHike") {
                Enemy.GDUtils.SetFollowPath(Actor, true);
            } else {
                Enemy.GDUtils.SetFollowPath(Actor, false);
            }
        }

        private bool CompleteHikeCb(double delta)
        {
            // We basically want this to always be false, since they never complete the hike.
            // TODO: Complete the hike somehow?
            return false;
            // ApplyEffectsToState(Actions.CompleteHike.StaticEffects);
        }

        private bool ViewRelicCb(double delta)
        {
            if (Brain.current_goal is not Goals.ViewRelic viewRelicGoal) {
                GD.PrintErr("Somehow in the ViewRelic Action with incorrect goal.");
                return false;
            };
            var target_relic = viewRelicGoal.target_relic;
            var distance = Actor.GlobalPosition.DistanceSquaredTo(target_relic.GlobalPosition);
            if (distance < 20) {
                // Make some sort of call to the relic???
                // Ensure it was actually viewed
                ApplyEffectsToState(Actions.ViewRelic.StaticEffects);
                return true;
            }

            SetTargetLocation(target_relic.GlobalPosition);

            return false;
        }

        private bool PickUpItemCb(double delta)
        {
            if (Brain.current_goal is not Goals.PickUpItemGoal pickupItemGoal) {
                GD.PrintErr("Somehow in the PickUpItem Action with incorrect goal.");
                return false;
            };

            var target = pickupItemGoal.target_item;
            var distance = Actor.GlobalPosition.DistanceSquaredTo(target.GlobalPosition);
            if (distance < 5) {
                // TODO: Should fire the animation in here for pickup, and ensure the item is actually picked up.
                // Probably need an animation timer 
                // Attempt to pick up the item
                Items.CarriableEnemyGoalItem.EnemyInteract(target, Actor);
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