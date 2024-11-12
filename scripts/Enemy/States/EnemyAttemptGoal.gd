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

	# If goal is a to pick up an item
	if current_goal is EnemyGoalPickupItem:
		handle_pickup_item(current_goal)
		return;

	# If goal is a drop of item
	if current_goal is EnemyGoalDropOffItem:
		handle_dropoff_item(current_goal);
		return;
	
	# Otherwise we want to move towards the goal.
	enemy.set_target_location(enemy.get_current_goal().global_position)

func handle_pickup_item(item: EnemyGoalPickupItem) -> void:
	# If enemy is already holding the goal item, and its within pick up range.
	# Then pick it up.
	# Otherwise move towards the item.
	var holding_goal_item := enemy.inventory_manager.held_item == item
	if !holding_goal_item and item.within_interaction_range(enemy):
		item.goal_item.enemy_interacted(enemy)
		return;
	enemy.set_target_location(item.get_item_location())

func handle_dropoff_item(item: EnemyGoalDropOffItem) -> void:
	if item.within_dropoff_range(enemy):
		item.drop_off_item_at_goal()
		return;
	enemy.set_target_location(item.get_dropoff_location())
