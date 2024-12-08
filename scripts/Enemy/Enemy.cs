using System;
using System.Collections.Generic;
using System.Linq;
using Buffalobuffalo.scripts.GOAP;
using Buffalobuffalo.scripts.Items;
using Godot;

namespace Buffalobuffalo.scripts.Enemy {
    /// <summary> 
    /// Simple wrapper for the methods to be called on the Enemy.gd script.
    /// `_actor` is the Enemy.gd script
    /// </summary>
    public static class GDUtils {
        public static void SetTargetLocation(CharacterBody3D _actor, Vector3 location)
        {
            _actor.Call("set_target_location", location);
        }

        public static InventoryManager GetInventoryManager(CharacterBody3D _actor) {
            return (InventoryManager) _actor.Call("_get_inventory_manager");
        }

        public static void SetFollowPath(CharacterBody3D _actor, bool should_follow) {
            _actor.Call("_follow_path", should_follow);
        }

        public static NavigationAgent3D GetNavAgent(CharacterBody3D _actor) {
            return (NavigationAgent3D) _actor.Get("nav_agent");
        }
    }

    /// <summary>
    /// Debug Spawner helps us spawn certain things.
    /// On creation of the spawner, we need a node though since it's not attached.
    /// </summary>
    public class DebugSpawner {
        Node world_node;
        public DebugSpawner(Node _node) {
            var current_parent = _node;
            // Looping up until we get to the world.
            while (current_parent.GetParent() != null) {
                current_parent = current_parent.GetParent();
            }
            world_node = current_parent;
        }
        public void Spawn_rectangle(Vector3 location, Color color) {
            var spawner = world_node.GetNode("/root/DebugSpawner");
            spawner.Call("spawn_rectangle", location, color);
        }
    }

    /// <summary>
    /// A more generic util that is for interacting with a GoapAgent 
    /// </summary>
    public class AgentUtils {
        public static Vector3 GetEscapeLocation(GoapAgent agent, Node3D target_to_avoid) {
            return GetEscapeLocation(agent, target_to_avoid, 10);
        }
        /// <summary>
        /// Using the navmesh we find the best location for the actor to escape to.
        /// </summary>
        public static Vector3 GetEscapeLocation(GoapAgent agent, Node3D target_to_avoid, int distance_to_check) {
            var navagent = GDUtils.GetNavAgent(agent.Actor);
            var map_rid = NavigationServer3D.AgentGetMap(navagent.GetRid());

            // Check all the way around the actor.
            var angles_to_check = new List<float>{0, 45, 90, 135, 180, 225, 270, 315};

            // List is (Position, score)
            var candidates = new List<(Vector3, float)>();
            foreach (var angle in angles_to_check) {
                float radians = Mathf.DegToRad(angle);
                // This direction gives us something like (.5, 0, -.5)
                var direction = new Vector3(MathF.Cos(radians), 0, Mathf.Sin(radians));
                // We then need to amplify it by a distance and add it to the actors global_position to get the new position.
                var potential_point = agent.Actor.GlobalPosition + (direction * distance_to_check);
                // Get the closest point on the navigation mesh to our potential point
                var target_point = NavigationServer3D.MapGetClosestPoint(map_rid, potential_point);

                // For score we're just getting the one that's farthest away from the player.
                var score = target_point.DistanceSquaredTo(target_to_avoid.GlobalPosition);

                candidates.Add((target_point, score));
            }

            // For the escape location, we're going to pick a random one from the top 3 positions
            var sorted = candidates.OrderByDescending(c => c.Item2).ToList();
            var random = new Random();
            var num = random.Next(0, 2);

            return sorted[num].Item1;
        }
    }
}