using System.Collections.Generic;

namespace Buffalobuffalo.scripts.GOAP
{
    public class Blackboard {
        private readonly Dictionary<string, object> state = new();
        public Blackboard() {}

        public object GetValue(string key) {
            if (state == null) return null;

            if (state.TryGetValue(key, out var val)) {
                return val;
            }
            return null;
        }

        public void AddValue(string key, object val) {
            state.Add(key, val);
        }

        public void Clear() {
            state.Clear();
        }
    }
    public partial class GoapAgentBrain
    {
        /// <summary>
        /// The action planner for the given Agent. Includes all available actions.
        /// </summary>
        private GoapActionPlanner action_planner;
        private GoapAgent agent;
        private Blackboard blackboard = new();
        public GoapGoal current_goal {get; private set;}
        private List<GoapAction> current_plan;
        private int current_plan_step = 0;

        public GoapAgentBrain(GoapAgent _agent)
        {
            agent = _agent;
            action_planner = new GoapActionPlanner(agent)
            {
                available_actions = agent.AvailableActions
            };
        }

        /// <summary>
        /// Forces a new goal to be found.
        /// </summary>
        public void ResetCurrentGoal() {
            current_goal = null;
            current_plan = null;
            current_plan_step = 0;
        }

        /// <summary>
        /// Updates the available actions, then looks for a new action_plan.
        /// </summary>
        public void UpdateAvailableActions() {
            action_planner.available_actions = agent.AvailableActions;

            if (current_goal != null) {
                HandleGoalChange(current_goal);
            }
        }

        /// <summary>
        /// On every loop this script checks if the current goal is still
        /// the highest priority. if it's not, it requests a new plan
        /// for the new high priority goal.
        /// </summary>
        /// <param name="delta">Returns the current goal, otherwise null.</param>  
        public void Process(double delta)
        {
            var goal = GetBestGoal();
            if (goal == null) return;

            if (current_goal == null || goal != current_goal)
            {
                current_goal = goal;
                HandleGoalChange(goal);
            }
            else
            {
                FollowPlan(delta);
            }
        }

        /// <summary>
        /// On goal change, we need to do a few things like clearing the blackboard and gettin a new plan.
        /// </summary>
        private void HandleGoalChange(GoapGoal _goal) {
            blackboard.Clear();
            var target_location_prop = _goal.GetType().GetProperty("target_location");
            if (target_location_prop != null) {
                blackboard.AddValue("target_location", target_location_prop.GetValue(_goal));
            }

            current_plan = action_planner.GetNewPlan(_goal, blackboard);
            current_plan_step = 0;
        }

        /// <summary>
        /// Gets the best goal
        /// </summary>
        /// <returns>Returns a goal if one can be completed, and null if there are none.</returns>
        private GoapGoal GetBestGoal()
        {
            GoapGoal best_goal = null;

            foreach (GoapGoal goal in agent.AvailableGoals)
            {
                // If goal isn't valid, no need to consider it.
                if (goal.IsValid(agent) == false) continue;
                // Check for priority shifts
                if (best_goal == null || goal.GetPriority(agent) > best_goal?.GetPriority(agent))
                {
                    best_goal = goal;
                }
            }
            return best_goal;
        }

        /// <summary>
        /// Executes plan. This function is called on every game loop.
        /// "plan" is the current list of actions, and delta is the time since last loop. <br/>
        ///
        /// Every action exposes a function called perform, which will return true when
        /// the job is complete, so the agent can jump to the next action in the list.
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="delta"></param>
        private void FollowPlan(double delta)
        {
            if (current_plan.Count == 0) return;

            var is_step_complete = current_plan[current_plan_step].Perform(delta);
            if (is_step_complete && current_plan_step < current_plan.Count - 1)
            {
                current_plan_step += 1;
            }
        }
    }
}