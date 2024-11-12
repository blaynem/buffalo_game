class_name BoneManSetup
extends Node3D

@onready var skeleton: Skeleton3D = $Skeleton3D

func _ready() -> void:
	_setup_collisions();

func _setup_collisions() -> void:
	# Only setup the collisions if we have the bones!
	if find_child("PhysicalBoneSimulator3D"):
		_setup_physics_bone_collisions()

func _setup_physics_bone_collisions() -> void:
	var physical_bone_simulator: PhysicalBoneSimulator3D = $Skeleton3D/PhysicalBoneSimulator3D
	if physical_bone_simulator:
		for _bone in physical_bone_simulator.get_children():
			var bone: PhysicalBone3D = _bone;
			# If the collisions for the bones are set to enemy, some weird stuff happens.
			CollisionMap.set_collisions(bone, [CollisionMap.bones], [
				CollisionMap.world, # dont fall through world
			])
			# If we set these joints in the inspector, then the 3D view has annoying squares.
			# We still want this type, we just don't want the squares yet.
			bone.joint_type = PhysicalBone3D.JOINT_TYPE_6DOF;
