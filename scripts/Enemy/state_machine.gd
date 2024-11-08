extends Node

@export var initial_state: EnemyState
@export var nameplate: Label;

var current_state: EnemyState
var states: Dictionary = {}

func _ready() -> void:
	# Loop through all children of the StateMachine node
	for child in get_children():
		if child is EnemyState:
			states[child.name.to_lower()] = child
			child.Transitioned.connect(on_child_transition)
	
	if initial_state:
		handle_enter_state(initial_state)

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
	nameplate.text = new_state.name

# This is the connect method of Transitioned, the new_state is a Dictionary of {name, class}
func on_child_transition(state: EnemyState, new_state: Dictionary) -> void:
	if state != current_state:
		return;
		
	var _new_state := states.get(new_state.name.to_lower()) as EnemyState
	if !_new_state:
		return
		
	if current_state:
		current_state.exit()
	
	handle_enter_state(_new_state)
