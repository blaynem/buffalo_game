/// I.e. An actions "effects", or a goals "desired_state". The value can be a bool, int, etc.
global using ConditionDict = System.Collections.Generic.Dictionary<Buffalobuffalo.scripts.GOAP.Condition, object>;

namespace Buffalobuffalo.scripts.GOAP
{
    public static class ConditionsProvider
    {
        public static ConditionDict CloneConditions(ConditionDict original)
        {
            var clone = new ConditionDict();
            foreach (var kvp in original)
            {
                clone[kvp.Key] = kvp.Value;
            }
            return clone;
        }
    }
    /// <summary>
    /// Possible conditions, or states, that can be applied to an Agent.<br/><br/>
    /// </summary>
    public enum Condition
    {
        HasItemInHand,
        WantsToViewSights,
        CompletedHike,
    }
}