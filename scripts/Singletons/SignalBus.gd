extends Node

signal EnemyGoalStatusChange(id: int, status: bool, text: String)
signal GoalItemHolderChange(item_id: int, new_holder: PhysicsBody3D)

signal RelicCallsToEntity(relic: Relic, area: Area3D)
