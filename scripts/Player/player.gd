class_name Player
extends CharacterBody3D

@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var interaction_area: Area3D = $ItemInteractionArea
@onready var inventory_manager: InventoryManager = $InventoryManager

@export var WALK_SPEED := 10.0
@export var RUN_SPEED := 15.0
@export var JUMP_VELOCITY := 3

func _ready() -> void:
	set_collisions()
	inventory_manager.setup_inventory(self)

func set_collisions() -> void:
	# Collision on ourselves
	CollisionMap.set_collisions(self, [CollisionMap.player], [
		# We don't include the items as a mask, as that stop the items from getting physics by player.
		CollisionMap.world, # dont fall through world
		CollisionMap.enemy, # run into enemy
	])
	# Collisions for the interaction area
	CollisionMap.set_collisions(interaction_area, [CollisionMap.player], [
		CollisionMap.item_interactable, # allow clicking interactable items
	])

func _physics_process(delta: float) -> void:
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	if Input.is_action_just_pressed(KeyMap.jump) and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	var input_dir := Input.get_vector(KeyMap.left, KeyMap.right, KeyMap.forward, KeyMap.backward)
	# The camera alters the players basis
	var direction := (basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
	var speed := RUN_SPEED if Input.is_action_pressed(KeyMap.run) else WALK_SPEED
	
	if direction:
		velocity.x = direction.x * speed
		velocity.z = direction.z * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
		velocity.z = move_toward(velocity.z, 0, speed)

	move_and_slide()
