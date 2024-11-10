class_name Enemy
extends CharacterBody3D

@onready var nameplate: Nameplate = $Nameplate
@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var state_machine: EnemyStateMachine = %StateMachine
@onready var enemy_item_interaction_area: Area3D = %EnemyItemInteractionArea

@export var enemy_can_move: bool = true
@export var goal_manager: EnemyGoalManager;
@export var personality: EnemyPersonality;

var display_name: String
var display_status: String;

func _ready() -> void:
	display_name = personality.display_name;
	update_nameplate()
	set_collisions();
	state_machine.initial_state.Transitioned.connect(on_child_transition)

# This is the connect method of Transitioned, the new_state is a Dictionary of {name, class}
func on_child_transition(state: EnemyState, new_state: Dictionary) -> void:
	display_status = new_state.name
	update_nameplate()
	pass;

func update_nameplate() -> void:
	nameplate.update_content(display_name)
	nameplate.update_sub_content(display_status)

func set_collisions() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.enemy], [
		CollisionMap.world, # dont fall through world
		CollisionMap.player, # run into player
		CollisionMap.enemy, # run into enemy
		CollisionMap.item_interactable, # allow clicking interactable items
	])
	CollisionMap.set_collisions(enemy_item_interaction_area, [CollisionMap.enemy], [
		CollisionMap.item_interactable, # allow clicking interactable items
	])

func get_current_goal() -> EnemyGoal:
	if goal_manager:
		var current_goal := goal_manager.get_current_goal();
		if current_goal:
			nameplate.update_sub_content(current_goal.name)
		return current_goal
	return null;

func move_towards_location(location: Vector3) -> void:
	if enemy_can_move:
		var goalDirection := location - global_position;
		
		velocity = goalDirection.normalized() * personality.move_speed
		move_and_slide()

func _process(delta: float) -> void:
	pass;

func _physics_process(delta: float) -> void:
	pass;
