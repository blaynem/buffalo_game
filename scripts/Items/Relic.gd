class_name Relic
extends Node3D

@onready var markers: Node = $Markers

func _ready() -> void:
	add_to_group(GroupMap.relic)
	_turn_off_marker_meshes();

func _turn_off_marker_meshes() -> void:
	for marker in markers.get_children():
		var mesh: MeshInstance3D = marker.get_child(0);
		mesh.queue_free()

# Gets a random location from the available markers.
func get_view_location() -> Vector3:
	var selection := randf_range(0, markers.get_child_count())
	var marker: Marker3D = markers.get_child(selection);
	return marker.global_position;
