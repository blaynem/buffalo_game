extends Node

# Map of all Groups

var player := "Player"
var goalItem := "GoalItem"

func get_player_from_scene() -> Player:
	return get_tree().get_first_node_in_group(player)
