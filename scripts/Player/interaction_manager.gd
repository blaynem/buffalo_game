extends Node3D

@onready var player: Player = get_tree().get_first_node_in_group(GroupMap.player)
@onready var label: Label3D = $Label

const base_text = "[E] to "

var active_areas: Array[ItemInteractionArea] = []
var can_interact := true;

func register_area(area: ItemInteractionArea) -> void:
	active_areas.push_back(area)
	active_areas.sort_custom(_sort_by_distance_to_player)

func unregister_area(area: ItemInteractionArea) -> void:
	active_areas.erase(area)
	active_areas.sort_custom(_sort_by_distance_to_player)

func _process(delta: float) -> void:
	if active_areas.size() > 0 && can_interact:
		label.text = base_text + active_areas[0].action_name
		label.global_position = active_areas[0].global_position
		label.global_position.y += 1 # offset by 36 pixels (idk why)
		label.show()
	else:
		# otherwise hide the label
		label.hide()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed(KeyMap.interact) && can_interact:
		if active_areas.size() > 0:
			# On press of interact, we want to hide the label for a few ms
			can_interact = false
			label.hide()
			
			await active_areas[0].interact.call()
			# after it's been pressed we set interact back
			can_interact = true;

func _sort_by_distance_to_player(area1: ItemInteractionArea, area2: ItemInteractionArea) -> bool:
	var area1_to_player := player.global_position.distance_to(area1.global_position)
	var area2_to_player := player.global_position.distance_to(area2.global_position)
	return area1_to_player < area2_to_player
