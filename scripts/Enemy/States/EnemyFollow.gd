class_name EnemyFollow
extends EnemyState

var groups := GroupMap.new()

# This is the enemy character
@export var enemy: CharacterBody3D
@export var move_speed := 5.0
@export var chase_radius := 5.0;

var player: Player;
var move_direction: Vector3
var wander_time: float

func enter() -> void:
	player = get_tree().get_first_node_in_group(groups.player)
	
func update(delta: float) -> void:
	pass;

func physics_update(delta: float) -> void:
	var direction := player.global_position - enemy.global_position
	
	if direction.length() > chase_radius:
		enemy.velocity = direction.normalized() * move_speed
	else:
		enemy.velocity = Vector3.ZERO
		
	enemy.move_and_slide()
