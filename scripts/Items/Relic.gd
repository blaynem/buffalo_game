class_name Relic
extends MeshInstance3D

@onready var call_area: Area3D = $"Call Area"

func _ready() -> void:
	add_to_group(GroupMap.relic)
	_set_collision_shapes();
	_set_signals();

func _set_signals() -> void:
	call_area.area_entered.connect(_call_area_entered)

func _call_area_entered(area: Area3D) -> void:
	SignalBus.RelicCallsToEntity.emit(self, area)

func _set_collision_shapes() -> void:
	# We are a relic, and we want our area to call out to enemies.
	CollisionMap.set_collisions(call_area, [CollisionMap.relic], [CollisionMap.enemy])
