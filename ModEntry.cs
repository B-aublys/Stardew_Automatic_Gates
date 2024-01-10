using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Tiles;

namespace AutomaticGates
{
    internal sealed class ModEntry : Mod
    {

        struct Gate_data
        {
            private readonly List<Fence> fences;
            public List<Fence> get_fences { get { return fences; } }

            private readonly List<Vector2> open_loc;
            public List<Vector2> get_open_loc { get { return open_loc; } }

            public Gate_data(List<Fence> fences, List<Vector2> open_loc)
            {
                this.fences = fences;
                this.open_loc = open_loc;
            }
        }


        private List<Gate_data> current_gates = new List<Gate_data>();

        private Vector2 player_tile_loc = Vector2.Zero;
        private Vector2 old_player_tile_loc = Vector2.Zero;
        private List<Vector2> adjacent_locations = new List<Vector2>();

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += this.On_Update_ticked;
            helper.Events.Player.Warped += this.reset_gates;
        } 

        private void reset_gates(object sender, WarpedEventArgs e)
        {
           current_gates.Clear();
        }

        private bool is_new_fence(Fence fence)
        {
            foreach(Gate_data gate_data in current_gates)
            {
                if (gate_data.get_fences.Contains(fence))
                {
                    return false;
                }
            }
            return true;
        }

        private void add_new_gate(Fence fence, Vector2 loc)
        {
            List<Fence> fence_list = new List<Fence>();
            List<Vector2> loc_list = Utility.getAdjacentTileLocations(loc);

            loc_list.Add(loc);
            fence_list.Add(fence);


            foreach (Vector2 location in Utility.getAdjacentTileLocations(loc))
            {
                Object found_object = Game1.getLocationFromName("Farm").getObjectAtTile((int)location.X, (int)location.Y);

                if (found_object is Fence double_fence && double_fence.isGate.Value)
                {
                    Vector2 checking_location = (location - loc) * 2 + loc;
                    Object potential_fence = Game1.currentLocation.getObjectAtTile((int)checking_location.X, (int)checking_location.Y);

                    if (potential_fence is Fence found_fence && !found_fence.isGate.Value)
                    {
                        fence_list.Add(double_fence);
                        loc_list.AddRange(Utility.getAdjacentTileLocations(location));
                        loc_list.Add(location);
                    }
                }
            }

            current_gates.Add(new Gate_data(fence_list, loc_list));
            fence.toggleGate(Game1.player, true);
        }

        private void close_uneeded_gates()
        {
            for (int i = current_gates.Count - 1; i >= 0; i--)
            {
                if (current_gates[i].get_fences == null || current_gates[i].get_fences.Contains(null))
                {
                    current_gates.RemoveAt(i);
                }
                if (!current_gates[i].get_open_loc.Contains(player_tile_loc))
                {
                    current_gates[i].get_fences[0].toggleGate(Game1.player, false);
                    current_gates.RemoveAt(i);
                }
            }
        }

        private void On_Update_ticked(object sender, UpdateTickedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            // Get player location
            player_tile_loc = Game1.player.getTileLocation();

            // If the player has changed his location check if gates need to be opened/closed
            if (player_tile_loc != old_player_tile_loc)
            {
                old_player_tile_loc = player_tile_loc;
                adjacent_locations = Utility.getAdjacentTileLocations(player_tile_loc);
                
                foreach (Vector2 location in adjacent_locations)
                {
                    Object found_object = (Game1.currentLocation.getObjectAtTile((int)location.X, (int)location.Y));

                    if (found_object is Fence fence && fence.isGate.Value)
                    {
                       if(is_new_fence(fence))
                        {
                            add_new_gate(fence, location);
                        }
                    }      
                }

                close_uneeded_gates();
            }
        }
    }
}
