using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Godot;

namespace Buffalobuffalo.scripts.GOAP
{

    public partial class GoapActionPlanner
    {
        public GoapAction[] available_actions;
        public GoapAgent agent;

        public GoapActionPlanner(GoapAgent _agent)
        {
            agent = _agent;
        }

        /// <summary>
        /// Updates the actions list with potential
        /// </summary>
        /// <param name="available_actions"></param>
        public void SetActions(GoapAction[] _available_actions)
        {
            available_actions = _available_actions;
        }

        /// <summary>
        /// Takes in a goal and determines the plans in order to achieve the goal.
        /// </summary>
        /// <param name="goal"></param>
        /// <returns>A list of planned actions to complete the goal.</returns>
        public GoapAction[] GetNewPlan(GoapGoal goal)
        {
            var desired_state = goal.GetDesiredState();

            if (desired_state.Count <= 0)
                return Array.Empty<GoapAction>();

            return FindBestPlan(goal, desired_state);
        }

        /// <summary>
        /// Given a goal and the desired state, a
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="desired_state"></param>
        /// <returns></returns>
        private GoapAction[] FindBestPlan(GoapGoal goal, ConditionDict desired_state)
        {
            // First we need to build a list of plans to complete the initial goal.
            // Then we find the cheapest plan.

            var root_goal = new GoalAccomplisher(goal, desired_state);
            // TODO: Use the blackboard?
            dynamic blackboard = new ExpandoObject();

            // BuildPlan does alter the `root_goal.children`
            if (BuildPlan(root_goal, blackboard))
            {
                var plans = GetActionsTotalCost(root_goal, blackboard);
                return GetCheapestPlan(plans);
            }

            return Array.Empty<GoapAction>();
        }

        /// <summary>
        /// We recurse through the goal_step, building up a list of actions needed to complete each step.
        /// </summary>
        /// <param name="goal_step"></param>
        /// <returns></returns>
        private bool BuildPlan(GoalAccomplisher step, dynamic blackboard)
        {
            var has_followup = false;
            // If desired state is already met, we exit early.
            if (step.desired_state.Count == 0)
            {
                return true;
            }
            // We're going to mark off each of the pieces from the blackboard
            var current_state = step.CloneDesiredState();
            GD.Print("Action Planner blackboard skipped");
            // todo: finish this check since blackboard doesn't really exit?
            // foreach ((Condition condition, ConditionValue value) in current_state){
            // }
            // If the blackboard has marked it off, we exit.
            if (current_state.Count == 0) return true;


            foreach (GoapAction action in available_actions)
            {
                // Skip this action if it ain't valid.
                if (action.IsValid()) continue;
                var effects = action.GetEffects();
                // Doing annoying clone
                var desired_state = JsonSerializer.Deserialize<ConditionDict>(JsonSerializer.Serialize(current_state));

                // Check if the action should be used
                foreach ((Condition _name, _) in desired_state)
                {
                    // TODO: Handle this correctly for bool / int values. if its an int, it needs to be subtracted or added.
                    if (desired_state[_name].value == effects[_name].value)
                    {
                        desired_state.Remove(_name);

                        var preconditions = action.GetPreconditions();
                        // If there are none then we can return.
                        if (preconditions.Count == 0) return true;
                        // Adds actions preconditions to the desired_state.
                        foreach ((Condition p, ConditionValue pv) in preconditions)
                        {
                            // TODO: Handle if the precondition is already in desired state (bool/int options too)
                            desired_state.Add(p, pv);
                        }

                        // 
                        var new_root_goal = new GoalAccomplisher(action, desired_state);
                        // If building the plan works, then we append it to the current step through the goals.
                        if (BuildPlan(new_root_goal, blackboard))
                        {
                            _ = step.children.Append(action);
                            has_followup = true;
                        }
                    }
                }
            }
            return has_followup;
        }

        // Todo: implement this Plan thing to the GetActionsTotalCost potentially.
        class Plan {
            public GoapAction[] actions;
            public int cost;
            public Plan(GoapAction[] _actions, int _cost) {
                actions = _actions;
                cost = _cost;
            }
        }

        private List<GoalAccomplisher> GetActionsTotalCost(GoalAccomplisher accomplisher, dynamic blackboard)
        {
            var plans = new List<GoalAccomplisher>();

            if (accomplisher.children.Length <= 0)
            {
                dynamic plan = new ExpandoObject();
                var plan_actions = Array.Empty<GoapAction>();
                _ = plan_actions.Append(accomplisher.action);
                plan.actions = plan_actions;
                plans.Add(plan);
            }

            foreach (GoapAction child in accomplisher.children)
            {
                var plan_holder = new GoalAccomplisher(accomplisher.action, accomplisher.desired_state);
                foreach (GoalAccomplisher child_plan in GetActionsTotalCost(plan_holder, blackboard))
                {
                    _ = child_plan.children.Append(accomplisher.action);
                    child_plan.total_cost += accomplisher.action.GetCost(agent);
                    plans.Add(child_plan);
                }
            }

            return plans;
        }

        private GoapAction[] GetCheapestPlan(List<GoalAccomplisher> plans) {
            GoalAccomplisher best_plan = null;
            foreach (GoalAccomplisher p in plans) {
                GD.Print("TODO: Print Plan", p);
                if (best_plan == null || p.total_cost < best_plan.total_cost) {
                    best_plan = p;
                }
            }
            return best_plan.children;
        }

        // Lil box to store the goal in and have it typed.
        private class GoalAccomplisher
        {
            // Will either be a goal or action.
            public GoapAction action;
            public GoapGoal goal;
            // State required to complete the goal
            public ConditionDict desired_state;
            // The very first accomplisher in the tree will be a goal, not an action.
            // reeeee!
            public bool isAction = true;
            public int total_cost = 0;
            public GoapAction[] children = Array.Empty<GoapAction>();
            public void SetTotalCost(int cost)
            {
                total_cost = cost;
            }
            public GoalAccomplisher(GoapAction _action, ConditionDict _state)
            {
                action = _action;
                desired_state = _state;
                isAction = true;
            }
            public GoalAccomplisher(GoapGoal _goal, ConditionDict _state)
            {
                goal = _goal;
                action = null;
                desired_state = _state;
                isAction = false;
            }
            public ConditionDict CloneDesiredState()
            {
                var obj2 = JsonSerializer.Deserialize<ConditionDict>(JsonSerializer.Serialize(desired_state));
                return obj2;
            }
        }
    }

}