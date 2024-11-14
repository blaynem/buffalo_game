extends Node3D

@onready var goal_items_node: Node = $GoalItems
@onready var enemies_node: Node = $Enemies
# Spawn Markers
@onready var enemy_spawns: Node = $Markers/EnemySpawns
@onready var goal_ending_spawns: Node = $Markers/GoalEndingSpawns
@onready var goal_item_spawns: Node = $Markers/GoalItemSpawns

const ENEMY = preload("res://scenes/Enemy/enemy.tscn")
const GOAL_DELIVER_ITEM = preload("res://scenes/EnemyGoals/goal_deliver_item.tscn")
const BOX_ITEM = preload("res://scenes/Items/BoxItem.tscn")
const SPHERE_ITEM = preload("res://scenes/Items/SphereItem.tscn")
const EnemyGoalManager = preload("res://scripts/Enemy/Goals/EnemyGoalManager.gd")
const Brave = preload("res://scripts/Enemy/Personalities/Brave.gd")
const Scared = preload("res://scripts/Enemy/Personalities/Scared.gd")

func _unhandled_key_input(event: InputEvent) -> void:
	if event.is_action_pressed(KeyMap.action_1):
		spawn_enemy()

# The spawn_node arg should be a Node with Marker3D's as children.
func select_spawn(spawn_node: Node) -> Marker3D:
	var marker_nodes := spawn_node.get_children()
	var _count := marker_nodes.size()
	if _count <= 0:
		return null;
	if _count == 1:
		return marker_nodes[0];
	return marker_nodes.pick_random()

func spawn_enemy() -> void:
	var enemy := ENEMY.instantiate()
	enemy.personality = Brave.new()
	enemy.is_stunned = false;
	enemy.position = select_spawn(enemy_spawns).global_position
	enemy.nav_map_ready = true
	
	var goal_manager_node := Node.new()
	goal_manager_node.set_script(EnemyGoalManager)
	enemy.add_child(goal_manager_node)
	enemy.goal_manager = goal_manager_node
	
	var goal1 := GOAL_DELIVER_ITEM.instantiate()
	goal_manager_node.add_child(goal1)
	
	var item1 := BOX_ITEM.instantiate()
	goal1.item = item1
	goal1.dropoff_marker = select_spawn(goal_ending_spawns)
	goal1.item_spawn_marker = select_spawn(goal_item_spawns)
	goal1.item_color = Color.AQUA
	goal_items_node.add_child(item1)
	
	# lastly spawn the enemy
	enemies_node.add_child(enemy)
	pass
