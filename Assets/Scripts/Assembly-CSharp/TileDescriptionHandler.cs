using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Tilemaps;
using System.Windows.Forms;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Assembly_CSharp
{
    public class TileDescriptionHandler : MonoBehaviour
    {
        
        public GameObject uiPanel;
        public RectTransform uiPanelTransform;

        public Text name_Box;
        public Text description_Box;

        public void Start(){Instance = this; if (uiPanel.activeInHierarchy == true) { uiPanel.SetActive(false); }
            uiPanelTransform = this.GetComponent<RectTransform>();
        }

        public void OnTileHover(Tile tile)
        {
            var select = tileSecriptions.Where(self => self.tileName == tile.name);
            if (select.Count() > 0)
            {
                var entry = select.First();
                if (uiPanel.activeInHierarchy == false)  {uiPanel.SetActive(true);}
                name_Box.text = entry.DisplayName;
                description_Box.text = entry.Description;
                //uiPanelTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 300);
                uiPanelTransform.sizeDelta = new Vector2(Mathf.Max(name_Box.preferredWidth + 48, 300), description_Box.preferredHeight + 96);
            }
        }
        //GetComponent<RectTransform>().sizeDelta.x
        public void OnTileExit(Tile tile)
        {
            if (uiPanel != null)
            {
                uiPanel.SetActive(false);
            }
        }


        public static TileDescriptionHandler Instance;

        public static List<TileDescription> tileSecriptions = new List<TileDescription>()
        {
            new TileDescription(){ tileName = "ice", DisplayName = "Ice", Description = "Slippery tiles that make the player slide around.\nOnly functions in the Hollow."},
            new TileDescription(){ tileName = "effecthazard", DisplayName = "Hazard Tile", Description = "In the Mines, players standing on this tile will be poisoned.\nIn the Forge, players standing on this tile will be set on fire.\nA normal floor tile otherwise."},
            new TileDescription(){ tileName = "pit", DisplayName = "Pit", Description = "A hole in the ground the player can fall into."},
            new TileDescription(){ tileName = "diagonal_NE", DisplayName = "Diagonal Wall", Description = "A diagonal wall. Only functions on some visual subtypes in the Keep, Gungeon Proper and the Hollow.\nPlaces glitched tiles otherwise."},
            new TileDescription(){ tileName = "diagonal_NW", DisplayName = "Diagonal Wall", Description = "A diagonal wall. Only functions on some visual subtypes in the Keep, Gungeon Proper and the Hollow.\nPlaces glitched tiles otherwise."},
            new TileDescription(){ tileName = "diagonal_SE", DisplayName = "Diagonal Wall", Description = "A diagonal wall. Only functions on some visual subtypes in the Keep, Gungeon Proper and the Hollow.\nPlaces glitched tiles otherwise."},
            new TileDescription(){ tileName = "diagonal_SW", DisplayName = "Diagonal Wall", Description = "A diagonal wall. Only functions on some visual subtypes in the Keep, Gungeon Proper and the Hollow.\nPlaces glitched tiles otherwise."},
            new TileDescription(){ tileName = "lord_of_the_jammed", DisplayName = "Lord Of The Jammed [Entity]", Description = "The place-holder version of the Lord Of The Jammed unlike the one encountered ingame.\n\nHas a hitbox and deals contact damage."},
            new TileDescription(){ tileName = "spent", DisplayName = "Spent", Description = "When the main Spent is killed, summons a few small additional waves of Spent to attack."},
            new TileDescription(){ tileName = "black_stache", DisplayName = "Black Stache", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "dr_wolfs_monster", DisplayName = "Dr Wolfs Monster", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "hm_absolution", DisplayName = "HM Absolution", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "interdimensional_horror", DisplayName = "Interdimensional Horror", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "agunim", DisplayName = "Agunim", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "cannon", DisplayName = "Cannon", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "last_human", DisplayName = "Last Human", Description = "[Warning]\n\nPast Boss. Will likely glitch out when killed."},
            new TileDescription(){ tileName = "candle_guy", DisplayName = "Candle guy", Description = "When killed, spawns a pool of fire that doesn't go out on its own."},
            new TileDescription(){ tileName = "ser_manuel", DisplayName = "Ser Manuel", Description = "[Warning]\n\nGlitches out if not spawned from a reinforcement wave."},
            new TileDescription(){ tileName = "tutorial_bullet_kin", DisplayName = "Friendly Bullet Kin", Description = "Friend. :)"},
            new TileDescription(){ tileName = "door_north", DisplayName = "Entrance / Exit Point", Description = "Green Arrow Points INTO rooms, Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this entrance / exit point will be used to see how the player can enter the room."},
            new TileDescription(){ tileName = "door_south", DisplayName = "Entrance / Exit Point", Description = "Green Arrow Points INTO rooms, Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this entrance / exit point will be used to see how the player can enter the room."},
            new TileDescription(){ tileName = "door_west", DisplayName = "Entrance / Exit Point", Description = "Green Arrow Points INTO rooms, Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this entrance / exit point will be used to see how the player can enter the room."},
            new TileDescription(){ tileName = "door_east", DisplayName = "Entrance / Exit Point", Description = "Green Arrow Points INTO rooms, Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this entrance / exit point will be used to see how the player can enter the room."},
            new TileDescription(){ tileName = "door_southEntryOnly", DisplayName = "Entrance Point", Description = "Green Arrow Points INTO rooms.\n\nWhen the room is generated, this point will NOT be able to lead into rooms, and can only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "door_eastEntryOnly", DisplayName = "Entrance Point", Description = "Green Arrow Points INTO rooms.\n\nWhen the room is generated, this point will NOT be able to lead into rooms, and can only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "door_westEntryOnly", DisplayName = "Entrance Point", Description = "Green Arrow Points INTO rooms.\n\nWhen the room is generated, this point will NOT be able to lead into rooms, and can only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "doornorthEntryOnly", DisplayName = "Entrance Point", Description = "Green Arrow Points INTO rooms.\n\nWhen the room is generated, this point will NOT be able to lead into rooms, and can only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "door_eastExitOnly", DisplayName = "Exit Point", Description = "Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this point will ONLY be able to lead into new rooms, and cannot only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "door_southExitOnly", DisplayName = "Exit Point", Description = "Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this point will ONLY be able to lead into new rooms, and cannot only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "door_westExitOnly", DisplayName = "Exit Point", Description = "Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this point will ONLY be able to lead into new rooms, and cannot only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "doornorthExitOnly", DisplayName = "Exit Point", Description = "Red Arrow Points OUT of rooms.\n\nWhen the room is generated, this point will ONLY be able to lead into new rooms, and cannot only connect from earlier points on the floor."},
            new TileDescription(){ tileName = "dead_blow", DisplayName = "Dead Blow", Description = "A Dead Blow.\nHas many properties."},
            new TileDescription(){ tileName = "firebar_trap", DisplayName = "Firebar Trap", Description = "Has 2 lines of bullets that rotate in a clock-wise formation.\nConfigurable."},
            new TileDescription(){ tileName = "flameburst_trap", DisplayName = "Flameburst Trap", Description = "Fires a ring of bullets occasionally.\nConfigurable."},
            new TileDescription(){ tileName = "pew", DisplayName = "Pew", Description = "Has configurable length."},
            new TileDescription(){ tileName = "winchesterController", DisplayName = "Winchester Room Controller", Description = "Required to be placed in a Winchester Room for Winchester to work."},
            new TileDescription(){ tileName = "winchesterCamera", DisplayName = "Winchester Camera Controller", Description = "When the player is standing on a camera-activating tile, pans out the camera, centered on the position of this object."},
            new TileDescription(){ tileName = "winchesterCameraPanPlacer", DisplayName = "Winchester Camera Panner", Description = "Marks tiles to activate the Winchester Camera Controller. Configurable to mark multiple tiles."},
            new TileDescription(){ tileName = "winchesterCameraPanPlacer", DisplayName = "Winchester Camera Panner", Description = "Marks tiles to activate the Winchester Camera Controller. Configurable to mark multiple tiles."},         
            new TileDescription(){ tileName = "random_snipershell_professional", DisplayName = "Randomized Enemy [Sniper/Professional]", Description = "Places either a Sniper Bullet Kin or a Proffessional into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_gunsinger", DisplayName = "Randomized Enemy [Gunsinger/None]", Description = "Places either a Gunsinger or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_pinhead", DisplayName = "Randomized Enemy [Pinhead/None]", Description = "Places either a Pinhead or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_rubberkin", DisplayName = "Randomized Enemy [Rubber Kin/None]", Description = "Places either a Rubber Bullet Kin or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_tier_blob", DisplayName = "Randomized Enemy [Blobulon/Blobuloid/Blobulin]", Description = "Places either a Blobulon, Blobuloid or a Blobulin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_tier_poisonblob", DisplayName = "Randomized Enemy [Poisbulon/Poisbuloid/Poisbulin]", Description = "Places either a Poisbulon, Poisbuloid or a Poisbulin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_red_blue_vet_shotgun", DisplayName = "Randomized Enemy [Red/Blue/Veteran Shotgun Kin]", Description = "Places either a Red Shotgun Kin, Blue Shotgun Kin or a Veteran Shotgun Kin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_red_blue_vet_shotgun", DisplayName = "Randomized Enemy [Blue Shotgun Kin/Tarnisher/None]", Description = "Places either a Blue Shotgun Kin, Tarnisher or nothing Kin into the room when the room is generated"},
            new TileDescription(){ tileName = "random_redshotgun_bulletkin", DisplayName = "Randomized Enemy [Red Shotgun Kin/Bullet Kin]", Description = "Places either a Red Shotgun Kin or a Bullet Kin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_rubberkin_tazie", DisplayName = "Randomized Enemy [Rubber Bullet Kin/Tazie]", Description = "Places either a Rubber Bullet Kin or a Tazie into the room when the room is generated."},
            new TileDescription(){ tileName = "random_kingbullat_chancebulon", DisplayName = "Randomized Enemy [Chancebulon/King Bullat]", Description = "Places either a Chancebulon or a King Bullat into the room when the room is generated."},
            new TileDescription(){ tileName = "random_hollowpoint_appgunjurer", DisplayName = "Randomized Enemy [Hollowpoint/Apprentice Gunjurer]", Description = "Places either a Hollowpoint or a Apprentice Gunjurer into the room when the room is generated."},
            new TileDescription(){ tileName = "random_gunnut_leadmaiden", DisplayName = "Randomized Enemy [Gun Nut/Lead Maiden]", Description = "Places either a Gun Nut or a Lead Maiden into the room when the room is generated."},
            new TileDescription(){ tileName = "random_gripmaster_redshotgun_blueshotgun", DisplayName = "Randomized Enemy [Red Shotgun Kin/Gripmaster/Blue Shotgun Kin]", Description = "Places either a Red Shotgun Kin, Gripmaster or a Blue Shotgun Kin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_cubulon_wizbang_skullet", DisplayName = "Randomized Enemy [Cubulon/Wizbang/Skullet]", Description = "Places either a Cubulon, Wizbang or a Skullet into the room when the room is generated."},
            new TileDescription(){ tileName = "random_cubulon_cubulead", DisplayName = "Randomized Enemy [Cubulon/Leadbulon]", Description = "Places either a Cubulon or a Leadbulon into the room when the room is generated."},
            new TileDescription(){ tileName = "random_cubulon_appgunjurer", DisplayName = "Randomized Enemy [Cubulon/Apprentice Gunjurer]", Description = "Places either a Cubulon or an Apprentice Gunjurer into the room when the room is generated."},
            new TileDescription(){ tileName = "random_bulletkin_skullet", DisplayName = "Randomized Enemy [Skullet/Bullet Kin]", Description = "Places either a Skullet or a Bullet Kin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_bulletkin", DisplayName = "Randomized Enemy [Bullet Kin/Minelet/Red Shotgun Kin]", Description = "Places either a Bullet Kin, Minelet or a Red Shotgun Kin into the room when the room is generated."},
            new TileDescription(){ tileName = "random_bullat", DisplayName = "Randomized Enemy [Bullats]", Description = "Places a random Bullat into the room when the room is generated."},
            new TileDescription(){ tileName = "random_bookllet_gigi_appgunjurer", DisplayName = "Randomized Enemy [Red Booklet/Gigi/Apprentice Gunjurer]", Description = "Places either a Red Booklet, Gigi or a Apprentice Gunjurer into the room when the room is generated."},
            new TileDescription(){ tileName = "random_bookllet", DisplayName = "Randomized Enemy [Red/Green/Blue Booklet]", Description = "Places either a Red Booklet, Green Booklet or a Blue Booklet into the room when the room is generated."},
            new TileDescription(){ tileName = "random_blobulon_pinhead", DisplayName = "Randomized Enemy [Blobulon/Pinhead]", Description = "Places either a Blobulon or a Pinhead into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_tarnisher_blueshotgun", DisplayName = "Randomized Enemy [Blue Shotgun Kin/Gripmaster/None]", Description = "Places either a Blue Shotgun Kin, Gripmaster or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_gripmaster", DisplayName = "Randomized Enemy [Gripmaster/None]", Description = "Places either a Gripmaster or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_gunsinger", DisplayName = "Randomized Enemy [Gunsinger/None]", Description = "Places either a Gunsinger or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_pinhead", DisplayName = "Randomized Enemy [Gripmaster/None]", Description = "Places either a Gripmaster or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_sometimes_rubberkin", DisplayName = "Randomized Enemy [Rubber Bullet Kin/None]", Description = "Places either a Rubber Bullet Kin or nothing into the room when the room is generated."},
            new TileDescription(){ tileName = "random_medium_mines_enemy", DisplayName = "Randomized Medium Enemy [Mines]", Description = "Places a random Mines-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_medium_mines_enemy", DisplayName = "Randomized Medium Enemy [Mines]", Description = "Places a random medium Mines-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_medium_keep_enemy", DisplayName = "Randomized Medium Enemy [Keep]", Description = "Places a random medium Keep-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_medium_gungeon_enemy", DisplayName = "Randomized Medium Enemy [Gungeon Proper]", Description = "Places a random medium Gungeon Proper-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_easy_gungeon_enemy", DisplayName = "Randomized Easy Enemy [Gungeon Proper]", Description = "Places a random easy Gungeon Proper-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_easy_keep_enemy", DisplayName = "Randomized Easy Enemy [Keep]", Description = "Places a random easy Keep-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_hard_gungeon_enemy", DisplayName = "Randomized Hard Enemy [Gungeon Proper]", Description = "Places a random Hard Gungeon Proper-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "random_hard_keep_enemy", DisplayName = "Randomized Hard Enemy [Keep]", Description = "Places a random Hard Keep-tier enemy into the room when the room is generated."},
            new TileDescription(){ tileName = "floor_note", DisplayName = "Secret Room Note", Description = "A readable note with random messages.\nCan be configured to say whatever you want."},
            new TileDescription(){ tileName = "chandelier_trap", DisplayName = "Chandelier Drop Trap", Description = "A droppable Chandelier. This chandelier will be dropped when a switch with the same Event value it has is triggered."},
            new TileDescription(){ tileName = "chandelier_switch", DisplayName = "Chandelier Switch", Description = "This switch will drop any droppable chandeliers with the same Event Value as itself.\nIf multiple switches are placed with the same Event Value, only 1 of them will generate."},
            new TileDescription(){ tileName = "tnt_drop", DisplayName = "Cane-In", Description = "A Cane-In. This cave-in will trigger when a switch with the same Event value it has is triggered."},
            new TileDescription(){ tileName = "tnt_plunger_idle_001", DisplayName = "TNT Plunger", Description = "This switch will trigger any cave-ins with the same Event Value as itself.\nIf multiple switches are placed with the same Event Value, only 1 of them will generate."},
            new TileDescription(){ tileName = "glitch_floor_properties", DisplayName = "Glitchifier", Description = "Should only be used in rooms intended to appear on Glitched floors.\nSets the current floor to be glitched, and applies a glitch shader and several properties to all enemies in the room."},
            new TileDescription(){ tileName = "gull_land_point", DisplayName = "Gatling Gull Land Point", Description = "Used by the Gatling Gull to signify where it can jump to for certain attacks."},
            new TileDescription(){ tileName = "flame_pipe_east", DisplayName = "Forge Pipe", Description = "Fires a temporary short-range beam when shot."},
            new TileDescription(){ tileName = "flame_pipe_north", DisplayName = "Forge Pipe", Description = "Fires a temporary short-range beam when shot."},
            new TileDescription(){ tileName = "flame_pipe_west", DisplayName = "Forge Pipe", Description = "Fires a temporary short-range beam when shot."},
            new TileDescription(){ tileName = "minecart", DisplayName = "Minecart", Description = "A rideable minecart that moves along its assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "minecartboomer", DisplayName = "Explosive Minecart", Description = "A minecart with an explosive barrel that moves along its assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "minecartturret", DisplayName = "Turret Minecart", Description = "A minecart that starts firing shortly after entering combat, and stops once combat ends. Automatically moves along its assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "minecart_spawner", DisplayName = "Minecart Spawner", Description = "Spawns minecarts of various types on an assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "lightbulbThankYouNevernamed", DisplayName = "Custom Light Object", Description = "A very configurable light source.\nChange its radius, intensity and color with the Property tool."},
            new TileDescription(){ tileName = "sawblade", DisplayName = "Sawblade", Description = "A sawblade that travels along its assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "winchester_target_001", DisplayName = "Winchester Target", Description = "A Winchester Target used in Winchesters Minigame that follows an assigned Node Path.\nConfigurable.\n\nNote: you can have more than / less than 4 Targets present."},
            new TileDescription(){ tileName = "winchestermovingBumper1x3", DisplayName = "Winchester Moving Block [1X3]", Description = "A moving set of blocks used in Winchesters Minigame that follows an assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "winchestermovingBumper2x2", DisplayName = "Winchester Moving Block [2X2]", Description = "A moving set of blocks used in Winchesters Minigame that follows an assigned Node Path.\nConfigurable."},
            new TileDescription(){ tileName = "truth_chest", DisplayName = "Truth Chest", Description = "Only unlocked by Brother Albern."},
            new TileDescription(){ tileName = "lost_adventurer_idle_left_001", DisplayName = "Lost Adventurer", Description = "Very slowly moves along his assigned Node Path."},
            new TileDescription(){ tileName = "head_skull_001", DisplayName = "Dragun Skull", Description = "Decorative variant, will not drop anything."},
            new TileDescription(){ tileName = "challengeShrine", DisplayName = "Challenge Shrine", Description = "Summons reinforcement waves and spawns a chest once up to 3 have been cleared. The default reinforcement wave [Wave 0] in the room has to have NO enemies for this to work properly."},
            new TileDescription(){ tileName = "mine_column_idle_001", DisplayName = "Large Column", Description = "Only destroyable by the Treadnought."},
            new TileDescription(){ tileName = "random_sarcophagus", DisplayName = "Random Sarcophagus", Description = "Places down a random sarcophagus when the room is generated on a floor."},
            new TileDescription(){ tileName = "random_shrine", DisplayName = "Random Shrine", Description = "Places down a random shrine when the room is generated on a floor."},
            new TileDescription(){ tileName = "spinning_ice_log_spike_horizontal_001001", DisplayName = "Rolling Log [Horizonal Movement]", Description = "A rolling log that follows an assigned Node Path.\nConfigurable, vertical size can be changed."},
            new TileDescription(){ tileName = "spinning_log_spike_horizontal_001", DisplayName = "Rolling Log [Horizonal Movement]", Description = "A rolling log that follows an assigned Node Path.\nConfigurable, vertical size can be changed."},
            new TileDescription(){ tileName = "spinning_ice_log_spike_vertical_001", DisplayName = "Rolling Log [Vetical Movement]", Description = "A rolling log that follows an assigned Node Path.\nConfigurable, horizontal size can be changed."},
            new TileDescription(){ tileName = "spinning_log_spike_vertical_001", DisplayName = "Rolling Log [Vetical Movement]", Description = "A rolling log that follows an assigned Node Path.\nConfigurable, horizontal size can be changed."},
            new TileDescription(){ tileName = "Boss_Pedestal", DisplayName = "Boss Pedestal", Description = "A Boss Pedestal, that can have items on it.\nConfigurable."},
            new TileDescription(){ tileName = "artfull_dodger_talk_002", DisplayName = "Winchester", Description = "Lets you play his game.\nConfigurable."},
            new TileDescription(){ tileName = "blobulord_sewer_grate_001", DisplayName = "Blobulord Grate", Description = "Contrary to popular belief, it's purely decorative."},
            new TileDescription(){ tileName = "conveyor_belt_right", DisplayName = "Conveyor Belt [Horizontal]", Description = "Pushes the player in a certain direction.\nConfigurable."},
            new TileDescription(){ tileName = "conveyor_belt_up", DisplayName = "Conveyor Belt [Vertical]", Description = "Pushes the player in a certain direction.\nConfigurable."},
            new TileDescription(){ tileName = "custom_barrel", DisplayName = "Custom Barrel", Description = "A heavily configurable barrel."},
            new TileDescription(){ tileName = "default_door_horizontal", DisplayName = "Door", Description = "A door you can walk through. Normally, enemies cannot open these."},
            new TileDescription(){ tileName = "default_door_vertical", DisplayName = "Door", Description = "A door you can walk through. Normally, enemies cannot open these."},
            new TileDescription(){ tileName = "firePlace", DisplayName = "Fireplace", Description = "When its fire is extinguished and the switch is flipped, reveals all valid secret rooms of a certain type, namely the entrance to the Oubliette."},
            new TileDescription(){ tileName = "floor_spikes", DisplayName = "Floor Spikes", Description = "Shoot out spikes when you step on them.\nConfigurable."},
            new TileDescription(){ tileName = "flame_trap", DisplayName = "Flame Trap", Description = "Occasionally shoots out jets of fire.\nConfigurable."},
            new TileDescription(){ tileName = "gungon_lair_trap", DisplayName = "Bullet Past Pitfall Trap", Description = "When stepped on, slowly breaks until it collapses, permanently leaving behind a pit.\nConfigurable."},
            new TileDescription(){ tileName = "high_dragunfire_chest", DisplayName = "Dragunfire Chest", Description = "Now you can finally unlock the damn thing."},
            new TileDescription(){ tileName = "jail_cell_door", DisplayName = "Jail Cell Door", Description = "Requires a Cell Key to open. If this door is present, the Cell Key is forced to be dropped by a random enemy on the floor, usually a boss."},
            new TileDescription(){ tileName = "locked_door", DisplayName = "Cell Door", Description = "Requires a Key to open."},
            new TileDescription(){ tileName = "lonks_backpack", DisplayName = "Lost Adventurers Backpack", Description = "Annoys the Lost Adventurer when interacted with."},
            new TileDescription(){ tileName = "lonks_shield", DisplayName = "Lost Adventurers Shield", Description = "Annoys the Lost Adventurer when interacted with."},
            new TileDescription(){ tileName = "lonks_sword", DisplayName = "Lost Adventurers Sword", Description = "Annoys the Lost Adventurer when interacted with."},
            new TileDescription(){ tileName = "mouse_trap_east", DisplayName = "Mouse Trap", Description = "Hurts when stepped on, breaks after being stepped on or after a blank is triggered.\nConfigurable."},
            new TileDescription(){ tileName = "mouse_trap_north", DisplayName = "Mouse Trap", Description = "Hurts when stepped on, breaks after being stepped on or after a blank is triggered.\nConfigurable."},
            new TileDescription(){ tileName = "mouse_trap_west", DisplayName = "Mouse Trap", Description = "Hurts when stepped on, breaks after being stepped on or after a blank is triggered.\nConfigurable."},
            new TileDescription(){ tileName = "pitfall_trap", DisplayName = "Pitfall Trap", Description = "Falls away when stepped on, revealing a pit. Resets to a solid position after a few seconds.\nConfigurable."},
            new TileDescription(){ tileName = "random_npc", DisplayName = "Random NPC", Description = "Places a random NPC in the room when the floor is generated."},
            new TileDescription(){ tileName = "random_pickup", DisplayName = "Random Pickup", Description = "Places a random pickup in the room when the floor is generated."},
            new TileDescription(){ tileName = "rock", DisplayName = "Rock", Description = "And Stone!"},
            new TileDescription(){ tileName = "sewerGrate", DisplayName = "Oubliette Entrance", Description = "Unlocked with 2 keys. Once opened, brings the player to the Oubliette when the player falls in."},
            new TileDescription(){ tileName = "winchester_bumper_greed_001", DisplayName = "Green Winchester Block", Description = "Falls away when hit."},
            new TileDescription(){ tileName = "winchester_bumper_red_001", DisplayName = "Red Winchester Block", Description = "Instantly destroys the Winchester projectile when hit."},
            new TileDescription(){ tileName = "vertical_crusher", DisplayName = "Crusher", Description = "When stepped into, crushes anything in it after a small delay, and resets after a few seconds.\nConfigurable."},
            new TileDescription(){ tileName = "horizontal_crusher", DisplayName = "Crusher", Description = "When stepped into, crushes anything in it after a small delay, and resets after a few seconds.\nConfigurable."},
            new TileDescription(){ tileName = "gungeon_platform_moving_001", DisplayName = "Moving Platform [Gungeon Proper]", Description = "A platform the player can stand on that moves along his assigned Node Path.\nConfigurable.\n\nNote: position nodes to where it'll travel based on the bottom-left corner of the platform."},
            new TileDescription(){ tileName = "catacombs_platform_moving_001", DisplayName = "Moving Platform [Hollow]", Description = "A platform the player can stand on that moves along his assigned Node Path.\nConfigurable.\n\nNote: position nodes to where it'll travel based on the bottom-left corner of the platform."},
            new TileDescription(){ tileName = "forge_platform_moving_001", DisplayName = "Moving Platform [Forge]", Description = "A platform the player can stand on that moves along his assigned Node Path.\nConfigurable.\n\nNote: position nodes to where it'll travel based on the bottom-left corner of the platform."},
            new TileDescription(){ tileName = "mines_platform_moving_001", DisplayName = "Moving Platform [Mines]", Description = "A platform the player can stand on that moves along his assigned Node Path.\nConfigurable.\n\nNote: position nodes to where it'll travel based on the bottom-left corner of the platform."},
            new TileDescription(){ tileName = "sewer_platform_moving_001", DisplayName = "Moving Platform [Oubliette]", Description = "A platform the player can stand on that moves along his assigned Node Path.\nConfigurable.\n\nNote: position nodes to where it'll travel based on the bottom-left corner of the platform."},
            new TileDescription(){ tileName = "black_skusket", DisplayName = "Black Skusket", Description = "An unused, very fast skusket that occasionally fires a bullet at the player. Normally has very high health."},
            new TileDescription(){ tileName = "boss_template", DisplayName = "Boss Template", Description = "A template for the Gungeon devs to build other bosses off of."},
            new TileDescription(){ tileName = "bunker", DisplayName = "Bunker", Description = "Unused, yet just about finished."},
            new TileDescription(){ tileName = "det", DisplayName = "Det", Description = "Det. Stand-still version."},
            new TileDescription(){ tileName = "diagonal_det", DisplayName = "Det", Description = "Det. Moves on a diagonal path, bounces off of walls."},
            new TileDescription(){ tileName = "horizontal_det", DisplayName = "Det", Description = "Det. Moves on a horizontal path, bounces off of walls."},
            new TileDescription(){ tileName = "vertical_det", DisplayName = "Det", Description = "Det. Moves on a horizontal path, bounces off of walls."},
            new TileDescription(){ tileName = "x_det", DisplayName = "X Det", Description = "X Det. Stand-still version."},
            new TileDescription(){ tileName = "diagonal_x_det", DisplayName = "X Det", Description = "X Det. Moves on a diagonal path, bounces off of walls."},
            new TileDescription(){ tileName = "horizontal_x_det", DisplayName = "X Det", Description = "X Det. Moves on a horizontal path, bounces off of walls."},
            new TileDescription(){ tileName = "vertical_x_det", DisplayName = "X Det", Description = "X Det. Moves on a horizontal path, bounces off of walls."},
            new TileDescription(){ tileName = "dragun_knife_advanced", DisplayName = "Advanced Draguns Knife", Description = "Tries to stick to the nearest wall. Use with caution."},
            new TileDescription(){ tileName = "draguns_knife", DisplayName = "Draguns Knife", Description = "Tries to stick to the nearest wall. Use with caution."},
            new TileDescription(){ tileName = "tutorial_turret", DisplayName = "Tutorial Turret", Description = "A turret used only in the tutorial. Shoots occasionally."},
            new TileDescription(){ tileName = "faster_tutorial_turret", DisplayName = "Fast Tutorial Turret", Description = "A turret used only in the tutorial. Shoots very fast."},
            new TileDescription(){ tileName = "skusket_head", DisplayName = "Skusket Head", Description = "Weak. Bounces towards the player."},
            new TileDescription(){ tileName = "test_dummy", DisplayName = "Test Dummy", Description = "Cannot attack, and has over a million HP."},
            new TileDescription(){ tileName = "wallmonger", DisplayName = "Wallmonger", Description = "Can crush the player, killing them. Make your arena long enough."},
            new TileDescription(){ tileName = "resourceful_rat_mech", DisplayName = "Rat Mech", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "resourceful_rat", DisplayName = "Resourceful Rat", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "megalich", DisplayName = "Megalich", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "lich", DisplayName = "Lich", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "infinilich", DisplayName = "Infinilich", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "dragun", DisplayName = "Dragun", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "dragun_advanced", DisplayName = "Advanced Dragun", Description = "[Warning]\n\nUse with caution, can potentially cause issues."},
            new TileDescription(){ tileName = "mine_flayers_bell", DisplayName = "Mine Flayer Bell", Description = "Walks around, not required to be killed to leave the room. Fires a ring of bullets when killed."},
            new TileDescription(){ tileName = "mine_flayers_claymore", DisplayName = "Mine Flayer Claymore", Description = "Walks around, not required to be killed to leave the room. Explodes in proximity to the player."},
            new TileDescription(){ tileName = "misfire_beast", DisplayName = "Misfire Beast", Description = "MISTER BEEEAST!!!"},
            new TileDescription(){ tileName = "metal_cube_guy", DisplayName = "Cube", Description = "Unused. Moves like Mountain cubes / Lead Cubes."},
            new TileDescription(){ tileName = "misfire_beast", DisplayName = "Misfire Beast", Description = "MISTER BEEEAST!!!"},
            new TileDescription(){ tileName = "black_chest_mimic", DisplayName = "Black Chest Mimic", Description = "Does not drop items."},
            new TileDescription(){ tileName = "blue_chest_mimic", DisplayName = "Blue Chest Mimic", Description = "Does not drop items."},
            new TileDescription(){ tileName = "brown_chest_mimic", DisplayName = "Brown Chest Mimic", Description = "Does not drop items."},
            new TileDescription(){ tileName = "green_chest_mimic", DisplayName = "Green Chest Mimic", Description = "Does not drop items."},
            new TileDescription(){ tileName = "pedestal_mimic", DisplayName = "Pedestal Mimic", Description = "Does not drop items."},
            new TileDescription(){ tileName = "rat_chest_mimic", DisplayName = "Rat Chest Mimic", Description = "Does not drop items."},
            new TileDescription(){ tileName = "wall_mimic", DisplayName = "Wall Mimic", Description = "Does not drop items."},

        };
        public class TileDescription
        {
            public string tileName;


            public string DisplayName = "Name";
            public string Description = "Boring Ass Description";
        }
    }
}
