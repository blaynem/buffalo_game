class_name EnemyGoalPickupItem
extends EnemyGoal

var goal_item: CarriableEnemyGoalItem

func _ready() -> void:
	SignalBus.GoalItemHolderChange.connect(_on_goal_item_holder_change)

func _on_goal_item_holder_change(item_id: int, holder: PhysicsBody3D, new_holder: PhysicsBody3D) -> void:
	if item_id == goal_item.get_instance_id():
		if new_holder is Player:
			set_goal_completed(false)
		if new_holder is Enemy:
			set_goal_completed(true)

func get_item_location() -> Vector3:
	return goal_item.global_position;

# Distance the entity is from the goal.
func _distance_from_goal(entity: Node3D) -> float:
	# TODO: This should if entity is within InteractionArea layer, not the global position.
	return (goal_item.global_position - entity.global_position).length()

# If the entity is within pickup range of item
func within_interaction_range(entity: Node3D) -> bool:
	return _distance_from_goal(entity) <= goal_item.pickup_range
