using Godot;

namespace Buffalobuffalo.scripts.Items {
    // Simple wrapper for the methods to be called on CarriableEnemyGoalItem
    public static class CarriableEnemyGoalItem {
        public static void DropItem(GodotObject item)
        {
            item.Call("drop_item");
        }
        public static void EnemyInteract(GodotObject item, GodotObject actor)
        {
            item.Call("enemy_interact", actor);
        }
    }
}