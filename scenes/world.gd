extends Node3D

@onready var goal_items_node: Node = $GoalItems
@onready var enemies_holder_node: Node = $Enemies

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
		print("wow")

func spawn_enemy() -> void:
	var enemy := ENEMY.instantiate()
	enemy.personality = Brave.new()
	enemy.is_stunned = false;
	enemy.position = Vector3(0,1,0)
	enemy.nav_map_ready = true
	var goal_manager_node := Node.new()
	goal_manager_node.set_script(EnemyGoalManager)
	
	var goal1 := GOAL_DELIVER_ITEM.instantiate()
	var spawn_marker := Marker3D.new()
	var deliver_marker := Marker3D.new()
	var item1 := BOX_ITEM.instantiate()
	
	enemy.add_child(goal_manager_node)
	goal_manager_node.add_child(goal1)
	enemy.goal_manager = goal_manager_node
	
	goal_items_node.add_child(item1)
	
	goal1.item = item1
	goal1.dropoff_marker = deliver_marker
	goal1.item_spawn_marker = spawn_marker
	goal1.item_color = Color.AQUA
	goal1.add_child(spawn_marker)
	goal1.add_child(deliver_marker)
	spawn_marker.global_position = Vector3(-10,0,5)
	deliver_marker.global_position = Vector3(10,0,5)
	enemies_holder_node.add_child(enemy)
	pass
	#var goal_manager := EnemyGoalManager.new()
	#var enemy := Enemy.new();
