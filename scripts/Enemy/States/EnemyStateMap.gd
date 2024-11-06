extends Resource
class_name EnemyStateMap;

# Map of all potential Enemy States. In order for these to work properly, 
# the nodes need to keep the script name and not be renamed.

var idle := "EnemyIdle"
var follow := "EnemyFollow"
var attemptGoal := "EnemyAttemptGoal"
