#class_name EnemyGoalDropOffItem
#extends EnemyGoal
#
#var goal_item: CarriableEnemyGoalItem
#
#func set_goal_item(item: CarriableEnemyGoalItem) -> void:
	#goal_item = item
#
#func attempt_goal() -> void:
	#if goal_owner.is_stunned:
		#return;
	#goal_item.place_item_at_goal()
	#set_goal_completed(true)
