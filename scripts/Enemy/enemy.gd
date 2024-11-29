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
	
	inventory_manager.SetupInventory(self)
	_update_nameplate()
	_set_collisions();
	_setup_model();
	_setup_signals();
	
	call_deferred("_after_spawn");

func _after_spawn() -> void:
	global_position = follow_path.global_position

func _setup_signals() -> void:
	ragdoll_handler.RagdollChange.connect(_handle_ragdoll_change)
	SignalBus.RelicCallsToEntity.connect(_handle_relic_calls)
	action_timer.timeout.connect(_action_timer_timeout)

func _handle_relic_calls(relic: Relic, area: Area3D) -> void:
	if !can_be_called_by_relic: return;
	if last_called_relic == relic: return;
	
	if area == enemy_item_interaction_area:
		can_be_called_by_relic = false
		is_following_path = false;
		last_called_relic = relic;
		set_target_location(relic.global_position)

func view_relic_action() -> void:
	if performing_action: return;
	action_timer.wait_time = 3;
	action_timer.start()
	performing_action = true;
	model.play_human_animation(Animations.Human.JUMP)

func _action_timer_timeout() -> void:
	performing_action = false;
	is_following_path = true;
	can_be_called_by_relic = true;
	pass;

func _setup_personality() -> void:
	display_name = personality.display_name

func _setup_model() -> void:
	model.play_human_animation(default_animation)

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

# Example of how to do the setting of actions / goals.
#func _handle_setting_goals() -> void:
	##agent.SetAvailableActions()
	#const GoapAction = preload("res://scripts/GOAP/Actions/Action.cs");
	#const GoapGoal = preload("res://scripts/GOAP/Goals/GoapGoal.cs")
	##goal1.CreateByName()
	#var actions: Array[GoapAction] = []
	#var action := agent.CreateAction("");
	#var goal: GoapGoal = agent.CreateGoal("", "")
	#agent.SetAvailableActions([action])
	#agent.SetAvailableGoals([goal])

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
			_go_to_desired_location(delta, follow_path.global_position)
		else:
			# If we've gotten within the distance to the relic we view it.
			if global_position.distance_to(target_location) < 5:
				view_relic_action()
				return;
			# Target_location is usually set by the GOAPAgent methods.
			_go_to_desired_location(delta, target_location);
	
	_handle_animations(delta)
	move_and_slide()

func _handle_path_movement(_delta: float) -> void:
	# Get distance between path node and the enemey.
	var distance := global_position.distance_to(follow_path.global_position)
	# If they are within like 2m we should consider it "progress"
	if distance < 5:
		follow_path.progress += _delta * personality.move_speed * 2; # lil speed boost multiplier

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

#
#func _old_physics_process(delta: float) -> void:
	#if !is_on_floor():
		#velocity.y += get_gravity().y * delta
	## If ragdolled, we want to correctly move the position
	#if ragdoll_handler.is_ragdolled:
		#var new_root_transform := model.bones.hips.transform
		#global_transform.origin = global_transform.origin.lerp(new_root_transform.origin, 0.1)
#
	#if nav_map_ready && !ragdoll_handler.is_ragdolled:
		#var direction := Vector3()
		#
		#nav_agent.target_position = target_location
		#direction = (nav_agent.get_next_path_position() - global_position).normalized()
		#velocity = velocity.lerp(direction * personality.move_speed, personality.acceleration * delta)
		#
		## Rotate to face the movement direction
		#if direction != Vector3.ZERO:
			#var target_rotation_y := atan2(direction.x, direction.z)
			#rotation.y = lerp_angle(rotation.y, target_rotation_y, 0.1)
		#
	#_handle_animations(delta)
	#move_and_slide()
