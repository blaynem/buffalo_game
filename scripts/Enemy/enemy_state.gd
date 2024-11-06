class_name EnemyState;
extends Node

signal Transitioned(current: EnemyState, new: String)

var groups := GroupMap.new()
var enemy_states := EnemyStateMap.new()

func enter() -> void:
	pass

func exit() -> void:
	pass

func update(_delta: float) -> void:
	pass

func physics_update(delta: float) -> void:
	pass
