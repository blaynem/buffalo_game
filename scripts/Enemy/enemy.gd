class_name Enemy
extends CharacterBody3D

@onready var nameplate: Nameplate = $Nameplate
@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var state_machine: EnemyStateMachine = %StateMachine
@onready var enemy_item_interaction_area: Area3D = %EnemyItemInteractionArea
@onready var nav_agent: NavigationAgent3D = $NavigationAgent3D

@export var enemy_can_move: bool = true
@export var goal_manager: EnemyGoalManager;
@export var personality: EnemyPersonality;

var display_name: String
var display_status: String;
var target_location: Vector3;
# Only use nav when the map is ready
var nav_map_ready := false;

func _ready() -> void:
	display_name = personality.display_name;
	update_nameplate()
	set_collisions();
	NavigationServer3D.map_changed.connect(_on_navigation_map_ready)
	state_machine.initial_state.Transitioned.connect(_on_child_transition)

func _on_navigation_map_ready(_map: RID) -> void:
	nav_map_ready = true;

# This is the connect method of Transitioned, the new_state is a Dictionary of {name, class}
func _on_child_transition(state: EnemyState, new_state: Dictionary) -> void:
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

func set_target_location(location: Vector3) -> void:
	target_location = location

func _process(delta: float) -> void:
	pass;

func _physics_process(delta: float) -> void:
	if enemy_can_move && nav_map_ready:
		var direction := Vector3()
		
		nav_agent.target_position = target_location
		direction = (nav_agent.get_next_path_position() - global_position).normalized()
		velocity = velocity.lerp(direction * personality.move_speed, personality.acceleration * delta)
		
		# Rotate to face the movement direction
		if direction != Vector3.ZERO:
			var target_rotation_y := atan2(direction.x, direction.z)
			rotation.y = lerp_angle(rotation.y, target_rotation_y, 0.1)
	
		move_and_slide()
