class_name EnemyPersonality extends Resource

var display_name: String = "Trent"
## How fast direction is changed
var acceleration: float = 10.0;
## How quickly the enemy walks
var move_speed: float = 3.0
## How quickly the enemy runs
var run_speed: float = 5.0

@export_group("Vision Ranges")
## Radius before the enemy stops chasing the player.
## Essentially the "tether" range.
var chase_radius: float = 20.0;
## The radius in which the enemy will detect the player.
var detection_radius: float = 20.0;
## Radius in which the enemy will no longer move closer to the Player.
var follow_radius: float = 5.0;
