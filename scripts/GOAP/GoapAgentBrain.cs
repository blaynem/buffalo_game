using System.Collections.Generic;
using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Agents;
using Buffalobuffalo.scripts.GOAP.Goals;
using Godot;

namespace Buffalobuffalo.scripts.GOAP
{
    public partial class GoapAgentBrain
    {
        /// <summary>
        /// The action planner for the given Agent. Includes all available actions.
        /// </summary>
        private GoapActionPlanner action_planner;
        private GoapAgent agent;
        private GoapGoal current_goal;
        private List<GoapAction> current_plan;
        private int current_plan_step = 0;

        public GoapAgentBrain(GoapAgent _agent)
        {
            agent = _agent;
            action_planner = new GoapActionPlanner(agent);

            UpdateAvailableActions();
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
            current_plan = action_planner.GetNewPlan(current_goal);
            current_plan_step = 0;
        }

        /// <summary>
        /// On every loop this script checks if the current goal is still
        /// the highest priority. if it's not, it requests a new plan
        /// for the new high priority goal.
        /// </summary>
        /// <param name="delta"></param>  
        public void Process(double delta)
        {
            var goal = GetBestGoal();
            if (current_goal == null || goal != current_goal)
            {
                // TODO: Do we want a blackboard? Do we just use the agent state?
                // GoapBlackboard blackboard = new GoapBlackboard();

                current_goal = goal;
                current_plan = action_planner.GetNewPlan(goal);
                current_plan_step = 0;
            }
            else
            {
                FollowPlan(delta);
            }
        }

        /// <summary>
        /// Gets the best goal
        /// </summary>
        /// <returns></returns>
        private GoapGoal GetBestGoal()
        {
            GoapGoal best_goal = null;

            foreach (GoapGoal goal in agent.AvailableGoals)
            {
                if (goal.IsValid(agent) && best_goal == null || goal.GetPriority() > best_goal?.GetPriority())
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