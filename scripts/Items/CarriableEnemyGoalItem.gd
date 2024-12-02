class_name CarriableEnemyGoalItem
extends RigidBody3D

@export var enemy_pickup_cd: float = 1.0;

@onready var player: Player = get_tree().get_first_node_in_group(GroupMap.player)
@onready var collision_shape: CollisionShape3D = %CollisionShape3D
@onready var interactible_area: ItemInteractionArea = %InteractionArea
@onready var enemy_pickup_timer: Timer = $EnemyPickupTimer

var holder: PhysicsBody3D = null;

func _ready() -> void:
	add_to_group(GroupMap.temp_item)
	set_collisions();
	# When item is interacted with
	interactible_area.set_interact_fn(player_interact)
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
	], [])

func player_interact() -> void:
	if holder == player:
		return; # no changes happened.
	# New item is placed on player
	player.inventory_manager.PickupItem(self)
	holder = player
	enemy_pickup_timer.start()

func enemy_interact(enemy: Enemy) -> void:
	# Enemy cannot pick up if the timer 
	if holder == enemy or !enemy_pickup_timer.is_stopped():
		return; # no changes happened.
	holder = enemy
	enemy.inventory_manager.PickupItem(self)

func drop_item() -> void:
	if holder is Enemy:
		# If the enemy dropped the item, we start a timer.
		enemy_pickup_timer.start()

	holder = null;

# Player can't place at goal.
func place_item_at_goal() -> void:
	if holder is Enemy:
		holder = null;
		queue_free()

func change_mesh_color(new_color: Color) -> void:
	var mesh := get_node("MeshInstance3D") as MeshInstance3D
	var new_material := StandardMaterial3D.new()
	new_material.albedo_color = new_color
	# If we use the same material, it will be reused for all isntances of the box.
	mesh.set_surface_override_material(0, new_material)

func _physics_process(delta: float) -> void:
	if !holder:
		collision_shape.disabled = false
		return;
	if holder is Enemy:
		self.global_position = holder.inventory_manager.GetCarryPosition()
	if holder is Player:	
		self.global_position = holder.inventory_manager.GetCarryPosition()
	collision_shape.disabled = true
