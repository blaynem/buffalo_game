class_name ItemInteractionArea
extends Area3D

## The string that shows up when within interaction range.
@export var action_name := "interact"

var interact: Callable;

func _ready() -> void:
	self.area_entered.connect(_on_area_entered)
	self.area_exited.connect(_on_area_exited)
	set_collisions()
	
func set_collisions() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.item_interactable], [
		CollisionMap.player, # player can interact
		CollisionMap.enemy, # enemy can interact
	])

func _on_area_entered(_area: Area3D) -> void:
	InteractionManager.register_area(self)

func _on_area_exited(_area: Area3D) -> void:
	InteractionManager.unregister_area(self)
