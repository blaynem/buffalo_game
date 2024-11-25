using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Agents;
using Buffalobuffalo.scripts.GOAP.Goals;
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
            SetActions(agent.AvailableActions);
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
        public List<GoapAction> GetNewPlan(GoapGoal goal)
        {
            if (goal.DesiredState.Count <= 0)
                return new();

            return FindBestPlan(goal, goal.DesiredState);
        }

        /// <summary>
        /// Given a goal and the desired state, a
        /// </summary>
        /// <param name="goal"></param>
        /// <param name="required_state"></param>
        /// <returns></returns>
        private List<GoapAction> FindBestPlan(GoapGoal goal, ConditionDict required_state)
        {
            // First we need to build a list of plans to complete the initial goal.
            // Then we find the cheapest plan.

            var root_goal = new TaskAccomplisher(goal, required_state);
            // TODO: Use the blackboard?
            dynamic blackboard = new ExpandoObject();

            // BuildPlan does alter the `root_goal` action list.
            if (BuildPlan(root_goal, blackboard))
            {
                Plan root_plan = new(root_goal);
                var plans = GetActionsTotalCost(root_plan, blackboard);
                return GetCheapestPlan(plans);
            }

            return new();
        }

        /// <summary>
        /// Given a Task, we want to build a plan to complete the task.
        /// We recurse through the goal_step, building up a list of actions needed to complete each step.
        /// </summary>
        /// <returns>True if the plan was able to be built successfully.</returns>
        private bool BuildPlan(TaskAccomplisher accomplisher, dynamic blackboard)
        {
            var has_followup = false;
            // If tasks desired_state is already met, we exit early.
            if (accomplisher.TaskCompleted()) return true;

            // Let's mark off anything we have in the blackboard for the task, and check if it's completed.
            accomplisher.DoThingWithBlackboard(blackboard);
            if (accomplisher.TaskCompleted()) return true;

            // Next we loop over all the available actions to see if any of their effects
            // handle the state we want to get to.
            foreach (var action in available_actions)
            {
                // Skip this action if it ain't valid.
                if (!action.IsValid) continue;
                var useAction = accomplisher.ActionSatisfiesPartialState(action);

                if (useAction) {
                    // Applying the action to the accomplisher returns our new state we need to solve for.
                    var desired_child_state = accomplisher.ApplyAction(action);

                    TaskAccomplisher child_task = new(action, desired_child_state);
                    // Apply the preconditions to the child task.
                    child_task.ApplyPreconditions(action);

                    // If the child task is completed, it means this action was used and should be included.
                    // If it hasn't, we recursively call the BuildPlan so we can try to build up the state.
                    // If it cannot be built, then the action won't be included as a child task.
                    var task_completed = child_task.TaskCompleted();
                    if (child_task.TaskCompleted() || BuildPlan(child_task, blackboard)) {
                        accomplisher.AddChildTask(child_task);
                        has_followup = true;
                    }
                }
            }
            return has_followup;
        }

        private class Plan {
            public object action;
            public List<GoapAction> actions = new();
            public List<GoapAction> children = new();
            public int cost = 0;

            public Plan(GoapAction _action, int _cost){
                action = _action;
                actions.Add(_action);
                cost = _cost;
            }
            public Plan(TaskAccomplisher accomplisher) {
                action = accomplisher.task;
                foreach (var child in accomplisher.child_tasks) {
                    if (child.task is GoapAction action) {
                        children.Add(action);
                    }
                }
            }
        }

        private List<Plan> GetActionsTotalCost(Plan p, dynamic blackboard)
        {
            var plans = new List<Plan>();

            // If there are no children, we set the cost and push it to the plans.
            if (p.children.Count <= 0)
            {
                GoapAction _action = (GoapAction) p.action;
                Plan _plan = new(_action, _action.GetCost(agent));
                plans.Add(_plan);
                return plans;
            }

            // If there are children, we want to recurse through and get the costs,
            // adding the childs cost to the parents.
            foreach (var child in p.children)
            {
                Plan _plan = new(child, child.GetCost(agent));
                foreach (Plan child_plan in GetActionsTotalCost(_plan, blackboard))
                {
                    if (p.action is GoapAction action && child_plan.action is GoapAction c_action) {
                        child_plan.actions.Add(action);
                        child_plan.cost += action.GetCost(agent);
                    }
                    plans.Add(child_plan);
                }
            }

            return plans;
        }

        private static List<GoapAction> GetCheapestPlan(List<Plan> plans) {
            Plan best_plan = null;
            foreach (var p in plans) {
                if (best_plan == null || p.cost < best_plan.cost) {
                    best_plan = p;
                }
            }
            return best_plan.actions;
        }

        // Lil box to store the goal / action we want to accomplish.
        private class TaskAccomplisher
        {
            public List<TaskAccomplisher> child_tasks {get;private set;}= new();
            // State required to complete the task.
            private ConditionDict current_state;
            /// The actions needed to accomplish this task.
            private readonly ConditionDict desired_end_state;
            // A checklist of what state is completed by actions in the actions_list.
            // Task that the actions_list is going to accomplish.
            // Will either be the Action or Goal, we don't really need this.
            // Just for debugging easier.
            public object task {get;private set;}
            public TaskAccomplisher(object _task, ConditionDict _state)
            {
                desired_end_state = _state;
                current_state = _state;
                task = _task;
            }

            public bool TaskCompleted() {
                // An action is considered completed if all of the desired_end_state is met via the current_state.
                // TODO: Do we need a better way of checking this TaskCompleted?
                if (desired_end_state != current_state) {
                    GD.Print("Task completed called. States mismatched.");
                }
                return current_state.Count == 0;
            }

            public void AddChildTask(TaskAccomplisher child) {
                child_tasks.Add(child);
            }

            public void DoThingWithBlackboard(object blackboard) {
                var blackboard_checklist = ConditionsProvider.CloneConditions(desired_end_state);
                GD.Print("Action Planner blackboard skipped");
                // todo: finish this check since blackboard doesn't really exit?
                // foreach ((Condition condition, object value) in current_state){
                // }
                // If the blackboard has marked it off, we exit.
            }

            public ConditionDict ApplyAction(GoapAction action) {
                // When an action gets added we should apply the `effects`
                // As well as apply the `preconditions`.

                var updated_state = ConditionsProvider.CloneConditions(current_state);
                // Loop through the current_state of the task and see if
                // any of the actions `effects` match up.
                foreach ((Condition state_name, object state_val) in current_state) {
                    if (action.Effects.TryGetValue(state_name, out var effect_change)) {
                        // If the effect is the same as the desired state, we can remove it.
                        if (effect_change is bool && state_val.Equals(effect_change)) {
                            updated_state.Remove(state_name);
                        } else {
                            // TODO: If string check
                            // TODO: If int check
                            GD.Print("AddAction: state change other than bool.");
                        }
                    }
                }

                current_state = updated_state;
                return current_state;
            }

            public ConditionDict ApplyPreconditions(GoapAction action) {
                if (action.Preconditions.Count == 0) {
                    return current_state;
                }

                var updated_state = ConditionsProvider.CloneConditions(current_state);
                foreach ((var p_name, var p_val) in action.Preconditions) {
                    if (p_val is bool) {
                        updated_state.Add(p_name, p_val);
                    } else {
                        // TODO: Handle preconditions: int, object, etc
                        GD.Print("---AddAction: Precondition add attempt other than bool");
                        // updated_state.Add(p_name, p_val);
                    }
                }

                current_state = updated_state;
                return current_state;
            }

            public bool ActionSatisfiesPartialState(GoapAction action) {
                // Loop through the current_state of the task and see if
                // any of the actions effects match up.
                foreach ((var state_name, var state_val) in current_state) {
                    if (action.Effects.TryGetValue(state_name, out var effect_change)) {
                        // If the effect is the same as the desired state, we can remove it.
                        if (effect_change is bool && state_val.Equals(effect_change)) {
                            return true;
                        } else {
                            // TODO: If string check
                            // TODO: If int check
                            GD.Print("ActionSatisfiesPartialState: state change other than bool.");
                            return true;
                        }
                    }
                }
                
                // If the action satisfies a portion of the current_state or not.
                return false;
            }
        }
    }
}