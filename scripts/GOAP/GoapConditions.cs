/// I.e. An actions "effects", or a goals "desired_state"
global using ConditionDict = System.Collections.Generic.Dictionary<Buffalobuffalo.scripts.GOAP.Condition, Buffalobuffalo.scripts.GOAP.ConditionValue>;

namespace Buffalobuffalo.scripts.GOAP
{
    /// <summary>
    /// Possible conditions, or states, that can be applied to an Agent.
    /// </summary>
    public enum Condition
    {
        HasItemInHand,
        HungerLevel,
    }
    /// <summary>
    /// Allows stricter typing for Conditions.
    /// </summary>
    public class ConditionValue
    {
        public bool isBool { get; }
        public bool isInt { get; }
        public object value { get; }

        public ConditionValue(bool boolValue)
        {
            isBool = true;
            value = boolValue;
            isInt = false;
        }

        public ConditionValue(int _intValue)
        {
            isBool = false;
            value = false;
            isInt = true;
        }

    }
}