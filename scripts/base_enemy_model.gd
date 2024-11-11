class_name BaseEnemyModel
extends Node3D

@onready var skeleton: Skeleton3D = $Armature/Skeleton3D
@onready var mesh: MeshInstance3D = $Armature/Skeleton3D/MeshInstance3D
@onready var animation_player: AnimationPlayer = $AnimationPlayer

@export var default_animation := Animations.Human.T_POSE

func _ready() -> void:
	play_human_animation(default_animation)

func play_human_animation(anim: Animations.Human) -> void:
	var _anim := Animations.get_human_animation_name(anim)
	animation_player.play(_anim)
