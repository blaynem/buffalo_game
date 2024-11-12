class_name CarriableEnemyGoalItem
extends GoalItem

@export var enemy_pickup_cd: float = 1.0;

@onready var player: Player = get_tree().get_first_node_in_group(GroupMap.player)
@onready var collision_shape: CollisionShape3D = %CollisionShape3D
@onready var interactible_area: ItemInteractionArea = %InteractionArea
@onready var enemy_pickup_timer: Timer = $EnemyPickupTimer

var holder: PhysicsBody3D = null;

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
	if holder == player:
		return; # no changes happened.
	# If we're currently holding an item, drop it to pick up the other.
	player.inventory_manager.drop_item()
	# New item is placed on player
	holder = player
	enemy_pickup_timer.start()
	SignalBus.GoalItemHolderChange.emit(self.get_instance_id(), holder)

func enemy_interacted(enemy: Enemy) -> void:
	# Enemy cannot pick up if the timer 
	if holder == enemy or !enemy_pickup_timer.is_stopped():
		return; # no changes happened.
	holder = enemy
	enemy.inventory_manager.pickup_item(self)
	SignalBus.GoalItemHolderChange.emit(self.get_instance_id(), holder)

func drop_item() -> void:
	# If the enemy dropped the item, we start a timer.
	if holder is Enemy:
		enemy_pickup_timer.start()

	SignalBus.GoalItemHolderChange.emit(self.get_instance_id(), null)
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
		self.global_position = holder.inventory_manager.carry_position.global_position
	if holder is Player:	
		self.global_position = holder.inventory_manager.carry_position.global_position
	collision_shape.disabled = true
