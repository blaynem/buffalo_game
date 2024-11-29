using System.Collections.Generic;
using System.Dynamic;
using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Agents;
using Buffalobuffalo.scripts.GOAP.Goals;
using Godot;

namespace Buffalobuffalo.scripts.GOAP
{

    public partial class GoapActionPlanner
    {
        public List<GoapAction> available_actions;
        public GoapAgent agent;

        public GoapActionPlanner(GoapAgent _agent)
        {
            agent = _agent;
            available_actions = agent.AvailableActions;
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
                // Grab the first action from the root goal, since it will always be the task. Though I could be wrong,
                // so i'm going to add a check here just in case.
                if (root_goal.Child_tasks.Count > 1)
                {
                    GD.PrintErr("AYYYYAAAA. Root goal has more child tasks?!");
                }
                var first_action = root_goal.Child_tasks[0];
                Plan _plan = new(first_action);

                var plans = GetActionsTotalCost(_plan, blackboard);
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
            if (accomplisher.Current_state.Count == 0) return true;

            // Let's mark off anything we have in the blackboard for the task, and check if it's completed.
            accomplisher.DoThingWithBlackboard(blackboard);
            if (accomplisher.Current_state.Count == 0) return true;

            var state = ConditionsProvider.CloneConditions(accomplisher.Current_state);
            // Next we loop over all the available actions to see if any of their effects
            // handle the state we want to get to.
            foreach (var action in available_actions)
            {
                var desired_state = ConditionsProvider.CloneConditions(state);
                // Skip this action if it ain't valid.
                if (!action.IsValid) continue;
                var useAction = ActionSatisfiesPartialState(desired_state, action);

                if (useAction)
                {
                    // Applying the action to the accomplisher returns our new state we need to solve for.
                    desired_state = ApplyAction(desired_state, action);

                    TaskAccomplisher child_task = new(action, desired_state);
                    // Apply the preconditions to the child task.
                    child_task.Current_state = ApplyPreconditions(child_task.Current_state, action);

                    // If the child task is completed, it means this action was used and should be included.
                    // If it hasn't, we recursively call the BuildPlan so we can try to build up the state.
                    // If it cannot be built, then the action won't be included as a child task.
                    var child_task_completed = child_task.Current_state.Count == 0;
                    if (child_task_completed || BuildPlan(child_task, blackboard))
                    {
                        accomplisher.AddChildTask(child_task);
                        has_followup = true;
                    }
                }
            }
            return has_followup;
        }

        private List<Plan> GetActionsTotalCost(Plan plan, dynamic blackboard)
        {
            var plans = new List<Plan>();

            // If there are no children, that means we're at the lowest node
            // So set that plan and return.
            if (plan.accomplisher.Child_tasks.Count <= 0)
            {
                GoapAction _action = (GoapAction)plan.accomplisher.Task;
                Plan _plan = new();
                _plan.actions.Add(_action);
                _plan.cost = _action.GetCost(agent);

                plans.Add(_plan);
                return plans;
            }

            // If there are children, we want to recurse through and get the costs,
            // adding the childs cost to the parents.
            foreach (var child in plan.accomplisher.Child_tasks)
            {
                Plan _plan = new(child);
                foreach (Plan child_plan in GetActionsTotalCost(_plan, blackboard))
                {
                    if (plan.accomplisher.Task is GoapAction action)
                    {
                        child_plan.actions.Add(action);
                        child_plan.cost += action.GetCost(agent);
                    }
                    plans.Add(child_plan);
                }
            }

            return plans;
        }

        public static ConditionDict ApplyAction(ConditionDict _curr_state, GoapAction action)
        {
            var updated_state = ConditionsProvider.CloneConditions(_curr_state);
            // Loop through the current_state of the task and see if
            // any of the actions `effects` match up.
            foreach ((Condition state_name, object state_val) in _curr_state)
            {
                if (action.GetEffects().TryGetValue(state_name, out var effect_change))
                {
                    // If the effect is the same as the desired state, we can remove it.
                    if (effect_change is bool && state_val.Equals(effect_change))
                    {
                        updated_state.Remove(state_name);
                    }
                    else
                    {
                        // TODO: If string check
                        // TODO: If int check
                        GD.Print("AddAction: state change other than bool.");
                    }
                }
            }

            return updated_state;
        }

        public static ConditionDict ApplyPreconditions(ConditionDict _curr_state, GoapAction action)
        {
            if (action.Preconditions.Count == 0)
            {
                return _curr_state;
            }

            var updated_state = ConditionsProvider.CloneConditions(_curr_state);
            foreach ((var p_name, var p_val) in action.Preconditions)
            {
                if (p_val is bool)
                {
                    updated_state.Add(p_name, p_val);
                }
                else
                {
                    // TODO: Handle preconditions: int, object, etc
                    GD.Print("---AddAction: Precondition add attempt other than bool");
                    // updated_state.Add(p_name, p_val);
                }
            }

            return updated_state;
        }

        public static bool ActionSatisfiesPartialState(ConditionDict _curr_state, GoapAction action)
        {
            // Loop through the current_state of the task and see if
            // any of the actions effects match up.
            foreach ((var state_name, var state_val) in _curr_state)
            {
                if (action.GetEffects().TryGetValue(state_name, out var effect_change))
                {
                    // If the effect is the same as the desired state, we can remove it.
                    if (effect_change is bool && state_val.Equals(effect_change))
                    {
                        return true;
                    }
                    else
                    {
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

        private static List<GoapAction> GetCheapestPlan(List<Plan> plans)
        {
            Plan best_plan = null;
            foreach (var p in plans)
            {
                if (best_plan == null || p.cost < best_plan.cost)
                {
                    best_plan = p;
                }
            }
            return best_plan.actions;
        }

        // Lil box to hold some plans.
        private class Plan
        {
            // The accomplisher
            public TaskAccomplisher accomplisher;
            // The actions required to complete the task
            public List<GoapAction> actions = new();
            public int cost = 0;

            public Plan() { }
            public Plan(TaskAccomplisher _accomplisher)
            {
                accomplisher = _accomplisher;
            }
        }

        // Lil box to store the goal / action we want to accomplish.
        private class TaskAccomplisher
        {
            public List<TaskAccomplisher> Child_tasks { get; private set; } = new();
            // State required to complete the task.
            public ConditionDict Current_state { get; set; }
            /// The actions needed to accomplish this task.
            private readonly ConditionDict desired_end_state;
            // A checklist of what state is completed by actions in the actions_list.
            // Task that the actions_list is going to accomplish.
            // Will either be the Action or Goal, we don't really need this.
            // Just for debugging easier.
            public object Task { get; private set; }
            // Our task may be the parent most Goal or the Actions to complete it.
            public bool IsGoal
            {
                get => Task is GoapGoal;
            }
            public TaskAccomplisher(object _task, ConditionDict _state)
            {
                desired_end_state = _state;
                Current_state = _state;
                Task = _task;
            }

            public void AddChildTask(TaskAccomplisher child)
            {
                Child_tasks.Add(child);
            }

            public void DoThingWithBlackboard(object blackboard)
            {
                var blackboard_checklist = ConditionsProvider.CloneConditions(desired_end_state);
                GD.Print("Action Planner blackboard skipped");
                // todo: finish this check since blackboard doesn't really exit?
                // foreach ((Condition condition, object value) in current_state){
                // }
                // If the blackboard has marked it off, we exit.
            }
        }
    }
}