class_name EnemyGoalPickupItem
extends EnemyGoal

@export var goal_item: CarriableEnemyGoalItem

# If the carrier is the Enemy, then the goal is completed as we wanted to grab the item.
func interact_with_goal_item(entity: Object) -> void:
	if entity is Enemy:
		goal_item.set_item_holder(entity)
		set_goal_completed(true)
		return;
	# The goal isn't considered completed unless the Enemy has picked the item up.
	if entity is Player:
		goal_item.set_item_holder(entity)
		set_goal_completed(false)

func dropped() -> void:
	set_goal_completed(false)

func placed() -> void:
	set_goal_completed(true)

func get_item_location() -> Vector3:
	return goal_item.global_position;

# Distance the entity is from the goal.
func _distance_from_goal(entity: Node3D) -> float:
	# TODO: This should be distance from the collision layer, not the global position.
	return (goal_item.global_position - entity.global_position).length()

# If the entity is within pickup range of item
func within_interaction_range(entity: Node3D) -> bool:
	return _distance_from_goal(entity) <= goal_item.pickup_range
