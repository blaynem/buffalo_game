extends CharacterBody3D

@onready var nameplate := $Nameplate

func handleNameplate() -> void:
	var screen_pos := get_viewport().get_camera_3d().unproject_position(global_transform.origin + Vector3(0, 2, 0))
	nameplate.position = screen_pos

func _process(delta: float) -> void:
	handleNameplate()

func _physics_process(delta: float) -> void:
	pass;
