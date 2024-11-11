# How to do things!


## Importing Animations / Models from Mixamo
Need to remove the root motion otherwise the animation will move the player and that sucks.
2 minute video - https://www.youtube.com/watch?v=61WEupW4Eow

Common Gotchas:
	- The name of the animation is actually wrong! Sometimes theres an extra space at the end.
	Easiest way to check is by doing a `animation_player.get_animation_list()` and double checking
	the names.
