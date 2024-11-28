class_name TestWorld
extends Node3D

const ENEMY = preload("res://scenes/Enemy/enemy.tscn")
const Brave = preload("res://scripts/Enemy/Personalities/Brave.gd")
const Scared = preload("res://scripts/Enemy/Personalities/Scared.gd")

func _unhandled_key_input(event: InputEvent) -> void:
	if event.is_action_pressed(KeyMap.action_1):
		handle_enemy_spawn(20)

func handle_enemy_spawn(enemy_count: int) -> void:
	# Spawn x amount of enemies.
	for c in range(enemy_count):
		var follow_path := PathFollow3D.new()
		follow_path.progress = 40 * c;
		path_3d.add_child(follow_path);
		spawn_enemy(follow_path);

func spawn_enemy(follow_path: PathFollow3D) -> void:
	var enemy := ENEMY.instantiate()
	enemy.personality = Brave.new()
	enemy.is_stunned = false;
	enemy.nav_map_ready = true
	
	# lastly spawn the enemy
	enemies_node.add_child(enemy)
