class_name Nameplate
extends Label3D

@export var default_font_size := 60

var content: String = "Content"
var sub_content: String = "SubContent"

func _ready() -> void:
	font_size = default_font_size;

func update_content(_content: String) -> void:
	content = _content
	_update_text()

func update_sub_content(_sub_content: String) -> void:
	sub_content = _sub_content
	_update_text()

func _update_text() -> void:
	var format_string := "{content}\n<{sub_content}>"
	text = format_string.format({"content": content, "sub_content": sub_content})
	pass;
