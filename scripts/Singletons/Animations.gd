extends Node
# List of available animations in the Farming pack

# Enum to represent each animation
enum Human {
	T_POSE,
	IDLE,
	RUN,
	WALK,
	JUMP,
	LEFT_STRAFE_WALK,
	LEFT_STRAFE_RUN,
	LEFT_TURN_90,
	LEFT_TURN,
	RIGHT_STRAFE_WALK,
	RIGHT_STRAFE_RUN,
	RIGHT_TURN_90,
	RIGHT_TURN,
}

var locomotion_base := "people_locomotion_pack/"

# Dictionary to map enum values to StringNames
var animation_names: Dictionary = {
	Human.T_POSE: "t_pose/t_pose",
	Human.IDLE: locomotion_base + "idle",
	Human.RUN: locomotion_base + "running",
	Human.WALK: locomotion_base + "walking",
	Human.JUMP: locomotion_base + "jump",
	Human.LEFT_STRAFE_WALK: locomotion_base + "left_strafe_walking",
	Human.LEFT_STRAFE_RUN: locomotion_base + "left_strafe_run",
	Human.LEFT_TURN_90: locomotion_base + "left_turn_90",
	Human.LEFT_TURN: locomotion_base + "left_turn_180",
	Human.RIGHT_STRAFE_WALK: locomotion_base + "right_strafe_walking",
	Human.RIGHT_STRAFE_RUN: locomotion_base + "right_strafe_run",
	Human.RIGHT_TURN_90: locomotion_base + "right_turn_90",
	Human.RIGHT_TURN: locomotion_base + "right_turn_180",
}

func get_human_animation_name(animation: Human) -> String:
	return String(animation_names.get(animation))
