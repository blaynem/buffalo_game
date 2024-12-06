using System;
using System.Collections.Generic;
using System.Linq;
using Buffalobuffalo.scripts.Animation;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    // TODO: Maybe alter the potential actions depending on the personality.
    public partial class HumanAgent : GoapAgent
    {
        public AnimationHandler animationHandler;
        public HumanAgent() {}
        public override void _Ready()
        {
             AvailableActions = new(){
                new Actions.PickUpItem(PickUpItemCb),
                new Actions.CompleteHike(CompleteHikeCb),
                new Actions.ViewRelic(ViewRelicCb),
                new Actions.CompleteActivityZone(CompleteActivityZoneCb),
            };

            var relics = GetTree().GetNodesInGroup("Relic");
            var zones = GetTree().GetNodesInGroup("EnemyActivity");

            Random random = new();
            var relic_amt = random.Next(1, relics.Count);
            var zone_amt = random.Next(1, zones.Count);

            // shuffle em and grab the first few items.
            relics.Shuffle();
            zones.Shuffle();

            var availableGoals = new List<GoapGoal>(){
                new Goals.CompleteHike()
            };
            
            var relic_to_visit = relics.Take(relic_amt);
            var zones_to_visit = zones.Take(zone_amt);
            
            foreach (Node node in relic_to_visit) {
                var goal = new Goals.ViewRelic((Node3D) node);
                availableGoals.Add(goal);
            };
            foreach (Node node in zones_to_visit) {
                var goal = new Goals.CompleteActivityZone((Node3D) node);
                availableGoals.Add(goal);
            };

            AvailableGoals = availableGoals;
            base._Ready();
        }

        public void AttachAnimationHandler(AnimationPlayer animationPlayer) {
            animationHandler = new(animationPlayer);
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

        private bool CompleteActivityZoneCb(double delta)
        {
            if (Brain.current_goal is not Goals.CompleteActivityZone activityZone) {
                GD.PrintErr("Somehow in the ViewRelic Action with incorrect goal.");
                return false;
            };

            var distance = Actor.GlobalPosition.DistanceSquaredTo(activityZone.target_location);
            if (distance < 5) {
                var animation_name = activityZone.interaction_animation;
                if (animationHandler.current_animation.name != animation_name) {
                    animationHandler.PlayWithCallback(animation_name, () => {
                        ApplyEffectsToState(Actions.CompleteActivityZone.StaticEffects);
                        activityZone.CompleteGoal();
                    });
                }
                return activityZone.IsCompleted();
            }

            
            SetTargetLocation(activityZone.target_location);
            return false;
        }

        private bool ViewRelicCb(double delta)
        {
            if (Brain.current_goal is not Goals.ViewRelic viewRelicGoal) {
                GD.PrintErr("Somehow in the ViewRelic Action with incorrect goal.");
                return false;
            };
            var target_relic = viewRelicGoal.target_relic;
            var distance = Actor.GlobalPosition.DistanceSquaredTo(target_relic.GlobalPosition);
            if (distance < 10) {
                // TODO: Edit this to be some sort of alternate than just turning lol
                var animation_name = "people_locomotion_pack/left_turn_180";
                if (animationHandler.current_animation.name != animation_name) {
                    animationHandler.PlayWithCallback(animation_name, () => {
                        ApplyEffectsToState(Actions.ViewRelic.StaticEffects);
                        viewRelicGoal.CompleteGoal();
                    });
                }
                return viewRelicGoal.IsCompleted();
            }

            SetTargetLocation(target_relic.GlobalPosition);

            return false;
        }

        private bool PickUpItemCb(double delta)
        {
            var item_pickedup = false;
            if (Brain.current_goal is not Goals.PickUpItemGoal pickupItemGoal) {
                GD.PrintErr("Somehow in the PickUpItem Action with incorrect goal.");
                return false;
            };

            var target = pickupItemGoal.target_item;
            var distance = Actor.GlobalPosition.DistanceSquaredTo(target.GlobalPosition);
            if (distance < 5) {
                var animation_name = "people_locomotion_pack/jump";
                if (animationHandler.current_animation.name != animation_name) {
                    animationHandler.PlayWithCallback(animation_name, () => {
                        // Probably need an animation timer 
                        // Attempt to pick up the item
                        Items.CarriableEnemyGoalItem.EnemyInteract(target, Actor);
                        // Ensure it actually got looted
                        if (GetInventoryManager().Held_item == target) {
                            ApplyEffectsToState(Actions.PickUpItem.StaticEffects);
                            item_pickedup = true;
                        }
                    });

                    return item_pickedup;
                }
            }

            SetTargetLocation(target.GlobalPosition);

            return item_pickedup;
        }
    }
}