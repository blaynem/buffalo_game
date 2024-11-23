namespace Buffalobuffalo.scripts.GOAP
{
    public abstract partial class GoapGoal
    {
        /// <summary>
        /// Returns the goals name.
        /// </summary>
        /// <returns></returns>
        public abstract string GetGoalName();
        /// <summary>
        /// This indicates if the action should be considered or not.
        /// </summary>
        public abstract bool IsValid(GoapAgent agent);
        /// <summary>
        /// Action Cost<br/>
        /// Should be >= 1.
        /// </summary>
        public abstract int Priority();
        /// <summary>
        /// The desired state we need to reach for goal to be completed.<br/>
        /// I.e. Actions "effects" will apply these conditions.
        /// </summary>
        /// <returns>Dictionary of Conditions and object</returns>
        public abstract ConditionDict GetDesiredState();
    }
}