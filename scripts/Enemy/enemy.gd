class_name Enemy
extends CharacterBody3D

const InventoryManager = preload("res://scripts/Items/InventoryManager.cs")

@onready var nameplate: Nameplate = $Nameplate
@onready var item_hold_position: Marker3D = $CarryObjetMarker
@onready var enemy_item_interaction_area: Area3D = %EnemyItemInteractionArea
@onready var nav_agent: NavigationAgent3D = $NavigationAgent3D
@onready var model: EnemyBaseModel = $BoneManModel
@onready var ragdoll_handler: EnemyRagdollHandler = $RagdollHandler
@onready var action_timer: Timer = $Timer

## If true, the enemy can not take any actions.
@export var is_stunned: bool = true
@export var personality: EnemyPersonality;
@export var default_animation: Animations.Human = Animations.Human.T_POSE

const HumanAgent = preload("res://scripts/GOAP/Agents/HumanAgent.cs");
@onready var agent: HumanAgent = $HumanAgent

var display_name: String
var display_status: String;
var inventory_manager := InventoryManager.new();
var target_location: Vector3;
var held_item: CarriableEnemyGoalItem = null;

# If true, is following the path, rather than going towards the target_location (usually their goal)
var is_following_path: bool = true;
var agent_path: PathFollow3D;
# If true, is currently performing an action.
var performing_action := false;

func set_target_location(location: Vector3) -> void:
	target_location = location

func _ready() -> void:
	# Set personality before everything.
	_setup_personality()
	
	inventory_manager.SetupInventory(self)
	_update_nameplate()
	_set_collisions();
	_setup_signals();
	
	call_deferred("_after_spawn");

func _after_spawn() -> void:
	global_position = agent_path.global_position

func _setup_signals() -> void:
	ragdoll_handler.RagdollChange.connect(_handle_ragdoll_change)
	action_timer.timeout.connect(_action_timer_timeout)

func _action_timer_timeout() -> void:
	performing_action = false;
	pass;

# If is set to true, will follow the path rather than the other objective.
func _follow_path(_should_follow_path: bool) -> void:
	is_following_path = _should_follow_path;

func _setup_personality() -> void:
	display_name = personality.display_name

func _handle_ragdoll_change(is_ragdolled: bool) -> void:
	# Note: This does make it so we can just run through the enemy
	var coll: CollisionShape3D = $CollisionShape3D
	if is_ragdolled:
		is_stunned = true;
		self.set_collision_mask_value(CollisionMap.player, false)
		inventory_manager.DropItem()
	else:
		is_stunned = false
		self.set_collision_mask_value(CollisionMap.player, true)
		model.play_human_animation(Animations.Human.WALK)

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
	model.play_human_animation(Animations.Human.WALK);


# Used for the c# layer to get the inventory manager
func _get_inventory_manager() -> InventoryManager:
	return inventory_manager;

# Used for the c# layer to get the inventory manager
func _get_item_hold_position_marker() -> Marker3D:
	return item_hold_position;

func _physics_process(delta: float) -> void:
	if !is_on_floor():
		velocity.y += get_gravity().y * delta
	# If ragdolled, we want to correctly move the position
	if ragdoll_handler.is_ragdolled:
		var new_root_transform := model.bones.hips.transform
		global_transform.origin = global_transform.origin.lerp(new_root_transform.origin, 0.1)
	
	if !ragdoll_handler.is_ragdolled:
		_handle_path_movement(delta);
		if is_following_path:
			_go_to_desired_location(delta, agent_path.global_position)
		else:
			"""
			TODO: Need to handle animations better now. Since we need to fire differnet ones
			from the Human Agent.
			We should actually move all the animations to C# instead imo
			and have it handled via the HumanAgent.
			Target_location is usually set by the GOAPAgent methods.
			"""
			_go_to_desired_location(delta, target_location);
	
	_handle_animations(delta)
	move_and_slide()

# This is the moving the literal Path3DFollower.
func _handle_path_movement(_delta: float) -> void:
	# Get distance between path node and the enemey.
	var distance := global_position.distance_to(agent_path.global_position)
	# If they are within like 2m we should consider it "progress"
	if distance < 5:
		agent_path.progress += _delta * personality.move_speed * 2; # lil speed boost multiplier

# When we want the agent to Enemy to follow the NavAgent
func _go_to_desired_location(_delta: float, desired_position: Vector3) -> void:
	# Ensuring map is ready.
	var map_RID := NavigationServer3D.agent_get_map(nav_agent);
	if map_RID:
		var direction := Vector3()
		
		nav_agent.target_position = desired_position
		
		# Take out the height so we are on the floor.
		var map_height_offset := NavigationServer3D.map_get_cell_height(map_RID)
		var next_path_pos := nav_agent.get_next_path_position()
		next_path_pos.y -= map_height_offset;
		
		direction = (next_path_pos - global_position).normalized()
		velocity = velocity.lerp(direction * personality.move_speed, personality.acceleration * _delta)
		
		# Rotate to face the movement direction
		if direction != Vector3.ZERO:
			var target_rotation_y := atan2(direction.x, direction.z)
			rotation.y = lerp_angle(rotation.y, target_rotation_y, 0.1)
