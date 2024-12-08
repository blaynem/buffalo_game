extends Node

"""
Basically just a few utils that we can spawn items at certain locations to debug.
"""

func spawn_rectangle(location: Vector3, color: Color = Color.BLUE_VIOLET) -> void:
	# Create a new MeshInstance
	var rectangle := MeshInstance3D.new()
	
	var surface := StandardMaterial3D.new();
	surface.albedo_color = color;

	# Assign a cube mesh (used as the rectangle)
	var mesh := BoxMesh.new()
	mesh.size = Vector3(0.1, 20, 0.1)  # Set the dimensions
	mesh.surface_set_material(0, surface);
	rectangle.mesh = mesh

	# Set the rectangle's position
	rectangle.transform.origin = location

	# Add it to the current scene
	add_child(rectangle)
