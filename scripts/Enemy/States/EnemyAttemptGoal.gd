class_name EnemyAttemptGoal
extends EnemyState

# This is the enemy character
@onready var enemy: Enemy = get_owner()

func enter() -> void:
	pass;

func update(delta: float) -> void:
	pass;

func physics_update(delta: float) -> void:
	var current_goal := enemy.get_current_goal()
	if !current_goal:
		SignalBus.EnemyStateMachineTransitioned.emit(self.get_instance_id(), self, enemy_states.idle)
		return;

	if current_goal.is_within_goal_range(enemy):
		current_goal.attempt_goal()
	
	# Otherwise we want to move towards the goal.
	enemy.set_target_location(current_goal.get_goal_location())
