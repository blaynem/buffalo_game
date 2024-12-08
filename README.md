# How to do things!

## Setup
- Ensure you have an exported var in your terminal for `GODOT4`. Mine is `export GODOT4="/Applications/godot.app/Contents/MacOS/Godot"`
- Few extensions to install:
	- `C# Dev Kit` by Microsoft, `.NET Install Tool` by Microsoft, `godot-tools` by Geequlim

## FAQ
**Why can't I access my C# code in gdscript?**
You can, nerd! Just need to build it. Press Ctrl + Shift + B (or Cmd + Shift + B on macOS) and click `build`.

**Okay, but I really can't see it even after building.. why?**
It's possible you're not extending a Godot native type like `Node`, `Resource`, etc.
It's also likely because the type you're trying to use is not compatible. Take the below examples.
```c#
// Can be seen
public bool BoolTest(bool test) {
    return test;
}

// Can be seen
public GodotObject TestGodotObj(GodotObject test) {
    return test;
}

// Cannot be seen. As object is not a known gdscript construct.
public object TestObject(object test) {
    return test;
}

// Then in GDScript
const GoapAgent := preload("res://scripts/GOAP/Agents/GoapAgent.cs");
var goapAgent := GoapAgent.new();
goapAgent.BoolTest(false);
goapAgent.TestGodotObj(Object.new());
// Does not work.
goapAgent.TestObject(Object.new());
```


## TODO:
- Cleanup items / Personalities to be a resource: https://www.youtube.com/watch?v=4vAkTHeoORk&ab_channel=Godotneers

## Attaching Bones to Model in Blender

- Easy tutorial to follow! https://www.youtube.com/watch?v=U1yL9qKlgKM&ab_channel=PolyFlo
- Keep in mind there is an automatic rig for people / animals

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

## Help with Math

I should have paid better attention to maths! (This)[https://allenchou.net/2019/08/trigonometry-basics-sine-cosine/] tutorial is helpful.