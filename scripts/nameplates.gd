class_name Nameplate
extends Label3D

@onready var enemy: Enemy = $".."

@export var default_font_size := 60

func _ready() -> void:
	text = enemy.nameplate_name
	font_size = default_font_size;
