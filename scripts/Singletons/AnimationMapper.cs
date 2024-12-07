using System;
using System.Collections.Generic;

namespace Buffalobuffalo.scripts.Animation
{
    /// <summary>
    /// All available animation enums, meant to be used in c# land.
    /// If any of these change, please also alter them inside of the gdscript Animations.gd global.
    /// </summary>
    public static class AnimationMapper
    {
        // List of available animations in the Farming pack
        public enum Human
        {
            T_POSE, IDLE, RUN, WALK, JUMP,
            LEFT_STRAFE_WALK, LEFT_STRAFE_RUN, LEFT_TURN_90, LEFT_TURN,
            RIGHT_STRAFE_WALK, RIGHT_STRAFE_RUN, RIGHT_TURN_90, RIGHT_TURN,
        }

        public enum Buffalo
        {
            RESET, WALK, RUN, IDLE, YEET, JUMP,
        }

        private const string LocomotionBase = "people_locomotion_pack/";

        // Dictionary for animation names
        private static readonly Dictionary<object, string> AnimationNames = new(){
            { Human.T_POSE, "t_pose/t_pose" },
            { Human.IDLE, LocomotionBase + "idle" },
            { Human.RUN, LocomotionBase + "running" },
            { Human.WALK, LocomotionBase + "walking" },
            { Human.JUMP, LocomotionBase + "jump" },
            { Human.LEFT_STRAFE_WALK, LocomotionBase + "left_strafe_walking" },
            { Human.LEFT_STRAFE_RUN, LocomotionBase + "left_strafe_run" },
            { Human.LEFT_TURN_90, LocomotionBase + "left_turn_90" },
            { Human.LEFT_TURN, LocomotionBase + "left_turn_180" },
            { Human.RIGHT_STRAFE_WALK, LocomotionBase + "right_strafe_walking" },
            { Human.RIGHT_STRAFE_RUN, LocomotionBase + "right_strafe_run" },
            { Human.RIGHT_TURN_90, LocomotionBase + "right_turn_90" },
            { Human.RIGHT_TURN, LocomotionBase + "right_turn_180" },
            { Buffalo.RESET, "RESET" },
            { Buffalo.WALK, "walk" },
            { Buffalo.IDLE, "idle" },
            { Buffalo.YEET, "yeet" },
            { Buffalo.RUN, "run" },
            { Buffalo.JUMP, "jump" }
        };

        /// <summary>
        /// Allows mapping the animation enum to its string variant.
        /// </summary>
        public static string ToAnimationName(this Enum animation)
        {
            return AnimationNames.TryGetValue(animation, out var name) ? name : null;
        }
    }
}