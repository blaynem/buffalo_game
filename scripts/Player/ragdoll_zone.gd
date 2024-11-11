extends Area3D

func _ready() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.player], [
		CollisionMap.enemy, # run into enemy
	])
	area_entered.connect(_on_area_entered)
	body_entered.connect(_on_body_entered)

func _on_area_entered(area: Area3D) -> void:
	if area.get_parent() is Enemy:
		_enable_ragdoll(area.get_parent())
		pass;

func _on_body_entered(body: Node3D) -> void:
	if body is Enemy:
		_enable_ragdoll(body)

func _enable_ragdoll(enemy: Enemy) -> void:
	enemy.ragdoll_handler.enable_ragdoll()
