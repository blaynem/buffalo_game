class_name InventoryManagerOld
extends Node

var inventory_owner: CharacterBody3D = null;
var carry_position: Marker3D = null;
var held_item: CarriableEnemyGoalItem = null;

# Note: Do NOT also call held_item.drop_item() here, or it'll recurse
func drop_item() -> void:
	if held_item:
		held_item = null

func pickup_item(item: CarriableEnemyGoalItem) -> void:
	if held_item and inventory_owner is Player:
		held_item.drop_item()
	held_item = item

func setup_inventory(owner: CharacterBody3D) -> void:
	if owner is Player or owner is Enemy:
		carry_position = owner.item_hold_position
		inventory_owner = owner
