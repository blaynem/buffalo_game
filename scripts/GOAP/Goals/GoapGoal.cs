using Buffalobuffalo.scripts.GOAP.Agents;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public abstract partial class GoapGoal
    {
        /// <summary>
        /// Action Cost<br/>
        /// If you need to do a calculation on the Priority, override the GetCost.
        /// </summary>
        protected abstract int Priority { get; }
        /// <summary>
        /// This indicates if the action should be considered or not.
        /// </summary>
        public abstract bool IsValid(GoapAgent agent);
        /// <summary>
        /// The desired state we need to reach for goal to be completed.<br/>
        /// I.e. Actions "effects" will apply these conditions.
        /// </summary>
        /// <returns>Dictionary of Conditions and object</returns>
        public abstract ConditionDict DesiredState { get; }

        /// <summary>
        /// Action Cost<br/>
        /// Should be >= 1.
        /// </summary>
        // TODO: May need to add the blackboard back in for the priority calc.
        public virtual int GetPriority()
        {
            return Priority;
        }

        /// <summary>
        /// Returns the goals name.
        /// </summary>
        /// <returns></returns>
        public virtual string GetGoalName()
        {
            return GetType().Name;
        }

        public static GoapGoal CreateByName(string goal_name, Variant arg)
        {
            return goal_name.ToLower() switch
            {
                "buildfirepit" => new BuildFirePit(),
                "pickupitem" => CreatePickUpItemGoal(arg),
                "takeinthesights" => new TakeInTheSightsGoal(),
                _ => null,
            };
        }
        
        // Helper method for cases that require more complex logic
        private static GoapGoal CreatePickUpItemGoal(Variant target_item)
        {
            var goal = new PickUpItemGoal();
            goal.SetTargetItem((GodotObject) target_item);
            return goal;
        }
    }
}