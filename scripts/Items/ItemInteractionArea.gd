class_name ItemInteractionArea
extends Area3D

## The string that shows up when within interaction range.
@export var action_name := "interact"

var interact: Callable;

func set_interact_fn(_fn: Callable) -> void:
	interact = _fn

func _ready() -> void:
	self.area_entered.connect(_on_area_entered)
	self.area_exited.connect(_on_area_exited)
	set_collisions()

func set_collisions() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.item_interactable], [
		CollisionMap.enemy, # enemy can interact
	])

func _on_area_entered(_area: Area3D) -> void:
	var parent := _area.get_parent()
	if parent is Player:
		InteractionManager.register_area(self)

func _on_area_exited(_area: Area3D) -> void:
	var parent := _area.get_parent()
	if parent is Player:
		InteractionManager.unregister_area(self)
