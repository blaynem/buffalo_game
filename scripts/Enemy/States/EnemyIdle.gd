class_name EnemyIdle
extends EnemyState

# This is the enemy character
@export var enemy: CharacterBody3D
@export var move_speed := 5.0

var move_direction: Vector3
var wander_time: float

func randomize_wander() -> void:
	move_direction = Vector3(randf_range(-1, 1), 0, randf_range(-1, 1)).normalized()
	wander_time = randf_range(1, 3)

func enter() -> void:
	randomize_wander()
	
func update(delta: float) -> void:
	if wander_time > 0:
		wander_time -= delta
	
	else:
		randomize_wander()

func physics_update(delta: float) -> void:
	if enemy:
		enemy.velocity = move_direction * move_speed
		enemy.move_and_slide()
