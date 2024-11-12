class_name EnemyGoalDeliverItem
extends EnemyGoal

var dropoff_goal: EnemyGoalDropOffItem
var pickup_goal: EnemyGoalPickupItem
@export var item: CarriableEnemyGoalItem;
@export var dropoff_marker: Marker3D
@export var item_marker: Marker3D
@export var item_color: Color = Color.BLUE

func _ready() -> void:
	for child in get_children():
		if child is EnemyGoalDropOffItem:
			dropoff_goal = child
		if child is EnemyGoalPickupItem:
			pickup_goal = child
	
	# Set the locations of the item and dropoff
	set_item_spawn()
	setup_pickup_goal()
	setup_dropoff_goal()
	# Set item color
	item.change_mesh_color(item_color)

# TODO: Find a better way than this??
func get_sub_goal_order() -> Array[EnemyGoal]:
	return [pickup_goal, dropoff_goal]

func set_item_spawn() -> void:
	item.global_position = item_marker.global_position

func setup_pickup_goal() -> void:
	# Set the item for the pickup and dropoff goals.
	pickup_goal.goal_item = item;

func setup_dropoff_goal() -> void:
	dropoff_goal.goal_item = item;
	dropoff_goal.dropoff_marker = dropoff_marker
