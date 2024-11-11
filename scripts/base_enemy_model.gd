class_name BaseEnemyModel
extends Node3D

@onready var skeleton: Skeleton3D = $Armature/Skeleton3D
@onready var mesh: MeshInstance3D = $Armature/Skeleton3D/MeshInstance3D
@onready var animation_player: AnimationPlayer = $AnimationPlayer

@export var default_animation := Animations.Human.T_POSE

func _ready() -> void:
	play_human_animation(default_animation)
	_setup_collisions();

func _setup_collisions() -> void:
	# Only setup the collisions if we have the bones!
	if find_child("PhysicalBoneSimulator3D"):
		_setup_physics_bone_collisions()

func _setup_physics_bone_collisions() -> void:
	var physical_bone_simulator: PhysicalBoneSimulator3D = $Armature/Skeleton3D/PhysicalBoneSimulator3D
	if physical_bone_simulator:
		for _bone in physical_bone_simulator.get_children():
			var bone: PhysicalBone3D = _bone;
			# If the collisions for the bones are set to enemy, some weird stuff happens.
			CollisionMap.set_collisions(bone, [CollisionMap.bones], [
				CollisionMap.world, # dont fall through world
				CollisionMap.player, # run into player
				CollisionMap.enemy, # run into enemy
				CollisionMap.item_interactable, # allow clicking interactable items
			])

func play_human_animation(anim: Animations.Human) -> void:
	var _anim := Animations.get_human_animation_name(anim)
	animation_player.play(_anim)
