class_name EnemyActivityZone
extends Node3D

"""
The EnemyActivityZone is meant as a zone where we can call a `get_target_location`
method in order to get a random point in the zone for the enemy to meander to. When there,
the enemy will fire an action to interact with the specific item in this zone.

Think of it kinda like a field of flowers. They go to a random point, fire the "pick flowers" command,
loot flowers, then consider the action completed.
"""

@onready var area_3d: Area3D = $Area3D
@onready var debug_mesh: MeshInstance3D = $Area3D/MeshInstance3D
@onready var collision_shape: CollisionShape3D = $Area3D/CollisionShape3D

func _ready() -> void:
	add_to_group(GroupMap.enemy_activity)
	_setup_collisions();
	
	_setup_signals();

func get_interaction_animation() -> String:
	return "people_locomotion_pack/jump"

func spawn_rectangle(location: Vector3) -> void:
	# Create a new MeshInstance
	var rectangle := MeshInstance3D.new()

	# Assign a cube mesh (used as the rectangle)
	var mesh := BoxMesh.new()
	mesh.size = Vector3(0.25, 20, 0.25)  # Set the dimensions
	rectangle.mesh = mesh

	# Set the rectangle's position
	var stuff := rectangle;
	rectangle.transform.origin = location

	# Add it to the current scene
	add_child(rectangle)

func get_target_location() -> Vector3:
	var random_point := _get_random_point_in_shape();
	spawn_rectangle(random_point);
	# After we have the random point inside the shape, we need to get the position globally.
	var location := global_position + random_point;
	return location;

func _get_random_point_in_shape() -> Vector3:
	if not collision_shape or not collision_shape.shape:
		return Vector3.ZERO # Fallback if no collision shape

	var shape := collision_shape.shape
	var random_point := Vector3.ZERO

	# Check for specific shape types
	if shape is BoxShape3D:
		var size: Vector3 = shape.size
		random_point.x = randf_range(-size.x / 2, size.x / 2)
		#random_point.y = randf_range(-size.y / 2, size.y / 2)
		random_point.z = randf_range(-size.z / 2, size.z / 2)

	elif shape is SphereShape3D or shape is CylinderShape3D:
		var point := Vector3(randf_range(-1, 1), 0, randf_range(-1, 1)).normalized()
		var outer_radius: float = shape.radius;
		var inner_radius := 0;
		random_point = Vector3.RIGHT.rotated(point, randf() * TAU) * sqrt(randf_range(pow(1 - (outer_radius - inner_radius) / outer_radius, 2), 1)) * outer_radius
	
	# Set y back to .5.
	random_point.y = .5;
	return random_point;


func _setup_collisions() -> void:
	CollisionMap.set_collisions(area_3d, [CollisionMap.activity_area], [])

func _setup_signals() -> void:
	pass;
