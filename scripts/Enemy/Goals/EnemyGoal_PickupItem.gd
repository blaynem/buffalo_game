#class_name EnemyGoalPickupItem
#extends EnemyGoal
#
#var goal_item: CarriableEnemyGoalItem
#
#func _ready() -> void:
	#SignalBus.GoalItemHolderChange.connect(_on_goal_item_holder_change)
#
#func _physics_process(delta: float) -> void:
	## Item might have been queue freed, so we should set it to null
	#if goal_item && is_instance_valid(goal_item):
		#set_goal_location(goal_item.global_position)
	#else:
		#goal_item = null
#
#func _on_goal_item_holder_change(item_id: int, new_holder: PhysicsBody3D) -> void:
	## In case the goal has been queue_freed we need to check the valid instance.
	#if is_instance_valid(goal_item) && item_id == goal_item.get_instance_id():
		#set_goal_completed(new_holder is Enemy)
#
#func set_goal_item(item: CarriableEnemyGoalItem) -> void:
	#goal_item = item
	#set_goal_location(item.global_position)
#
## If the entity is within pickup range of item
#func is_within_goal_range(entity: Node3D) -> bool:
	#if entity is Enemy:
		#return goal_item.interactible_area.overlaps_area(entity.enemy_item_interaction_area);
	#return false
#
#func attempt_goal() -> void:
	#if goal_owner.is_stunned:
		#return;
	#var held_item := goal_owner.inventory_manager.GetHeldItem();
	## If enemy has a different item, then drop it.
	#if held_item && held_item != goal_item:
		#held_item.drop_item()
	#
	## now interact with the item
	#goal_item.enemy_interact(goal_owner)
