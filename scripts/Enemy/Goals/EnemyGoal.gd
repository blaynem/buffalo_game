#class_name EnemyGoal
#extends Node
#
#var goal_owner: Enemy;
#
#var _goal_location: Vector3;
#var _goal_completed := false;
#
#var goal_interaction_range := 2
#
#func set_goal_owner(enemy: Enemy) -> void:
	#goal_owner = enemy;
#
#func is_goal_completed() -> bool:
	#return _goal_completed;
#
#func set_goal_completed(status: bool) -> void:
	#_goal_completed = status;
	#SignalBus.EnemyGoalStatusChange.emit(get_instance_id(), status, name)
#
#func get_goal_location() -> Vector3:
	#return _goal_location
#
#func set_goal_location(loc: Vector3) -> void:
	#_goal_location = loc
#
## If the entity is within interaction range
#func is_within_goal_range(target: Enemy) -> bool:
	#return (_goal_location - target.global_position).length() <= goal_interaction_range
#
## Attempt the goal, this is meant to be overridden.
#func attempt_goal() -> void:
	#if goal_owner.is_stunned:
		#return;
	#pass;
