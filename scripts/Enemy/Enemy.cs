using Buffalobuffalo.scripts.Items;
using Godot;

namespace Buffalobuffalo.scripts.Enemy {
    // Simple wrapper for the methods to be called on the Enemy.gd script.
    // _actor is the Enemy.gd script
    public static class GDUtils {
        public static void SetTargetLocation(GodotObject _actor, Vector3 location)
        {
            _actor.Call("set_target_location", location);
        }

        public static InventoryManager GetInventoryManager(GodotObject _actor) {
            return (InventoryManager) _actor.Call("_get_inventory_manager");
        }

        public static void SetFollowPath(GodotObject _actor, bool should_follow) {
            _actor.Call("_follow_path", should_follow);
        }
    }
}