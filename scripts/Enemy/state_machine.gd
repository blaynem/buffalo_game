class_name EnemyStateMachine
extends Node

@onready var enemy: Enemy = get_parent()
var current_state: EnemyState
var states: Dictionary = {}
var potential_states := EnemyStateMap.new()

func _ready() -> void:
	# Loop through all children of the StateMachine node
	for child in get_children():
		if child is EnemyState:
			states[child.name.to_lower()] = child
	
	SignalBus.EnemyStateMachineTransitioned.connect(on_child_transition)
	var init_state: String = potential_states.idle.name;
	var s := states.get(init_state.to_lower()) as EnemyState
	handle_enter_state(s)
	_stupid_status_display_hack_pls_delete(init_state)

# TODO: This is hack to set the status to the initial
func _stupid_status_display_hack_pls_delete(_status: String) -> void:
	enemy.display_status = _status

func _process(delta: float) -> void:
	if current_state:
		current_state.update(delta);
	
func _physics_process(delta: float) -> void:
	if current_state:
		current_state.physics_update(delta);

# On entering a new state we do all of these.
func handle_enter_state(new_state: EnemyState) -> void:
	new_state.enter()
	current_state = new_state

# This is the connect method of Transitioned, the new_state is a Dictionary of {name, class}
func on_child_transition(enemy_id: int, prev_state: EnemyState, new_state: Dictionary) -> void:
	if enemy_id == enemy.get_instance_id():
		# If the prev_state mismatches with the current_state,
		# then something funky is going on so we ignore it.
		if prev_state != current_state:
			return;
			
		var _new_state := states.get(new_state.name.to_lower()) as EnemyState
		if !_new_state:
			return
			
		if current_state:
			current_state.exit()
		
		handle_enter_state(_new_state)
