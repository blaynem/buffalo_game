class_name Enemy
extends CharacterBody3D

@onready var nameplate: Nameplate = $Nameplate
@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var state_machine: EnemyStateMachine = %StateMachine

@export var move_speed := 3.0
@export var enemy_can_move := false
@export var nameplate_name := "Trent"

@export_group("Vision Ranges")
## Radius before the enemy stops chasing the player.
## Essentially the "tether" range.
@export var chase_radius := 20.0;
## The radius in which the enemy will detect the player.
@export var detection_radius := 20.0;
## Radius in which the enemy will no longer move closer to the Player.
@export var follow_radius := 5.0;

var goal_manager: EnemyGoalManager

func _ready() -> void:
	set_collisions();
	state_machine.initial_state.Transitioned.connect(on_child_transition)

# This is the connect method of Transitioned, the new_state is a Dictionary of {name, class}
func on_child_transition(state: EnemyState, new_state: Dictionary) -> void:
	nameplate.text = new_state.name
	pass;

func set_collisions() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.enemy], [
		CollisionMap.world, # dont fall through world
		CollisionMap.player, # run into player
		CollisionMap.enemy, # run into enemy
		CollisionMap.item_interactable, # allow clicking interactable items
	])

func set_goal_manager(_goal_manager: EnemyGoalManager) -> void:
	goal_manager = _goal_manager

func get_current_goal() -> EnemyGoal:
	if goal_manager:
		var current_goal := goal_manager.get_current_goal();
		if current_goal:
			nameplate.text = current_goal.name
		return current_goal
	return null;

func move_towards_location(location: Vector3) -> void:
	if !enemy_can_move:
		return;
	var goalDirection := location - global_position;
	
	velocity = goalDirection.normalized() * move_speed
	move_and_slide()

func _process(delta: float) -> void:
	pass;

func _physics_process(delta: float) -> void:
	pass;
