using Buffalobuffalo.scripts.GOAP.Agents;

namespace Buffalobuffalo.scripts.GOAP.Goals
{
    public abstract partial class GoapGoal
    {
        /// <summary>
        /// Action Cost<br/>
        /// If you need to do a calculation on the Priority, override the GetCost.
        /// </summary>
        protected abstract int Priority {get;}
        /// <summary>
        /// This indicates if the action should be considered or not.
        /// </summary>
        public abstract bool IsValid(GoapAgent agent);
        /// <summary>
        /// The desired state we need to reach for goal to be completed.<br/>
        /// I.e. Actions "effects" will apply these conditions.
        /// </summary>
        /// <returns>Dictionary of Conditions and object</returns>
        public abstract ConditionDict DesiredState {get;}

        /// <summary>
        /// Action Cost<br/>
        /// Should be >= 1.
        /// </summary>
        // TODO: May need to add the blackboard back in for the priority calc.
        public virtual int GetPriority() {
            return Priority;
        }

        /// <summary>
        /// Returns the goals name.
        /// </summary>
        /// <returns></returns>
        public virtual string GetGoalName() {
            return GetType().Name;
        }
    }
}