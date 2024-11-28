class_name Enemy
extends CharacterBody3D

@onready var nameplate: Nameplate = $Nameplate
@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var enemy_item_interaction_area: Area3D = %EnemyItemInteractionArea
@onready var nav_agent: NavigationAgent3D = $NavigationAgent3D
@onready var model: EnemyBaseModel = $BoneManModel
@onready var ragdoll_handler: EnemyRagdollHandler = $RagdollHandler
@onready var inventory_manager: InventoryManager = $InventoryManager

## If true, the enemy can not take any actions.
@export var is_stunned: bool = true
@export var personality: EnemyPersonality;
@export var default_animation: Animations.Human = Animations.Human.T_POSE

var display_name: String
var display_status: String;
var target_location: Vector3;
# Only use nav when the map is ready
var nav_map_ready := false;
var held_item: CarriableEnemyGoalItem = null;

# If true, is following the path, rather than going towards the target_location (usually their goal
var is_following_path: bool = true;
var follow_path: PathFollow3D;
var performing_action := false;

# If true, can be called by a nearby relic.
var can_be_called_by_relic := true;
var last_called_relic: Relic;

func set_target_location(location: Vector3) -> void:
	target_location = location

func _ready() -> void:
	# Set personality before everything.
	_setup_personality()
	
	inventory_manager.setup_inventory(self)
	_update_nameplate()
	_set_collisions();
	_setup_model();
	_setup_signals();

func _setup_signals() -> void:
	NavigationServer3D.map_changed.connect(_on_navigation_map_ready)
	ragdoll_handler.RagdollChange.connect(_handle_ragdoll_change)

func _setup_personality() -> void:
	display_name = personality.display_name

func _setup_model() -> void:
	model.play_human_animation(default_animation)

func _on_navigation_map_ready(_map: RID) -> void:
	nav_map_ready = true;

func _handle_ragdoll_change(is_ragdolled: bool) -> void:
	# Note: This does make it so we can just run through the enemy
	var coll: CollisionShape3D = $CollisionShape3D
	if is_ragdolled:
		is_stunned = true;
		self.set_collision_mask_value(CollisionMap.player, false)
		var held_item := inventory_manager.held_item;
		if held_item:
			held_item.drop_item()
			inventory_manager.drop_item()
	else:
		is_stunned = false
		self.set_collision_mask_value(CollisionMap.player, true)
		model.play_walk_animation()

func _update_nameplate() -> void:
	nameplate.update_content(display_name)
	nameplate.update_sub_content(display_status)

func _set_collisions() -> void:
	CollisionMap.set_collisions(self, [CollisionMap.enemy], [
		CollisionMap.world, # dont fall through world
		CollisionMap.player, # run into player
		CollisionMap.enemy, # run into enemy
		CollisionMap.item_interactable, # allow clicking interactable items
	])
	CollisionMap.set_collisions(enemy_item_interaction_area, [CollisionMap.enemy], [
		CollisionMap.item_interactable, # allow clicking interactable items
	])

func _handle_animations(_delta: float) -> void:
	if ragdoll_handler.is_ragdolled:
		return;
	if velocity == Vector3.ZERO:
		model.play_human_animation(Animations.Human.IDLE)
		return;
	model.play_walk_animation();


func _physics_process(delta: float) -> void:
	if !is_on_floor():
		velocity.y += get_gravity().y * delta
	# If ragdolled, we want to correctly move the position
	if ragdoll_handler.is_ragdolled:
		var new_root_transform := model.bones.hips.transform
		global_transform.origin = global_transform.origin.lerp(new_root_transform.origin, 0.1)

	if nav_map_ready && !ragdoll_handler.is_ragdolled:
		var direction := Vector3()
		
		nav_agent.target_position = target_location
		direction = (nav_agent.get_next_path_position() - global_position).normalized()
		velocity = velocity.lerp(direction * personality.move_speed, personality.acceleration * delta)
		
		# Rotate to face the movement direction
		if direction != Vector3.ZERO:
			var target_rotation_y := atan2(direction.x, direction.z)
			rotation.y = lerp_angle(rotation.y, target_rotation_y, 0.1)
		
	_handle_animations(delta)
	move_and_slide()
