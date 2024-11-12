# How to do things!


## Importing Animations / Models from Mixamo
Need to remove the root motion otherwise the animation will move the player and that sucks.
2 minute video - https://www.youtube.com/watch?v=61WEupW4Eow

Common Gotchas:
	- The name of the animation is actually wrong! Sometimes theres an extra space at the end.
	Easiest way to check is by doing a `animation_player.get_animation_list()` and double checking
	the names.

## Ragdoll
Followed along with this video: https://www.youtube.com/watch?v=0MHY2TDeMLM&ab_channel=CrigzVsGameDev

Helpful tips:
	- Seeing the bones and joints is HARD!!! To make it easier...
		- Turn off "preview sunlight" and "preview environment"
			- There are two buttons that look like a sun and a globe in the top bar under the list of open scenes
		- Turn on "Display wireframe"
			- Click the three-dot menu in the viewport (it should say "Perspective" or "Orthogonal") to turn this on
		- Look at the model directly from each side in orthogonal view when setting up the collision shapes
			- You can do this by clicking the X/Y/Z circles in the "compass" in the top-right of the viewport
	- Set up the joints first, THEN go through and set all of the collision shapes
		- Sometimes, you have to rotate a joint to get the angles right. This also rotates the collision shape, so it's a bit painful to have to change both of them all the time
	- Hide all of the PhysicalBone3D nodes that you aren't actively working on, otherwise it's just WAY too busy

Gotcha:
	- Need to run the `physical_bones_start_simulation()` on PhysicalBoneSumulator3d instead
	- Bones shouldn't have enemy mask, otherwise they get REAL weird.
