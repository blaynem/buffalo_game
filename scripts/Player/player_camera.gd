extends Camera3D

@export var mouse_sensitivity := 0.1
@export var _player: Player

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and Input.mouse_mode == Input.MOUSE_MODE_CAPTURED:
		var delta_x: float = -event.relative.x * mouse_sensitivity
		var delta_y: float = -event.relative.y * mouse_sensitivity
		
		# Rotate around Y axis (left/right)
		_player.rotate_y(deg_to_rad(delta_x))
		
		# Rotate around X axis (up/down)
		var new_camera_rotation_x: float = rotation_degrees.x + delta_y
		new_camera_rotation_x = clamp(new_camera_rotation_x, -89.9, 89.9)
		rotation_degrees.x = new_camera_rotation_x
