extends Node

# The dictionary here is {name, class} from enemy_states
# TODO: Delete this EnemyStateMachineTransitioned, since it's only used in state machine.
signal EnemyStateMachineTransitioned(enemy_id: int, current: EnemyState, new: Dictionary)
signal EnemyGoalStatusChange(id: int, status: bool, text: String)
signal GoalItemHolderChange(item_id: int, new_holder: PhysicsBody3D)
