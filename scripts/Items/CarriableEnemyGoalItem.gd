class_name CarriableEnemyGoalItem
extends GoalItem

@onready var collision_shape := %CollisionShape3D
@onready var mesh := %MeshInstance3D
@onready var interactible_area := %Area3D
# This can be either the Player marker, Enemy marker, or null.
var carry_object_marker: Marker3D = null;
var carrier: CharacterBody3D = null;

func _ready() -> void:
	set_collisions();

func set_collisions() -> void:
	# Collisions for self
	CollisionMap.set_collisions(self, [
		CollisionMap.items, # is an item
	], [
		CollisionMap.world, # dont fall through world
		CollisionMap.player, # get pushed by player
		CollisionMap.items, # get pushed by other items
		CollisionMap.enemy # get pushed by enemy
	])
	# Collisions for interactable area
	CollisionMap.set_collisions(interactible_area, [
		CollisionMap.item_interactable # is an interactable area
	], [
		CollisionMap.player, # get pushed by player
		CollisionMap.enemy # get pushed by enemy
	])

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

func change_mesh_color(new_color: Color) -> void:
	var new_material := StandardMaterial3D.new()
	new_material.albedo_color = new_color
	# If we use the same material, it will be reused for all isntances of the box.
	mesh.set_surface_override_material(0, new_material)

func _physics_process(delta: float) -> void:
	if carrier == null:
		collision_shape.disabled = false
	else:
		self.global_position = carry_object_marker.global_position
		collision_shape.disabled = true
