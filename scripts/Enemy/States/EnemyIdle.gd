class_name EnemyIdle
extends EnemyState

# This is the enemy character
@onready var enemy: Enemy = get_owner()

var player: Player;
var move_direction: Vector3
var wander_time: float

func randomize_wander() -> void:
	move_direction = Vector3(randf_range(-1, 1), 0, randf_range(-1, 1)).normalized()
	wander_time = randf_range(1, 3)

func enter() -> void:
	player = GroupMap.get_player_from_scene()
	randomize_wander()
	
func update(delta: float) -> void:
	if enemy.get_current_goal():
		Transitioned.emit(self, enemy_states.goal)
	if wander_time > 0:
		wander_time -= delta
		
	else:
		randomize_wander()

func physics_update(delta: float) -> void:
	enemy.velocity = move_direction * enemy.move_speed
	enemy.move_and_slide()
		
	var playerDirection := player.global_position - enemy.global_position;
	if playerDirection.length() <= enemy.detection_radius:
		Transitioned.emit(self, enemy_states.follow)
	
	
