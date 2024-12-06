using Buffalobuffalo.scripts.GOAP.Goals;
using Godot;

/// The base action that all GoapActions will inherit from.

namespace Buffalobuffalo.scripts.GOAP
{
    public abstract partial class GoapGoal
    {
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
        /// Priority for the given gGoal<br/>
        /// Should be >= 1.
        /// </summary>
        // TODO: May need to add the blackboard back in for the priority calc.
        public abstract int GetPriority(GoapAgent agent);

        /// <summary>
        /// Returns the goals name.
        /// </summary>
        /// <returns></returns>
        public virtual string GetGoalName()
        {
            return GetType().Name;
        }
    }
}