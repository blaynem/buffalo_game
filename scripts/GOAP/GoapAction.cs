using System;

/// The base action that all GoapActions will inherit from.

namespace Buffalobuffalo.scripts.GOAP
{
    public abstract partial class GoapAction
    {
        protected Func<double, bool> performCallback;
        /// <summary>
        /// Takes in a cb that will be fired when the action is attempted via GOAP.<br/>
        /// Func takes in the delta, returns true if action is completed.
        /// </summary>
        /// <param name="action_callback"></param>
        public GoapAction(Func<double, bool> action_callback) {
            performCallback = action_callback;
        }
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
        public abstract ConditionDict GetEffects();
        /// <summary>
        /// Action Cost<br/>
        /// Should be >= 1.
        /// </summary>
        public abstract int GetCost(GoapAgent agent);
        /// <summary>
        /// This indicates if the action should be considered or not.
        /// </summary>
        public virtual bool IsValid { get; } = true;

        /// <summary>
        /// Action implementation called on every loop.
        /// 
        /// "delta" is the time in seconds since last loop. <br/>
        ///
        /// Returns a bool determining if the perform completed or not.
        /// </summary>
        public virtual bool Perform(double delta) {
            // The callback should return a boolean, so we can convert it to one.
            return performCallback(delta);
        }
        

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