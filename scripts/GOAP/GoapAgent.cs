using Godot;

namespace Buffalobuffalo.scripts.GOAP
{
    public partial class GoapAgent : Node
    {
        /// <summary>
        /// The action planner for the given Agent. Includes all available actions.
        /// </summary>
        private readonly GoapActionPlanner action_planner;
        [Export]
        private GoapAgent agent;
        [Export]
        private GoapGoal[] goals;
        private GoapGoal current_goal;
        private GoapAction[] current_plan;
        private int current_plan_step = 0;

        public GoapAgent(GoapAgent _agent, GoapGoal[] _goals, GoapAction[] available_actions)
        {
            agent = _agent;
            goals = _goals;

            var planner = new GoapActionPlanner(_agent);
            planner.SetActions(available_actions);
            action_planner = planner;
        }

        public GoapAgent() {}

        public override void _Ready()
        {
            GD.Print("wow test");
        }

        /// <summary>
        /// On every loop this script checks if the current goal is still
        /// the highest priority. if it's not, it requests a new plan
        /// for the new high priority goal.
        /// </summary>
        /// <param name="delta"></param>        
        public override void _Process(double delta)
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

            foreach (GoapGoal goal in goals)
            {
                if (goal.IsValid() && best_goal == null || goal.Priority() > best_goal.Priority())
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
            if (current_plan.Length == 0) return;

            var is_step_complete = current_plan[current_plan_step].Perform(agent, delta);
            if (is_step_complete && current_plan_step < current_plan.Length - 1)
            {
                current_plan_step += 1;
            }
        }
    }
}