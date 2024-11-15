extends Area3D

@onready var player: Player = get_tree().get_first_node_in_group(GroupMap.player)
var max_parent_check_depth := 7;

func _ready() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.player], [
		CollisionMap.bones, # run into enemy bones
		CollisionMap.enemy # run into enemy
	])
	area_entered.connect(_on_area_entered)

func _on_area_entered(area: Area3D) -> void:
	var enemy: Enemy = _ensure_is_enemy_bones(area);
	if enemy and !enemy.ragdoll_handler.is_ragdolled:
		_enable_ragdoll(enemy)

# Checks to ensure that it's the enemies BONES we're hitting.
func _ensure_is_enemy_bones(target: Node3D) -> Enemy:
	var i := 0;
	var current_parent := target
	while i <= max_parent_check_depth:
		if current_parent is Enemy:
			return current_parent;
		i += 1;
		current_parent = current_parent.get_parent()
	return null;

func _enable_ragdoll(enemy: Enemy) -> void:
	enemy.ragdoll_handler.enable_ragdoll()

	# Parameters to tune the force
	var force_strength := 10.0
	var upward_force_strength := 50.0  # Additional upward force for drama

	# Calculate the relative velocity between the player and the enemy
	var relative_velocity := player.velocity - enemy.velocity  # Assuming enemy has a velocity property

	# Normalize the player's velocity to get the movement direction
	var player_direction := player.velocity.normalized()

	# Main force is based on the player's velocity and force strength
	var main_force := relative_velocity * force_strength

	# Add an upward component to create a dramatic ragdoll effect
	var upward_force := Vector3.UP * upward_force_strength

	# Introduce a slight randomization for realism (optional)
	var random_variation := Vector3(
		randf_range(-0.2, 0.2),
		randf_range(-0.1, 0.1),
		randf_range(-0.2, 0.2)
	)

	# Combine all forces
	var total_force := main_force + upward_force + random_variation

	# Apply the force to the enemy's hips
	enemy.model.bones.hips.apply_central_impulse(total_force)
