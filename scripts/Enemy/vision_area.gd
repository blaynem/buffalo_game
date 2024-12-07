extends Area3D

@onready var vision_timer: Timer = %VisionTimer
@onready var vision_raycast: RayCast3D = %VisionRaycast
@onready var enemy: Enemy = $".."

func _ready() -> void:
	vision_timer.timeout.connect(_on_timer_timeout)
	CollisionMap.set_collisions(self, [], [
		CollisionMap.player
	])
	vision_raycast.set_collision_mask_value(CollisionMap.player, true)

func _on_timer_timeout() -> void:
	var overlaps := get_overlapping_bodies();
	if overlaps.size() <= 0:
		return;
	
	for overlap in overlaps:
		if overlap.is_in_group(GroupMap.player):
			var player: Player = overlap;
			var player_pos := player.face_pointer.global_position;
			vision_raycast.look_at(player_pos, Vector3.UP, true);
			vision_raycast.force_raycast_update();
			
			if vision_raycast.is_colliding():
				var collider := vision_raycast.get_collider();
				if collider.is_in_group(GroupMap.player):
					SignalBus.VisionAreaSpotsPlayer.emit(enemy.get_instance_id())
					# We know the player can be seen by this entity
					vision_raycast.debug_shape_custom_color = Color.GOLD;
