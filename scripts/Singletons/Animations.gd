extends Node
# List of available animations in the Farming pack

# Enum to represent each animation
enum Human {
	T_POSE = 0xA00,
	IDLE = 0xA01,
	RUN = 0xA02,
	WALK = 0xA03,
	JUMP = 0xA04,
	LEFT_STRAFE_WALK = 0xA05,
	LEFT_STRAFE_RUN = 0xA06,
	LEFT_TURN_90 = 0xA07,
	LEFT_TURN = 0xA08,
	RIGHT_STRAFE_WALK = 0xA09,
	RIGHT_STRAFE_RUN = 0xA010,
	RIGHT_TURN_90 = 0xA11,
	RIGHT_TURN = 0xA12,
}

enum Buffalo {
	RESET= 0xB00,
	WALK = 0xB01,
	RUN = 0xB02,
	IDLE = 0xB03,
	YEET = 0xB04,
	JUMP = 0xB05,
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
	Buffalo.RESET: "RESET",
	Buffalo.WALK: "walk",
	Buffalo.IDLE: "idle",
	Buffalo.YEET: "yeet",
	Buffalo.RUN: "run",
	Buffalo.JUMP: "jump"
}

func get_human_animation_name(animation: Human) -> String:
	return String(animation_names.get(animation))

func get_buffalo_animation_name(animation: Buffalo) -> String:
	return String(animation_names.get(animation))
