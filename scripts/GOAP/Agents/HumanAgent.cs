using System.Collections.Generic;
using Buffalobuffalo.scripts.GOAP.Actions;
using Buffalobuffalo.scripts.GOAP.Goals;
using BuildFirePit = Buffalobuffalo.scripts.GOAP.Actions.BuildFirePit;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    // TODO: Maybe alter the potential actions depending on the personality.
    public partial class HumanAgent : GoapAgent
    {
        public HumanAgent() { }
        protected override List<GoapAction> DefineDefaultActions()
        {
            return new(){
                new Actions.TakeInTheSights(TakeInTheSightsCb),
                new Actions.CollectWood(CollectWoodCb),
                new Actions.CollectAxe(CollectAxeCb),
                new Actions.ChopWood(ChopWoodCb),
                new Actions.BuildFirePit(BuildFirePitCb),
            };
        }

        protected override List<GoapGoal> DefineDefaultGoals()
        {
            return new(){
                new Goals.TakeInTheSightsGoal(),
                new Goals.BuildFirePit(),
            };
        }

        // Each agent kinda needs to create their own build that maps the action to the callback.
        public override GoapAction CreateAction(string action_name)
        {
            GoapAction built_action = action_name.ToLower() switch
            {
                "buildfirepit" => new BuildFirePit(BuildFirePitCb),
                "pickupitem" => new PickUpItem(PickUpItemCb),
                "takeinthesights" => new TakeInTheSights(TakeInTheSightsCb),
                _ => null,
            };
            return built_action;
        }

        private bool TakeInTheSightsCb(double delta)
        {
            ApplyEffectsToState(TakeInTheSights.Effects);
            return true;
        }

        private bool PickUpItemCb(double delta)
        {
            // TODO: How the fk do we choose which item to pick up here?


            var _effects = PickUpItem.Effects;
            // Set the item thats being held.
            _effects[Condition.HasItemInHand] = "";

            ApplyEffectsToState(_effects);
            return true;
        }

        private bool CollectWoodCb(double delta)
        {
            ApplyEffectsToState(CollectWood.Effects);
            return true;
        }

        private bool CollectAxeCb(double delta)
        {
            ApplyEffectsToState(CollectAxe.Effects);
            return true;
        }

        private bool ChopWoodCb(double delta)
        {
            ApplyEffectsToState(ChopWood.Effects);
            return true;
        }

        private bool BuildFirePitCb(double delta)
        {
            ApplyEffectsToState(BuildFirePit.Effects);
            return true;
        }
    }
}