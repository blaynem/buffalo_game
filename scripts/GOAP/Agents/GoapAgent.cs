using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Goals;
using Godot;


namespace Buffalobuffalo.scripts.GOAP.Agents
{
    public partial class AgentState {
        private readonly ConditionDict state;

        public AgentState(ConditionDict initState) {
            state = initState;
        }

        internal bool HasState(Condition condition, object val) {
            if (state.TryGetValue(condition, out var curr_val)) {
                // NOTE: We may need to change this equality for different values.
                return curr_val.Equals(val);
            }
            return false;
        }

        internal void UpdateState(Condition condition, object val) {
            state[condition] = val;
        }
    }
    /// <summary>
    /// The base class of the Agent.
    /// </summary>
    public abstract partial class GoapAgent : Node
    {
        private AgentState State;
        public CharacterBody3D Actor { get; private set; }
        public GoapGoal[] AvailableGoals { get; private set; }
        public GoapAction[] AvailableActions { get; private set; }
        public GoapAgentBrain Brain { get; private set; }
        protected abstract GoapAction[] DefineDefaultActions();
        protected abstract GoapGoal[] DefineDefaultGoals();

        public GoapAgent(){}

        public override void _Ready()
        {
            Actor = (CharacterBody3D)GetParent();

            AvailableActions = DefineDefaultActions();
            AvailableGoals = DefineDefaultGoals();
            State = DefineDefaultState();
            
            Brain = new GoapAgentBrain(this);
            SetTargetLocation(new Vector3((float)10.0,(float)1.0,(float)10.0));

            GD.Print("ok Agent loaded");
        }

        /// <summary>
        /// On every physics process cycle we should run our thinky brain!
        /// </summary>
        /// <param name="delta"></param>
        public override void _PhysicsProcess(double delta) {
            Brain.Process(delta);
        }

        /// <summary>
        /// If the state is desired.
        /// </summary>
        public bool StateHasDesire(Condition condition, object val) {
            return State.HasState(condition, val);
        }

        public void UpdateState(Condition condition, object val) {
            State.UpdateState(condition, val);
        }

        protected virtual void SetAvailableGoals(GoapGoal[] _new_goals) {
            AvailableGoals = _new_goals;
        }

        protected virtual void SetAvailableActions(GoapAction[] _new_actions) {
            AvailableActions = _new_actions;
            Brain.UpdateAvailableActions();
        }

        protected virtual AgentState DefineDefaultState()
        {
            var init_conditions = ConditionsProvider.GetDefaultConditions();
            return new AgentState(init_conditions);
        }

        public void SetTargetLocation(Vector3 location) {
            Actor.Call("set_target_location", location);
        }
    }
};