/// I.e. An actions "effects", or a goals "desired_state". The value can be a bool, int, etc.
global using ConditionDict = System.Collections.Generic.Dictionary<Buffalobuffalo.scripts.GOAP.Condition, object>;

namespace Buffalobuffalo.scripts.GOAP
{
    public static class ConditionsProvider
    {
        /// <summary>
        /// Contains a list of the default conditions, basically all of our potential conditions.
        /// </summary>
        /// <returns></returns>
        public static ConditionDict GetDefaultConditions()
        {
            return new ConditionDict
            {
                { Condition.HasItemInHand, false },
                { Condition.HungerLevel, 0 },
                { Condition.WantsToViewSights, false },
                { Condition.HasWood, false },
                { Condition.HasFirepit, false },
            };
        }

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
    /// 
    /// Note: For any Condition added, we should add it to the GetDefaultConditions method.
    /// </summary>
    public enum Condition
    {
        HasItemInHand,
        HungerLevel,
        WantsToViewSights,
        HasWood,
        HasFirepit,
    }
}