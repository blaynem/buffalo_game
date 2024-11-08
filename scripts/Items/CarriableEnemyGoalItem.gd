class_name CarriableEnemyGoalItem
extends GoalItem

@onready var collision_shape := %CollisionShape3D
# This can be either the Player marker, Enemy marker, or null.
var carry_object_marker: Marker3D = null;
var carrier: CharacterBody3D = null;

func set_item_holder(interactor: Object) -> void:
	if interactor is Enemy:
		carrier = interactor
		carry_object_marker = interactor.item_hold_position
	if interactor is Player:
		carrier = interactor
		carry_object_marker = interactor.item_hold_position

func drop_item() -> void:
	print("drop called")
	carry_object_marker = null;
	carrier = null;

func _physics_process(delta: float) -> void:
	if carrier == null:
		collision_shape.disabled = false
	else:
		self.position = carry_object_marker.global_position
		collision_shape.disabled = true
