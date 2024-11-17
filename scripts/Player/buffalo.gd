class_name BuffaloModel
extends Node3D

@onready var animation_player: AnimationPlayer = $AnimationPlayer
var default_animation := Animations.Buffalo.IDLE

func _ready() -> void:
	play_buffalo_animation(default_animation)

func play_buffalo_animation(anim: Animations.Buffalo) -> void:
	var _anim := Animations.get_buffalo_animation_name(anim)
	animation_player.play(_anim)

func play_idle_anim() -> void:
	play_buffalo_animation(Animations.Buffalo.IDLE)
	
func play_walk_anim() -> void:
	play_buffalo_animation(Animations.Buffalo.WALK)

func play_run_anim() -> void:
	play_buffalo_animation(Animations.Buffalo.RUN)

func play_yeet_anim() -> void:
	play_buffalo_animation(Animations.Buffalo.YEET)

func play_stand_still_anim() -> void:
	play_buffalo_animation(Animations.Buffalo.RESET)

func play_jump_anim() -> void:
	play_buffalo_animation(Animations.Buffalo.JUMP)
