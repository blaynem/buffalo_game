class_name Enemy
extends CharacterBody3D

@onready var nameplate := $Nameplate

@export var move_speed := 3.0

@export_group("Vision Ranges")
## Radius before the enemy stops chasing the player.
## Essentially the "tether" range.
@export var chase_radius := 20.0;
## The radius in which the enemy will detect the player.
@export var detection_radius := 20.0;
## Radius in which the enemy will no longer move closer to the Player.
@export var follow_radius := 5.0;

func handleNameplate() -> void:
	var screen_pos := get_viewport().get_camera_3d().unproject_position(global_transform.origin + Vector3(0, 2, 0))
	nameplate.position = screen_pos

func _process(delta: float) -> void:
	handleNameplate()

func _physics_process(delta: float) -> void:
	pass;
