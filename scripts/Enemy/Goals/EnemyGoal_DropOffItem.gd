class_name EnemyGoalDropOffItem
extends EnemyGoal

@export var dropoff_range := 3.0
var goal_item: CarriableEnemyGoalItem

# Distance the entity is from the goal.
func _distance_from_goal(entity: Node3D) -> float:
	# TODO: This should be distance from the collision layer, not the global position.
	return (global_position - entity.global_position).length()

# If the entity is within goal interaction range.
func within_dropoff_range(entity: Node3D) -> bool:
	return _distance_from_goal(entity) <= dropoff_range

func get_dropoff_location() -> Vector3:
	return global_position;

func drop_off_item_at_goal() -> void:
	goal_item.place_item_at_goal()
	set_goal_completed(true)
