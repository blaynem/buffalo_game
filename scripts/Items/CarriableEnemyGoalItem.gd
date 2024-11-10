class_name CarriableEnemyGoalItem
extends GoalItem

@export var enemy_pickup_cd: float = 1.0;

@onready var player: Player = get_tree().get_first_node_in_group(GroupMap.player)
@onready var collision_shape: CollisionShape3D = %CollisionShape3D
@onready var mesh: MeshInstance3D = %MeshInstance3D
@onready var interactible_area: ItemInteractionArea = %InteractionArea
@onready var enemy_pickup_timer: Timer = $EnemyPickupTimer
# This can be either the Player marker, Enemy marker, or null.
var carry_object_marker: Marker3D = null;
var carrier: PhysicsBody3D = null;

func _ready() -> void:
	set_collisions();
	# When item is interacted with
	interactible_area.set_interact_fn(player_interacted)
	enemy_pickup_timer.wait_time = enemy_pickup_cd
	enemy_pickup_timer.one_shot = true

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

func player_interacted() -> void:
	if carrier == player:
		return; # no changes happened.
	var old_carrier := carrier;
	# If we're currently holding an item, drop it to pick up the other.
	if player.held_item:
		player.held_item.drop_item()
	# New item is placed on player
	player.held_item = self
	carrier = player
	carry_object_marker = player.item_hold_position
	enemy_pickup_timer.start()
	SignalBus.GoalItemHolderChange.emit(self.get_instance_id(), old_carrier, carrier)

func enemy_interacted(enemy: Enemy) -> void:
	# Enemy cannot pick up if the timer 
	if carrier == enemy or !enemy_pickup_timer.is_stopped():
		return; # no changes happened.
	var old_carrier := carrier;
	carrier = enemy
	carry_object_marker = enemy.item_hold_position
	SignalBus.GoalItemHolderChange.emit(self.get_instance_id(), old_carrier, carrier)

func drop_item() -> void:
	print("drop called")
	# If the enemy dropped the item, we start a timer.
	if carrier is Enemy:
		enemy_pickup_timer.start()

	SignalBus.GoalItemHolderChange.emit(self.get_instance_id(), carrier, null)
	carry_object_marker = null;
	carrier = null;

func place_item_at_goal() -> void:
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
