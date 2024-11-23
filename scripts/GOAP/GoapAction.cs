using Godot;

namespace Buffalobuffalo.scripts.GOAP
{
    public abstract partial class GoapAction
    {
        /// <summary>
        /// Returns the actions name.
        /// </summary>
        /// <returns></returns>
        public abstract string GetActionName();
        /// <summary>
        /// This indicates if the action should be considered or not.
        /// </summary>
        public abstract bool IsValid();
        /// <summary>
        /// Action Cost<br/>
        /// Should be >= 1.
        /// </summary>
        public abstract int GetCost(GoapAgent agent);
        /// <summary>
        /// Requirements for action to be completed. <br/>
        /// Example: { has_wood: true }
        /// </summary>
        public abstract ConditionDict GetPreconditions();
        /// <summary>
        /// What conditions this action satisfies <br/>
        ///
        /// Example:
        /// {
        ///   "has_wood": true
        /// }
        /// </summary>
        public abstract ConditionDict GetEffects();
        /// <summary>
        /// Action implementation called on every loop.
        /// "agent" is the NPC using the AI
        /// "delta" is the time in seconds since last loop. <br/>
        ///
        /// Returns true when the task is complete.
        /// </summary>
        public abstract bool Perform(GoapAgent agent, double delta);
    }
}