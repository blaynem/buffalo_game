extends Node

# The dictionary here is {name, class} from enemy_states
signal EnemyStateMachineTransitioned(enemy_id: int, current: EnemyState, new: Dictionary)
signal EnemyGoalStatusChange(id: int, status: bool, text: String)
signal GoalItemHolderChange(item_id: int, holder: PhysicsBody3D, new_holder: PhysicsBody3D)
