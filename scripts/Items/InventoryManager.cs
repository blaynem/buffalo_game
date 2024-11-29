using Godot;

namespace Buffalobuffalo.scripts.Items {
    public partial class InventoryManager : Resource {
        private CharacterBody3D inventory_owner;
        public Marker3D Carry_position {get; private set;}
        public GodotObject Held_item {get; private set;}

        public InventoryManager() { }

        public void DropItem() {
            // Calling `drop_item` on the `CarriableEnemyGoalItem` node.
            Held_item?.Call("drop_item");

            Held_item = null;
        }

        public Marker3D GetCarryPosition() {
            return Carry_position;
        }

        public GodotObject GetHeldItem(){
            return Held_item;
        }

        public void PickupItem(GodotObject item) {
            Held_item = item;
        }

        public void SetupInventory(CharacterBody3D _inventory_owner) {
            inventory_owner = _inventory_owner;
        }
    }
}