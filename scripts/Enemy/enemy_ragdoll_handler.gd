class_name EnemyRagdollHandler
extends Node

signal RagdollChange(status: bool)

@onready var model: EnemyBaseModel = $"../BoneManModel"
@export var ragdoll_cd: float = 3;

var is_ragdolled := false;

func enable_ragdoll() -> void:
	if model.can_ragdoll && !is_ragdolled:
		is_ragdolled = true;
		RagdollChange.emit(true)
		model.bone_sim.physical_bones_start_simulation()
		ragdoll_timeout(ragdoll_cd)

func disable_ragdoll() -> void:
	if model.can_ragdoll:
		is_ragdolled = false
		RagdollChange.emit(false)
		model.bone_sim.physical_bones_stop_simulation()

func ragdoll_timeout(time: float) -> void:
	var timer := get_tree().create_timer(time)
	
	await timer.timeout
	disable_ragdoll()
