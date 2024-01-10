/*using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using System.Collections.Generic;
using System.Linq;
using xTile;
using xTile.Tiles;

namespace AutomaticGates
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry_old
    {

        Vector2 old_player_loc = Vector2.Zero;
        Vector2 player_loc;
        Dictionary<Fence, bool> opened_fences = new Dictionary<Fence, bool>();

        *//*********
        ** Public methods
        *********//*
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        /// 
        public void Entry(IModHelper helper)
        {
            //helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.GameLoop.UpdateTicked += this.PrintLocation;
        }

        private static string ListToString<T>(List<T> list) 
        {
            List<string> names = new List<string>();

            foreach (T element in list)
            {
                names.Add(element.ToString());
            }

            return string.Concat(names.ToArray());
        }
        private void PrintLocation(object sender, UpdateTickedEventArgs e)
        {

            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            player_loc = Game1.player.getTileLocation();

            if (player_loc != old_player_loc) {
                old_player_loc = player_loc;

                List<Vector2> adjacent_location = Utility.getAdjacentTileLocations(player_loc);
                adjacent_location.Add(player_loc);

                foreach (Fence fence in opened_fences.Keys)
                {
                    opened_fences[fence] = false;
                }

                foreach (Vector2 location in adjacent_location) 
                {
                    Object found_object = (Game1.getLocationFromName("Farm").getObjectAtTile((int)location.X, (int)location.Y));

                    if (found_object != null && found_object.name == "Gate" && found_object is Fence fence) {
                        if (!opened_fences.ContainsKey(fence)) {
                            fence.toggleGate(Game1.player, open: true);
                            opened_fences.Add(fence, true);
                        } else {
                            opened_fences[fence] = true;
                        }
                    }       
                }

                foreach (Fence opened_fence in opened_fences.Keys)
                {
                    if (opened_fences[opened_fence] == false) {
                        opened_fence.toggleGate(Game1.player, open: false);
                        opened_fences.Remove(opened_fence);
                    }
                }

                //this.Monitor.Log(player_loc.ToString(), LogLevel.Debug);
                //this.Monitor.Log(ListToString(adjacent_location), LogLevel.Debug
                old_player_loc = player_loc;
            }
        }
        
        private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
        {
            // ignore if player hasn't loaded a save yet
            if (!Context.IsWorldReady)
                return;

            this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }
    }
}*/