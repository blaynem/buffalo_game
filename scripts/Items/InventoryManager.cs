using Godot;

namespace Buffalobuffalo.scripts.Items {
    public partial class InventoryManager : Resource {
        private CharacterBody3D inventory_owner;
        public Marker3D Carry_position {get; private set;}
        public GodotObject Held_item {get; private set;}

        public InventoryManager() { }

        public void DropItem() {
            if (Held_item == null) return;
            // Calling `drop_item` on the `CarriableEnemyGoalItem` node.
            Items.CarriableEnemyGoalItem.DropItem(Held_item);

            Held_item = null;
        }

        public Vector3 GetCarryPosition() {
            return Carry_position.GlobalPosition;
        }

        public GodotObject GetHeldItem(){
            return Held_item;
        }

        public void PickupItem(GodotObject item) {
            Held_item = item;
        }

        public void SetupInventory(CharacterBody3D _inventory_owner) {
            inventory_owner = _inventory_owner;
            Carry_position = (Marker3D)_inventory_owner.Call("_get_item_hold_position_marker");
        }
    }
}