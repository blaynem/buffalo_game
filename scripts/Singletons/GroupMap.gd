extends Node

# Map of all Groups

var player := "Player"
var relic := "Relic"
var enemy := "Enemy"
var enemy_activity := "EnemyActivity";
var carriable_item := "CarriableItem";

func get_player_from_scene() -> Player:
	return get_tree().get_first_node_in_group(player)

func get_relics_from_scene() -> Array[Relic]:
	return get_tree().get_nodes_in_group(relic) as Array[Relic];
