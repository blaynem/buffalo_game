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
	if distance > enemy.personality.chase_radius:
		SignalBus.EnemyStateMachineTransitioned.emit(enemy.get_instance_id(), self, enemy_states.idle)
		return;
	
	# Move towards the player
	enemy.set_target_location(player.global_position)
