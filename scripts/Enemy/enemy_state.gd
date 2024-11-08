class_name EnemyState;
extends Node

var enemy_states := EnemyStateMap.new()
# The dictionary here is {name, class} from enemy_states
signal Transitioned(current: EnemyState, new: Dictionary)

func enter() -> void:
	pass

func exit() -> void:
	pass

func update(_delta: float) -> void:
	pass

func physics_update(_delta: float) -> void:
	pass
