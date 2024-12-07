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
        public HumanAnimationHandler AnimationHandler;
        public HumanAgent() {}
        public override void _Ready()
        {
            // Ensure we set up the goals and actions before calling the base ready.
            SetupActionsAndGoals();
            base._Ready();
        }

        private void SetupActionsAndGoals() {
            AvailableActions = new(){
                new Actions.FindSafety(FindSafetyCb),
                new Actions.CalmDown(CalmDownCb),
                new Actions.PickUpItem(PickUpItemCb),
                new Actions.CompleteHike(CompleteHikeCb),
                new Actions.ViewRelic(ViewRelicCb),
                new Actions.CompleteActivityZone(CompleteActivityZoneCb),
            };

            var availableGoals = new List<GoapGoal>(){
                new Goals.CompleteHike(),
                new Goals.StaySafe()
            };

            var relics = GetTree().GetNodesInGroup("Relic");
            var zones = GetTree().GetNodesInGroup("EnemyActivity");

            Random random = new();
            var relic_amt = random.Next(1, relics.Count);
            var zone_amt = random.Next(1, zones.Count);

            // shuffle em and grab the first few items.
            relics.Shuffle();
            zones.Shuffle();
            
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
            AnimationHandler = new(animationPlayer, Actor);
        }

        /// <summary>
        /// When a player is spotted, we will do some things!
        /// </summary>
        public void HandlePlayerSpotted() {
            State.UpdateState(Condition.IsScared, true);
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
            AnimationHandler.HandlePhysicsProcess();
            
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

        private bool CalmDownCb(double delta)
        {
            // Calm themselves down once within safety range.
            State.UpdateState(Condition.IsScared, false);
            // TODO: Fire a different animation here that doesn't loop, then remove the `Stop` call.
            AnimationHandler.Stop(); // Stop the animation so it's them walking again.
            return false;
        }

        private bool FindSafetyCb(double delta)
        {
            var player = (CharacterBody3D) GetTree().GetFirstNodeInGroup("Player");
            var distance = Actor.GlobalPosition.DistanceSquaredTo(player.GlobalPosition);
            // If they're out of range, the saftey is true.
            if (distance >= 130) {
                return true;
            }

            // We want them to run for their lives!
            if (AnimationHandler.current_animation.name != AnimationMapper.Human.RUN.ToAnimationName()) {
                AnimationHandler.Play(AnimationMapper.Human.RUN);
            }
            // TODO: Figure out better target, also adjust the movement speed.
            SetTargetLocation(new Vector3(10, 0, 10));
            return false;
        }

        private bool CompleteActivityZoneCb(double delta)
        {
            if (Brain.current_goal is not Goals.CompleteActivityZone activityZone) {
                GD.PrintErr("Somehow in the ViewRelic Action with incorrect goal.");
                return false;
            };

            var distance = Actor.GlobalPosition.DistanceSquaredTo(activityZone.target_location);
            if (distance <= 1) {
                var animation_name = activityZone.interaction_animation;
                if (AnimationHandler.current_animation.name != animation_name) {
                    AnimationHandler.PlayWithCallback(animation_name, () => {
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
            var distance = Actor.GlobalPosition.DistanceSquaredTo(viewRelicGoal.target_location);
            if (distance <= 1) {
                // TODO: Edit this to be some sort of alternate than just turning lol
                var animation_name = "people_locomotion_pack/left_turn_180";
                if (AnimationHandler.current_animation.name != animation_name) {
                    AnimationHandler.PlayWithCallback(animation_name, () => {
                        viewRelicGoal.CompleteGoal();
                    });
                }
                return viewRelicGoal.IsCompleted();
            }

            SetTargetLocation(viewRelicGoal.target_location);

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
            if (distance <= 1) {
                var animation_name = "people_locomotion_pack/jump";
                if (AnimationHandler.current_animation.name != animation_name) {
                    AnimationHandler.PlayWithCallback(animation_name, () => {
                        // Probably need an animation timer 
                        // Attempt to pick up the item
                        Items.CarriableEnemyGoalItem.EnemyInteract(target, Actor);
                        // Ensure it actually got looted
                        if (GetInventoryManager().Held_item == target) {
                            item_pickedup = true;
                        }
                    });

                    return item_pickedup;
                }
            }

            SetTargetLocation(target.GlobalPosition);

            return item_pickedup;
        }

        public Items.InventoryManager GetInventoryManager()
        {
            return Enemy.GDUtils.GetInventoryManager(Actor);
        }

        protected void SetTargetLocation(Vector3 location)
        {
            Enemy.GDUtils.SetTargetLocation(Actor, location);
        }
    }
}