class_name InventoryManager
extends Node

var inventory_owner: CharacterBody3D = null;
var carry_position: Marker3D = null;
var held_item: CarriableEnemyGoalItem = null;

func drop_item() -> void:
	if held_item:
		held_item.drop_item()
		held_item = null

func pickup_item(item: CarriableEnemyGoalItem) -> void:
	if held_item and inventory_owner is Player:
		drop_item()
	held_item = item
	SignalBus.GoalItemHolderChange.emit(item.get_instance_id(), inventory_owner)

func setup_inventory(owner: CharacterBody3D) -> void:
	if owner is Player or owner is Enemy:
		carry_position = owner.item_hold_position
		inventory_owner = owner
