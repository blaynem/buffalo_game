using System.Collections.Generic;
using System.Linq;
using Buffalobuffalo.scripts.Items;
using Godot;

/// The base action that all GoapActions will inherit from.

namespace Buffalobuffalo.scripts.GOAP
{
    public class AgentState
    {
        private readonly ConditionDict state = new();

        /// <summary>
        /// Returns true if the condition is present and met, otherwise false.
        /// </summary>
        /// <param name="condition">Condition we want to match.</param>
        /// <param name="val">Value of the condition.</param>
        /// <returns></returns>
        public bool MatchesCondition(Condition condition, object val)
        {
            if (state.TryGetValue(condition, out var curr_val))
            {
                // NOTE: We may need to change this equality for different values.
                return curr_val.Equals(val);
            }
            return false;
        }

        public void UpdateState(Condition condition, object val)
        {
            state[condition] = val;
        }
    }
    /// <summary>
    /// The base class of the Agent.
    /// </summary>
    public abstract partial class GoapAgent : Node
    {
        public CharacterBody3D Actor { get; private set; }
        public AgentState State { get; set; } = new();
        public List<GoapGoal> AvailableGoals { get; protected set; } = new();
        public List<GoapAction> AvailableActions { get; protected set; } = new();
        public GoapAgentBrain Brain { get; private set; }

        public GoapAgent() {}

        public override void _Ready()
        {
            Actor = (CharacterBody3D)GetParent();
            Brain = new GoapAgentBrain(this);
            GD.Print("ok Agent loaded");
        }

        /// <summary>
        /// On every physics process cycle we should run our thinky brain!
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(double delta)
        {
            Brain.Process(delta);
        }

        public void SetAvailableGoals(GoapGoal[] _new_goals)
        {
            AvailableGoals = _new_goals.ToList();
            Brain.ResetCurrentGoal();
        }

        public void SetAvailableActions(GoapAction[] _new_actions)
        {
            AvailableActions = _new_actions.ToList();
            Brain.UpdateAvailableActions();
        }

        protected void ApplyEffectsToState(ConditionDict effects)
        {
            foreach ((Condition condition, object value) in effects)
            {
                State.UpdateState(condition, value);
            }
        }

        public InventoryManager GetInventoryManager()
        {
            return Enemy.GDUtils.GetInventoryManager(Actor);
        }

        protected void SetTargetLocation(Vector3 location)
        {
            Enemy.GDUtils.SetTargetLocation(Actor, location);
        }
    }
};