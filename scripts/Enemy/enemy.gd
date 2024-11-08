class_name Enemy
extends CharacterBody3D

@onready var nameplate := $Nameplate
@onready var item_hold_position: Marker3D = $CarryObjetMarker

@export var move_speed := 3.0
## The enemies Goal
@export var all_goals: Array[EnemyGoal] = [];
var current_goal_index: int = 0;

@export_group("Vision Ranges")
## Radius before the enemy stops chasing the player.
## Essentially the "tether" range.
@export var chase_radius := 20.0;
## The radius in which the enemy will detect the player.
@export var detection_radius := 20.0;
## Radius in which the enemy will no longer move closer to the Player.
@export var follow_radius := 5.0;

func _ready() -> void:
	SignalBus.EnemyGoalStatusChange.connect(handle_enemy_goal_status_change)

func handle_enemy_goal_status_change(goal_id: int, status: bool) -> void:
	# When a goals status is changed, the enemy should check through its list and see what should be completed next. Then update the current_goal_index.
	# For example, if the goal is to obtain an object and deliver it, the enemy must first pick up the object, then move towards the delivery zone. If they drop the object, they must obtain it once more.
	print("--handle enemy: ", goal_id, status)
	for i in all_goals.size():
		var goal := all_goals[i];
		if !goal.is_goal_completed():
			print("--new goal:", goal, " ", i)
			current_goal_index = i;
			return
	current_goal_index = -1;
	pass;

func get_current_goal() -> EnemyGoal:
	if current_goal_index < 0:
		return null;
	return all_goals[current_goal_index]

func handleNameplate() -> void:
	var screen_pos := get_viewport().get_camera_3d().unproject_position(global_transform.origin + Vector3(0, 2, 0))
	nameplate.position = screen_pos

func _process(delta: float) -> void:
	handleNameplate()

func _physics_process(delta: float) -> void:
	pass;
