class_name PlayerInteractionManager
extends Node3D

@onready var player: Player = get_tree().get_first_node_in_group(GroupMap.player)
@onready var label: Label3D = $Label

const base_text = "[E] to "

var active_areas: Array[ItemInteractionArea] = []
var can_interact := true;

func register_area(area: ItemInteractionArea) -> void:
	# If it's being held by a player, we don't want to register it.
	if currently_held_by_player(area):
		return;
	active_areas.push_back(area)
	active_areas.sort_custom(_sort_by_distance_to_player)

func unregister_area(area: ItemInteractionArea) -> void:
	active_areas.erase(area)
	active_areas.sort_custom(_sort_by_distance_to_player)

func currently_held_by_player(item: ItemInteractionArea) -> bool:
	var parent := item.get_parent()
	# If there's a parent, and that parent is the CarriableItem
	if parent and parent is CarriableEnemyGoalItem:
		var held_by := (parent as CarriableEnemyGoalItem).holder
		# If held_by is the player, we don't want to show the label.
		return held_by is Player;
	return false;

func _process(delta: float) -> void:
	if active_areas.size() > 0 && can_interact:
		var active_area := active_areas[0];
		label.text = base_text + active_area.action_name
		label.global_position = active_area.global_position
		label.global_position.y += 1 # offset by 36 pixels (idk why)
		label.show()
	else:
		# otherwise hide the label
		label.hide()

func fire_interact(item: ItemInteractionArea) -> void:
	# On press of interact, we want to hide the label for a few ms
	can_interact = false
	label.hide()
	# Unregister the current item so label no longer pops up.
	unregister_area(item)
	# Interact will both drop the old item and pick up a new one if there was one.
	await item.interact.call()
	
	var previous_held_item := player.inventory_manager.GetHeldItem()
	if previous_held_item:
		# Register the interactible_area of the item.
		register_area(previous_held_item.interactible_area)

	# after it's been pressed we set interact back
	can_interact = true;
	

func _input(event: InputEvent) -> void:
	if event.is_action_pressed(KeyMap.interact) && can_interact:
		if active_areas.size() > 0:
			fire_interact(active_areas[0])

func _sort_by_distance_to_player(area1: ItemInteractionArea, area2: ItemInteractionArea) -> bool:
	var area1_to_player := player.global_position.distance_to(area1.global_position)
	var area2_to_player := player.global_position.distance_to(area2.global_position)
	return area1_to_player < area2_to_player
