class_name RagdollSkeleton
extends PhysicalBoneSimulator3D

@export var linear_spring_stiffness: float = 1200.0
@export var linear_spring_damping: float = 40.0
@export var max_linear_force: float = 9999.0

@export var angular_spring_stiffness: float = 4000.0
@export var angular_spring_damping: float = 80.0
@export var max_angular_force: float = 9999.0

## This is the animated skeleton that we want to keep track of.
@export var target_skeleton: Skeleton3D
var parent_skeleton: Skeleton3D
var physics_bones: Array[Node]

func enable_ragdoll() -> void:
	physical_bones_start_simulation()

func disable_ragdoll() -> void:
	physical_bones_stop_simulation()

func _ready() -> void:
	parent_skeleton = get_parent()
	physics_bones = get_children().filter(_filter_physics_bones)

func _physics_process(delta: float) -> void:
	for b in physics_bones:
		# Despite it saying "global" bone pose, it's relative to the skeleton its from
		var bone_pose := target_skeleton.get_bone_global_pose(b.get_bone_id())
		var target_transform := target_skeleton.global_transform * target_skeleton.get_bone_global_pose(b.get_bone_id())
		# So we get the transfrom of the ragdoll skeleton
		var current_transform := global_transform * parent_skeleton.get_bone_global_pose(b.get_bone_id())
		
		# this seems like the pos difference would be equal.
		var position_diff := target_transform.origin - current_transform.origin
		var force := hookes_law(position_diff, b.linear_velocity, linear_spring_stiffness, linear_spring_damping)
		
		b.linear_velocity += (force * delta)
		
		var rotation_diff := (target_transform.basis * current_transform.basis.inverse())
		var torque := hookes_law(rotation_diff.get_euler(), b.angular_velocity, angular_spring_stiffness, angular_spring_damping)
		b.angular_velocity += (torque * delta)

func _filter_physics_bones(x: Object) -> bool:
	return x is PhysicalBone3D

func hookes_law(
	displacement: Vector3,
	current_velocity: Vector3,
	stiffness: float,
	dampening: float
) -> Vector3:
	return (stiffness * displacement) - (dampening * current_velocity)
