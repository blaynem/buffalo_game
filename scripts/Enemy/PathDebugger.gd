class_name PathDebugger
extends MeshInstance3D

@onready var enemy: Enemy = $".."
@onready var is_visible: bool:
	set(val):
		visible = val

@export var color: Color = Color.RED:
	set(val):
		var mat: StandardMaterial3D = get_surface_override_material(0);
		mat.albedo_color = val;

func _physics_process(delta: float) -> void:
	var pos := enemy.follow_path.global_position;
	global_position = Vector3(pos.x, pos.y + 1, pos.z);
