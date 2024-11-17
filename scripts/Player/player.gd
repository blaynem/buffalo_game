class_name Player
extends CharacterBody3D

@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var interaction_area: Area3D = $ItemInteractionArea
@onready var inventory_manager: InventoryManager = $InventoryManager
@onready var model: BuffaloModel = $buffalo

@export var WALK_SPEED := 7.0
@export var RUN_SPEED := 15.0
@export var JUMP_VELOCITY := 5

var _is_running := false
var _is_jumping := false;
var _time_since_last_movement := 0.0

func _ready() -> void:
	set_collisions()
	inventory_manager.setup_inventory(self)

func set_collisions() -> void:
	# Collision on ourselves
	CollisionMap.set_collisions(self, [CollisionMap.player], [
		# We don't include the items as a mask, as that stop the items from getting physics by player.
		CollisionMap.world, # dont fall through world
	])
	# Collisions for the interaction area
	CollisionMap.set_collisions(interaction_area, [CollisionMap.player], [
		CollisionMap.item_interactable, # allow clicking interactable items
	])

func _input(event: InputEvent) -> void:
	if Input.is_action_just_pressed(KeyMap.run):
		_is_running = true;
	if Input.is_action_just_released(KeyMap.run):
		_is_running = false;
	if Input.is_action_just_pressed(KeyMap.jump) and is_on_floor():
		model.play_jump_anim()
		_is_jumping = true;
		velocity.y = JUMP_VELOCITY

func handle_animation(_delta: float) -> void:
	# Wait until we've landed before handling the next animation
	if is_on_floor() && velocity.y <= 0:
		_is_jumping = false;
	else:
		return;
	if velocity == Vector3.ZERO:
		_time_since_last_movement += _delta
		if _time_since_last_movement >= 3:
			model.play_idle_anim()
			#_time_since_last_movement = 0;
			return
		model.play_stand_still_anim()
		return;
	if _is_running:
		model.play_run_anim()
		_time_since_last_movement = 0;
		return
	model.play_walk_anim()
	_time_since_last_movement = 0;

func _physics_process(delta: float) -> void:
	# Add the gravity.
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Only adjust the movement if we're on the floor.
	if is_on_floor():
		# Get the input direction and handle the movement/deceleration.
		var input_dir := Input.get_vector(KeyMap.left, KeyMap.right, KeyMap.forward, KeyMap.backward)
		# The camera alters the players basis
		var direction := (basis * Vector3(input_dir.x, 0, input_dir.y)).normalized()
		var speed := RUN_SPEED if _is_running else WALK_SPEED
	
		if direction:
			velocity.x = direction.x * speed
			velocity.z = direction.z * speed
		else:
			velocity.x = move_toward(velocity.x, 0, speed)
			velocity.z = move_toward(velocity.z, 0, speed)

	handle_animation(delta)
	move_and_slide()
