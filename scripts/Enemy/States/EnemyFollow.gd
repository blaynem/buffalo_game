class_name EnemyFollow
extends EnemyState

# This is the enemy character
@onready var enemy: Enemy = get_owner()

var player: Player;

func enter() -> void:
	player = GroupMap.get_player_from_scene()

func update(delta: float) -> void:
	pass;

func physics_update(delta: float) -> void:
	var direction := player.global_position - enemy.global_position
	var distance := direction.length();
	
	# If the player is farther away than the chase, we go back to idle.
	if distance > enemy.chase_radius:
		Transitioned.emit(self, enemy_states.idle)
		return;
	
	# Move towards the player
	enemy.velocity = direction.normalized() * enemy.move_speed
	# Stop following once we're within the radius
	if distance <= enemy.follow_radius:
		enemy.velocity = Vector3.ZERO
	
	enemy.move_and_slide()
