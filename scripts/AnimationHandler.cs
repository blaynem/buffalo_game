using System;
using System.Security.Cryptography;
using Godot;

namespace Buffalobuffalo.scripts.Animation {
    // Lifecycle of an animation state is Added -> Started -> Completed. Or Added -> Started -> Interrupted
    public enum AnimationState {
        Added,
        Started,
        Completed,
        Interrupted,
    }
    public class Animation {
        public AnimationState state = AnimationState.Added;
        public string name;
        public Action callback;
        public Animation(string _name, Action cb) {
            name = _name;
            callback = cb;
        }
    }
    // This should be how we start / cancel animations going forward so we can keep track of some events easier.
    // We need to know when an animation is running, and allow a call back when its finished/
    public partial class AnimationHandler : Resource{
        public Animation last_animation = null;
        public Animation current_animation = null;
        private readonly AnimationPlayer animationPlayer;

        public AnimationHandler(AnimationPlayer anim_player) {
            animationPlayer = anim_player;

            // Order of these fires:
            // 1. CurrenAnimationChanged
            // 2. AnimationStarted
            // 3. AnimationFinished.

            // Fired when the animation `.play` is called
            anim_player.AnimationStarted += OnAnimationStarted;
            // Is fired when the animation is set, and when `.stop` is called on it.
            anim_player.CurrentAnimationChanged += OnAnimationChanged;
            // Fired when the animation is done playing. Does not finish if the animation is looping.
            anim_player.AnimationFinished += OnAnimationFinished;
        }

        private void OnAnimationFinished(StringName animName)
        {
            GD.Print("--OnAnimationFinished: ", animName);
            if (current_animation == null) return;
            current_animation.state = AnimationState.Completed;
            // If we have a callback to fire, fire it.
            current_animation.callback?.Invoke();
        }

        // Fired when the animation `.play` is called
        private void OnAnimationStarted(StringName animName)
        {
            GD.Print("--OnAnimationStarted: ", animName);
            if (current_animation == null) return;
            if (current_animation.state == AnimationState.Added) {
                current_animation.state = AnimationState.Started;
            }
        }

        // Fired when the animation is set, and when `.stop` is called on the player.
        private void OnAnimationChanged(string animName) {
            // TODO: Determine if on change is even needed?
            GD.Print("--OnAnimationChanged: ", animName);
            // Note: If animName is empty, then the animation was interrupted.
            if (animName == "") {
                current_animation.state = AnimationState.Interrupted;
            }
        }

        /// <summary>
        /// Returns whether the actor can move during the given animation being played or not.
        /// </summary>
        /// <returns></returns>
        public bool CanMove() {
            if (current_animation == null) return true;
            if (current_animation.name == "people_locomotion_pack/walking") {
                return true;
            }
            return false;
        }

        public bool IsAnimationPlaying() {
            return current_animation.state == AnimationState.Started;
        }

        public void Play(string animation_name, bool force_stop = false) {
            PlayWithCallback(animation_name, null, force_stop);
        }

        public void PlayWithCallback(string animation_name, Action callback, bool force_stop = false) {
            if (force_stop) {
                Stop();
            }

            // If the current animation and new animation name are the same,
            // AND the animation wasn't interrupted, then we dont have anything to do.
            if (animation_name == current_animation?.name && current_animation?.state != AnimationState.Interrupted) {
                return;
            }

            // If it's not completed, then we interrupted it.
            if (current_animation != null && current_animation.state != AnimationState.Completed) {
                current_animation.state = AnimationState.Interrupted;
            }
            // Set the last_animation as the current one.
            last_animation = current_animation;
            // Finally update the current animation being played.
            current_animation = new(animation_name, callback);

            // Play the animation
            animationPlayer.Play(animation_name);
        }

        public void Stop() {
            animationPlayer.Stop();
        }
    }
}