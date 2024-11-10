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
		Transitioned.emit(self, enemy_states.idle)
		return;

	# If goal is a to pick up an item
	if current_goal is EnemyGoalPickupItem:
		handle_pickup_goal(current_goal)
		return;

	# If goal is a drop of item
	if current_goal is EnemyGoalDropOffItem:
		handle_dropoff_goal(current_goal);
		return;
	
	# Otherwise we want to move towards the goal.
	enemy.move_towards_location(enemy.get_current_goal().global_position)

func handle_pickup_goal(pickup_goal: EnemyGoalPickupItem) -> void:
	var carrying_item := pickup_goal.goal_item.carrier == enemy;
	if !carrying_item and pickup_goal.within_interaction_range(enemy):
		pickup_goal.goal_item.enemy_interacted(enemy)
		return;
	enemy.move_towards_location(pickup_goal.get_item_location())

func handle_dropoff_goal(dropoff_goal: EnemyGoalDropOffItem) -> void:
	if dropoff_goal.within_dropoff_range(enemy):
		dropoff_goal.drop_off_item_at_goal()
		return;
	enemy.move_towards_location(dropoff_goal.get_dropoff_location())
