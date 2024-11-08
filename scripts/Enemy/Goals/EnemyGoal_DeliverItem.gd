class_name EnemyGoalDeliverItem
extends EnemyGoal

@export_category("SubGoals")
@export var item: GoalItem;
@export var dropoff: EnemyGoalDropOffItem
@export var pickup: EnemyGoalPickupItem

func _ready() -> void:
	for child in get_children():
		if child is GoalItem:
			item = child
		if child is EnemyGoalDropOffItem:
			dropoff = child
		if child is EnemyGoalPickupItem:
			pickup = child
	
	pickup.goal_item = item;
	dropoff.goal_item = item;

# TODO: Find a better way than this??
func get_sub_goal_order() -> Array[EnemyGoal]:
	return [pickup, dropoff]
