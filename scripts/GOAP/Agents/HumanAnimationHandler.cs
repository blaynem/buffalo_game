using Buffalobuffalo.scripts.Animation;
using Godot;

namespace Buffalobuffalo.scripts.GOAP.Agents
{
    public partial class HumanAnimationHandler : AnimationHandler {
        public CharacterBody3D Actor { get; private set; }
        public HumanAnimationHandler(AnimationPlayer anim_player, CharacterBody3D actor) : base(anim_player)
        {
            Actor = actor;
            Init();
        }

        private void Init() {
            // We set these because theres a bug where if not walking, then idling.
            // And if idling inside hte AnimationHandler itself, then `CanMove` is set to false...
            // So rather than fix it, hehe we have this patch!
            // TODO: Fix this bug.
            Play(AnimationMapper.Human.WALK);
            Actor.Velocity = Vector3.Forward; // Anything besides Vector3.Zero.
        }

        /// <summary>
        /// Should be fired on every physics process
        /// </summary>
        public void HandlePhysicsProcess() {
            if (IsRagdolled()) return;
            // If no animation is playing, we fall back to these basics.
            if (!IsAnimationPlaying()) {
                if (Actor.Velocity == Vector3.Zero) {
                    Play(AnimationMapper.Human.IDLE);
                }
                Play(AnimationMapper.Human.WALK);
            }
        }

        private bool IsRagdolled() {
            var ragdoll_handler = (Node) Actor.Get("ragdoll_handler");
            return (bool) ragdoll_handler.Get("is_ragdolled");
        }
    }
}