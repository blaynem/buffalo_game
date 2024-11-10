extends Node

signal EnemyGoalStatusChange(id: int, status: bool, text: String)
signal GoalItemHolderChange(item_id: int, holder: PhysicsBody3D, new_holder: PhysicsBody3D)
