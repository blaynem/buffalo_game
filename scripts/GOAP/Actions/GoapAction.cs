using Buffalobuffalo.scripts.GOAP.Agents;

namespace Buffalobuffalo.scripts.GOAP.Actions
{
    public abstract partial class GoapAction
    {
        /// <summary>
        /// The cost of the Action.<br/>
        /// If you need to do a calculation on the Cost, override the GetCost.
        /// </summary>
        protected abstract int Cost { get; }
        /// <summary>
        /// Requirements for action to be completed. <br/>
        /// Example: { has_wood: true }
        /// </summary>
        public abstract ConditionDict Preconditions { get; }
        /// <summary>
        /// What conditions this action satisfies <br/>
        ///
        /// Example:
        /// {
        ///   "has_wood": true
        /// }
        /// </summary>
        public abstract ConditionDict Effects { get; }
        /// <summary>
        /// This indicates if the action should be considered or not.
        /// </summary>
        public virtual bool IsValid { get; } = true;

        /// <summary>
        /// Action Cost<br/>
        /// Should be >= 1.
        /// </summary>
        public virtual int GetCost(GoapAgent agent)
        {
            return Cost;
        }

        /// <summary>
        /// Action implementation called on every loop.
        /// "agent" is the NPC using the AI
        /// "delta" is the time in seconds since last loop. <br/>
        ///
        /// Returns true when the task is complete.
        /// </summary>
        public abstract bool Perform(GoapAgent agent, double delta);

        /// <summary>
        /// Returns the actions name.
        /// </summary>
        /// <returns></returns>
        public virtual string GetActionName()
        {
            return GetType().Name;
        }
    }
}