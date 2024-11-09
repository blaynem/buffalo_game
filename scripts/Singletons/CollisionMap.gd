extends Node

"""
For collisions, they're a bit confusing to conceptualize.
(ty blessed redditor Vickera)

Layers = "what i am"
Mask = "what i touch"

Examples:
	For Tilemaps, "I am environment, I touch nothing"*
	For player, "I am player, I touch environment"
	For enemy, "I am enemy, I touch environment"
	For player deal damage box, "I am nothing, I touch enemy"
	For enemy deal damage box, "I am nothing, I touch player"
	For a level prop with no actor interaction, "I am nothing, I touch environment"
"""


var world: int = 1;
var player: int = 2;
var items: int = 3;
var enemy: int = 4;
# When within this layer, an item can be interacted with. 
# kinda like "press 'e' to interact"
var item_interactable: int = 5;

# Clears the default collision layers / masks.
func _clear_collisions(entity: CollisionObject3D) -> void:
	entity.set_collision_layer_value(1, false)
	entity.set_collision_mask_value(1, false)

# Helper that will first clear all the collisions, then set them.
func set_collisions(entity: CollisionObject3D, layers: Array[int], masks: Array[int]) -> void:
	_clear_collisions(entity)
	
	for layer in layers:
		entity.set_collision_layer_value(layer, true)
	
	for mask in masks:
		entity.set_collision_mask_value(mask, true)
