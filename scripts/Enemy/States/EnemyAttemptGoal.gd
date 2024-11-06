class_name EnemyAttemptGoal
extends EnemyState

# This is the enemy character
@onready var enemy: Enemy = get_owner()

var player: Player;
var goalItem: GoalItem;
var pickupRange := 3.0

func enter() -> void:
	player = get_tree().get_first_node_in_group(groups.player)
	goalItem = get_tree().get_first_node_in_group(groups.goalItem)

func update(delta: float) -> void:
	pass;

func physics_update(delta: float) -> void:
	var playerDirection := player.global_position - enemy.global_position;
	# If player is nearby, then we want to break out of the Goal.
	if playerDirection.length() <= enemy.detection_radius:
		Transitioned.emit(self, enemy_states.follow)
		return;
	
	var goalDirection := goalItem.global_position - enemy.global_position;
	enemy.velocity = goalDirection.normalized() * enemy.move_speed
	enemy.move_and_slide()
		
	# If within pickup range, attempt to pick up.
	if goalDirection.length() <= pickupRange:
		print("Close enough????")
