class_name EnemyGoal
extends Node3D

var _goal_completed := false;

func is_goal_completed() -> bool:
	return _goal_completed;

func set_goal_completed(status: bool) -> void:
	_goal_completed = status;
	SignalBus.EnemyGoalStatusChange.emit(get_instance_id(), status, name)
