class_name EnemyGoalDeliverItem
extends EnemyGoal

var item: CarriableEnemyGoalItem;
var dropoff: EnemyGoalDropOffItem
var pickup: EnemyGoalPickupItem
@export var dropoff_marker: Marker3D
@export var item_marker: Marker3D
@export var item_color: Color = Color.BLUE

func _ready() -> void:
	for child in get_children():
		if child is CarriableEnemyGoalItem:
			item = child
		if child is EnemyGoalDropOffItem:
			dropoff = child
		if child is EnemyGoalPickupItem:
			pickup = child
	
	# Set the item for the pickup and dropoff goals.
	pickup.goal_item = item;
	dropoff.goal_item = item;
	# Set the locations of the item and dropoff
	set_item_spawn(item_marker.global_position)
	set_dropoff_location(dropoff_marker.global_position)
	# Set item color
	item.change_mesh_color(item_color)

# TODO: Find a better way than this??
func get_sub_goal_order() -> Array[EnemyGoal]:
	return [pickup, dropoff]

func set_item_spawn(location: Vector3) -> void:
	item.global_position = location

func set_dropoff_location(location: Vector3) -> void:
	dropoff.global_position = location
