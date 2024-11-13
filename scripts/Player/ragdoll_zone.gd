extends Area3D

var max_parent_check_depth := 7;

func _ready() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.player], [
		CollisionMap.bones, # run into enemy bones
		CollisionMap.enemy # run into enemy
	])
	area_entered.connect(_on_area_entered)

func _on_area_entered(area: Area3D) -> void:
	var enemy: Enemy = _ensure_is_enemy_bones(area);
	if enemy:
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
