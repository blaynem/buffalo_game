class_name EnemyBaseModel
extends Node

@onready var bones: BoneManSetup = $Armature
@onready var _animation_player: AnimationPlayer = $AnimationPlayer

var can_ragdoll := false;
var bone_sim: PhysicalBoneSimulator3D;

func _ready() -> void:
	if find_child("PhysicalBoneSimulator3D"):
		can_ragdoll = true;
		bone_sim = $Armature/Skeleton3D/PhysicalBoneSimulator3D
