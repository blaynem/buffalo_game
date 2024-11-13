class_name EnemyGoalManager
extends Node

"""
We're creating a Goal Manager that allows us to group the Enemy Goals a bit better, as well as give out a certain order in which the goals must be completed.

Each immediate child of the Goal Manager will be of a certain type, though they all extend from EnemyGoal. Below are the current available types and their subtypes / nodes.

Current types:
	Goal_DeliverItem
		- GoalItem
		- PickupItem
		- DropItem

We must first complete all of the subgoals before moving on to the next one.
"""

var all_goals: Array[EnemyGoal];
var current_goal_index: int = 0;
var goal_manager_owner: Enemy;

func _ready() -> void:
	goal_manager_owner = get_parent()
	SignalBus.EnemyGoalStatusChange.connect(_handle_enemy_goal_status_change)
	# loop through the Goal Managers' children, only the top nodes.
	for child in get_children():
		if child is EnemyGoalDeliverItem:
			child.set_goal_owner(goal_manager_owner)
			var subgoals: Array[EnemyGoal] = child.get_sub_goal_order()
			for goal in subgoals:
				goal.set_goal_owner(goal_manager_owner)
				all_goals.push_back(goal)

func _handle_enemy_goal_status_change(goal_id: int, new_status: bool, _text: String) -> void:
	update_current_goal();

func update_current_goal() -> void:
	"""
	Whenever a goal is added or updated, the enemy should check through its list and see what should be
	completed next. Then update the current_goal_index.
	For example, if the goal is to obtain an object and deliver it, the enemy must first pick up the object,
	then move towards the delivery zone. If they drop the object, they must obtain it once more.
	"""
	for i in all_goals.size():
		var goal := all_goals[i];
		
		if !goal.is_goal_completed():
			current_goal_index = i;
			return
	current_goal_index = -1;

func get_current_goal() -> EnemyGoal:
	if current_goal_index < 0:
		return null;
	return all_goals[current_goal_index]

func add_goal(new_goal: EnemyGoal) -> void:
	all_goals.append(new_goal);
	update_current_goal();
