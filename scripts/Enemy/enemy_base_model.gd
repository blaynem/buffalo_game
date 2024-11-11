class_name EnemyBaseModel
extends Node

@export var default_animation := Animations.Human.IDLE

@onready var animation_player: AnimationPlayer = $AnimationPlayer

var is_ragdoll := false;
var ragdoll_skeleton: RagdollSkeleton;

func _ready() -> void:
	if find_child("PhysicalBoneSimulator3D"):
		is_ragdoll = true;
		ragdoll_skeleton = $Armature/Skeleton3D/PhysicalBoneSimulator3D
	play_human_animation(default_animation)

func enable_ragdoll() -> void:
	if is_ragdoll:
		ragdoll_skeleton.enable_ragdoll()

func disable_ragdoll() -> void:
	if is_ragdoll:
		ragdoll_skeleton.disable_ragdoll()

func play_human_animation(anim: Animations.Human) -> void:
	if find_child("AnimationPlayer"):
		var animation_player: AnimationPlayer = $AnimationPlayer
		var _anim := Animations.get_human_animation_name(anim)
		animation_player.play(_anim)
