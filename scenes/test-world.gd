class_name TestWorld
extends Node3D

const ENEMY = preload("res://scenes/Enemy/enemy.tscn")
const Brave = preload("res://scripts/Enemy/Personalities/Brave.gd")
const Scared = preload("res://scripts/Enemy/Personalities/Scared.gd")

@onready var enemies_node: Node = $Enemies
@onready var relics_node: Node = $Relics
@onready var path_3d: Path3D = $WalkingPath/Path3D

var relic_list: Array[Relic];

func _ready() -> void:
	for child in relics_node.get_children():
		if child is Relic:
			relic_list.append(child);

func _unhandled_key_input(event: InputEvent) -> void:
	if Input.is_action_pressed(KeyMap.minus):
		Engine.time_scale -= 1
	if Input.is_action_pressed(KeyMap.jump):
		Engine.time_scale = 1
	if Input.is_action_pressed(KeyMap.plus):
		Engine.time_scale += 1
	if event.is_action_pressed(KeyMap.action_1):
		handle_enemy_spawn(10)

func handle_enemy_spawn(enemy_count: int) -> void:
	# Spawn x amount of enemies.
	for c in range(enemy_count):
		var follow_path := PathFollow3D.new()
		follow_path.progress = 50 * c;
		path_3d.add_child(follow_path);
		spawn_enemy(follow_path);

func spawn_enemy(follow_path: PathFollow3D) -> void:
	var enemy := ENEMY.instantiate()
	enemy.follow_path = follow_path;
	enemy.personality = Brave.new()
	enemy.is_stunned = false;
	
	# lastly spawn the enemy
	enemies_node.add_child(enemy)
