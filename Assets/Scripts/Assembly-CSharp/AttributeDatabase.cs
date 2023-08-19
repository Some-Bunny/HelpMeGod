using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

public static class AttributeDatabase
{
	
	public static bool TryGetListing(string name, out List<AC> listing)
	{
		listing = new List<AC>();
		foreach (KeyValuePair<AttributeDatabase.ValidAttribute, string[]> al in AttributeDatabase.attributeListings)
		{
			bool flag = al.Key(name);
			if (flag)
			{
				foreach (string shortName in al.Value)
				{
					listing.Add(AttributeDatabase.allAttributes[shortName]);
				}
				return true;
			}
		}
		return false;
	}
    public static object TryGetSpecialDefaults(string name, string longName)
    {
        foreach (var entry in AttributeDatabase.specialDefaultValueListings)
        {

            bool flag = entry.MatchingValue(longName, name);
            if (flag)
            {
                return entry.SpecialDefault;
            }
        }
        return null;
    }

    public static string LongToShortName(string longName)
	{
		return (from kvp in AttributeDatabase.allAttributes
		where kvp.Value.longName == longName
		select kvp).First<KeyValuePair<string, AC>>().Key;
	}

	
	public static JObject ToLongNamed(JObject jObject)
	{
		JObject ret = new JObject();
		foreach (KeyValuePair<string, JToken> kvp in jObject)
		{
			ret.Add(AttributeDatabase.allAttributes[kvp.Key].longName, kvp.Value);
		}
		return ret;
	}

	
	public static JObject ToShortNamed(JObject jObject)
	{
		JObject ret = new JObject();
		foreach (KeyValuePair<string, JToken> kvp in jObject)
		{
			ret.Add(AttributeDatabase.LongToShortName(kvp.Key), kvp.Value);
		}
		return ret;
	}


	public static Dictionary<string, AC> allAttributes = new Dictionary<string, AC>
	{
		{
			"j",
			new AC("jammed", "j", false, new object[0])
		},
		{
			"pls",
			new AC("prevent loot spawns", "pls", false, new object[0])
		},
		{
			"fP",
			new AC("followsPlayer", "fP", false, new object[0])
		},
		{
			"pOOC",
			new AC("persistOutOfCombat", "pOOC", true, new object[0])
		},
		{
			"fL",
			new AC("facesLeft", "fL", false, new object[0])
		},
		{
			"lG",
			new AC("leaveGoop", "lG", true, new object[0])
		},
		{
			"fB",
			new AC("fireBullets", "fB", true, new object[0])
		},
		{
			"gT",
			new AC("goopType", "gT", "fire", new object[]
			{
				"fire",
				"water",
				"poison",
				"oil",
				"blobulon",
				"web",
				"greenfire",
				"charm",
				"cheese"
			})
		},
		{
			"iD",
			new AC("initialDelay", "iD", 1f, new object[0])
		},
		{
			"minD",
			new AC("minDelay", "minD", 1f, new object[0])
		},
		{
			"maxD",
			new AC("maxDelay", "maxD", 1f, new object[0])
		},
		{
			"dI",
			new AC("overrideLootItem", "dI", "", new object[0])
		},
		{
			"jI",
			new AC("overrideJunkItem", "jI", "", new object[0])
		},
		{
			"mC",
			new AC("mimicChance", "mC", 0f, new object[0])
		},
		{
			"cL",
			new AC("locked", "cL", true, new object[0])
		},
		{
			"pV",
			new AC("forceNoFuse", "pV", false, new object[0])
		},
		{
			"bB",
			new AC("barrelSprite", "bB", "water_drum", new object[]
			{
				"water_drum",
				"red_explosive",
				"metal_explosive",
				"oil_drum",
				"poison_drum"
			})
		},
		{
			"rS",
			new AC("rollSpeed", "rS", 3f, new object[0])
		},
		{
			"tG",
			new AC("leavesGoopTrail", "tG", true, new object[0])
		},
		{
			"tGT",
			new AC("goopTrailType", "tGT", "fire", new object[]
			{
				"fire",
				"water",
				"poison",
				"oil",
				"blobulon",
				"web",
				"greenfire",
				"charm",
				"cheese"
			})
		},
		{
			"tGW",
			new AC("goopTrailSize", "tGW", 1f, new object[0])
		},
		{
			"pG",
			new AC("leavesGoopPuddle", "pG", true, new object[0])
		},
		{
			"pGT",
			new AC("goopPuddleType", "pGT", "fire", new object[]
			{
				"fire",
				"water",
				"poison",
				"oil",
				"blobulon",
				"web",
				"greenfire",
				"charm",
				"cheese"
			})
		},
		{
			"pGW",
			new AC("goopPuddleSize", "pGW", 3f, new object[0])
		},
		{
			"dBPR",
			new AC("destroyedByPlayerRoll", "dBPR", false, new object[0])
		},
		{
			"tSP",
			new AC("assignedPath", "tSP", "0", new object[] { "0" })
		},
		{
			"nSP_O",
			new AC("Start At Node:", "nSP_O", 0, new object[0])
		},
		{
			"mS",
			new AC("Max Speed", "mS", 9f, new object[0])
		},
		{
			"tTMS",
			new AC("timeToMaxSpeed", "tTMS", 1.5f, new object[0])
		},
		{
			"storedenemyBodyMC",
			new AC("Nearet Enemy Will Ride", "storedenemyBodyMC", false, new object[0])
		},

		{
			"nodPos",
			new AC("Node Order", "nodPos", "0", new object[]{ "0" })
		},
		{
			"nodType",
			new AC("Node Type", "nodType", "Center", new object[]{ "Center", "North", "NorthEast", "East", "SouthEast", "South", "SouthWest", "West", "NorthWest"})
		},
		{
			"lightRad",
			new AC("Light Radius", "lightRad", 2f, new object[0])
		},
		{
			"lightInt",
			new AC("Light Intensity", "lightInt", 2f, new object[0])
		},
		{
			"lightColorR",
			new AC("[Red]", "lightColorR", 1f, new object[0])
		},
		{
			"lightColorG",
			new AC("[Green]", "lightColorG", 1f, new object[0])
		},
		{
			"lightColorB",
			new AC("[Blue]", "lightColorB", 1f, new object[0])
		},

		{
			"bossPdstlItmID",
			new AC("Item ID", "bossPdstlItmID", -1, new object[0])
		},
		{
			"bossPdstlItmStringID",
			new AC("Item Tag", "bossPdstlItmStringID", "None.", new object[0])
		},//TRAPS only
        {
			"TrapTriggerMethod",
			new AC("Trap Trigger", "TrapTriggerMethod", "Timer", new object[] {"Timer", "Stepped On", "Collisions", "Script", })
		},
		{
			"TrapTriggerDelay",
			new AC("Cooldown", "TrapTriggerDelay", 1f, new object[0])
		},
		{
			"InitialtrapDelay",
			new AC("Initial Delay", "InitialtrapDelay", 1f, new object[0])
		},
        {
			"attackDelatTrap",
			new AC("Attack Delay", "attackDelatTrap", 0.5f, new object[0])
		},
		{
			"trapTriggerOnBlank",
			new AC("Attacks On Blank", "trapTriggerOnBlank", false, new object[]{false , true})
        },//TRAP END HERE
		{
            "WinchesterBounceCount",
            new AC("Amount Of Bounces", "WinchesterBounceCount", 1, new object[0])
        },
        {
            "WinCameraZoomOut",
            new AC("Zoom Scale", "WinCameraZoomOut", 0.75f, new object[0])
        },
        {
            "WinchestMoveXTele",
            new AC("Movement X:", "WinchestMoveXTele", 0f, new object[0])
        },
        {
            "WinchestMoveYTele",
            new AC("Movement Y:", "WinchestMoveYTele", 0f, new object[0])
        },
        {
            "WinchestGoneTime",
            new AC("Gone Time:", "WinchestGoneTime", 1f, new object[0])
        },
        {
            "WinchestTargetSpeed",
            new AC("Speed", "WinchestTargetSpeed", 6f, new object[0])
        },
        {
            "TileSizeX_",
            new AC("Tile Size X", "TileSizeX_", 4, new object[0])
        },
        {
            "TileSizeY_",
            new AC("Tile Size Y", "TileSizeY_", 4, new object[0])
        },
        {
            "ConveyorHorizontalVelocity",
            new AC("Horizontal Velocity", "ConveyorHorizontalVelocity", 4f, new object[0])
        },
        {
            "ConveyorVerticalVelocity",
            new AC("Vertical Velocity", "ConveyorVerticalVelocity", 4f, new object[0])
        },
        {
            "ConveyorReversed",
            new AC("Reversed", "ConveyorReversed", false, new object[0])
        },
        {
            "customNoteText",
            new AC("Custom Text", "customNoteText", "None.", new object[0])
        },
        {
            "customNoteTextIsStringKey",
            new AC("Text Is String Key", "customNoteTextIsStringKey", false, new object[0])
        },

        {
            "triggerEventValue",
            new AC("Trigger On Event:", "triggerEventValue", 0, new object[0])
        },
        {
            "triggeredEventValue",
            new AC("Set To Event:", "triggeredEventValue", 0, new object[0])
        },

        {
            "trapProjSpeed",
            new AC("Projectile Speed", "trapProjSpeed", 5f, new object[0])
        },
        {
            "trapProjRange",
            new AC("Projectile Range", "trapProjRange", 1000f, new object[0])
        },
        {
            "DirectionShoot",
            new AC("Shoot Direction", "DirectionShoot", "NORTH", new object[] { "NORTH", "NORTHEAST", "EAST", "SOUTHEAST", "SOUTH", "SOUTHWEST", "WEST", "NORTHWEST", })
        },
        {
            "projectileTypeTurret",
            new AC("Projectile Type", "projectileTypeTurret", "None.", new object[] { "None.","Bouncy.", "Explosive.","Tank Shell.", "Bouncy Bullet Kin.", "Grenade.", "Molotov.","Goblet." })
        },

        {
            "pewLength",
            new AC("Pew Length", "pewLength", 4, new object[0])
        },

        {
            "logLength",
            new AC("Length", "logLength", 4, new object[0])
        },
        {
            "logHeight",
            new AC("Height", "logHeight", 4, new object[0])
        },
        {
            "lf_pipe",
            new AC("Lifetime", "lf_pipe", 10f, new object[0])
        },
    };



    public static Dictionary<AttributeDatabase.ValidAttribute, string[]> attributeListings = new Dictionary<AttributeDatabase.ValidAttribute, string[]>
	{
        {
            (string name) => name == "sewer_platform_moving_001" |name == "gungeon_platform_moving_001" |name == "mines_platform_moving_001" |name == "catacombs_platform_moving_001" |name == "forge_platform_moving_001",
            new string[]
            {
                "tSP",
                "nSP_O",
                "mS",
                "TileSizeX_",
                "TileSizeY_",
            }
        },
        {
            (string name) => name == "flame_pipe_north"| name == "flame_pipe_west"| name == "flame_pipe_east",
            new string[]
            {
                "lf_pipe"
            }
        },
        {
            (string name) => name == "spinning_log_spike_vertical_001"| name == "spinning_ice_log_spike_vertical_001",
            new string[]
            {
                "tSP",
                "nSP_O",
                "logLength",
                "mS"
            }
        },
        {
            (string name) => name == "spinning_log_spike_horizontal_001" | name == "spinning_ice_log_spike_horizontal_001",
            new string[]
            {
                "tSP",
                "nSP_O",
                "logHeight",
                "mS"
            }
        },

       {
            (string name) => name == "mouse_trap_west" | name == "mouse_trap_east" |name == "mouse_trap_north",
            new string[]
            {
                "trapTriggerOnBlank"
            }
       },

       {
            (string name) => name == "pew",
            new string[]
            {
                "pewLength"
            }
       },
       {
            (string name) => name == "forge_shoot_face_north",
            new string[]
            {
                "TrapTriggerDelay",
                "trapProjSpeed",
                "trapProjRange",
                "DirectionShoot",
                "projectileTypeTurret"
            }
       },
       {
            (string name) => name == "forge_shoot_face_west",
            new string[]
            {
                "TrapTriggerDelay",
                "trapProjSpeed",
                "trapProjRange",
                "DirectionShoot",
                "projectileTypeTurret"
            }
       },
       {
            (string name) => name == "forge_shoot_face_east",
            new string[]
            {
                "TrapTriggerDelay",
                "trapProjSpeed",
                "trapProjRange",
                "DirectionShoot",
                "projectileTypeTurret"
            }
       },

       {
            (string name) => name == "chandelier_trap",
            new string[]
            {
                "triggerEventValue",
            }
       },
       {
            (string name) => name == "chandelier_switch",
            new string[]
            {
                "triggeredEventValue",
            }
       },

       {
            (string name) => name == "floor_note",
            new string[]
            {
                "customNoteText",
                "customNoteTextIsStringKey"
            }
       },
       {
            (string name) => name == "conveyor_belt_right",
            new string[]
            {
                "TileSizeX_",
                "TileSizeY_",
                "ConveyorHorizontalVelocity",
                "ConveyorVerticalVelocity",
                "ConveyorReversed"
            }
       },
       {
            (string name) => name == "conveyor_belt_up",
            new string[]
            {
                "TileSizeX_",
                "TileSizeY_",
                "ConveyorHorizontalVelocity",
                "ConveyorVerticalVelocity",
                "ConveyorReversed"
            }
       },

       {
            (string name) => name == "winchesterCameraPanPlacer",
            new string[]
            {
                "TileSizeX_",
                "TileSizeY_"
            }
       },
       {
            (string name) => name == "artfull_dodger_talk_002",
            new string[]
            {
                "WinchestMoveXTele",
                "WinchestMoveYTele",
                "WinchestGoneTime"
            }
        },
        {
            (string name) => name == "winchesterController",
            new string[]
            {
                "WinchesterBounceCount"
            }
        },
        {
            (string name) => name == "winchesterCamera",
            new string[]
            {
                "WinCameraZoomOut"
            }
        },
        {
			(string name) => name == "every_single_enemy_ever",
			new string[]
			{
				"j"
			}
		},
		{
			(string name) => name == "dead_blow",
			new string[]
			{
				"fP",
				"pOOC",
				"fL",
				"lG",
				"fB",
				"gT",
				"iD",
				"minD",
				"maxD"
			}
		},
        {
            (string name) => name == "all_nodes",
            new string[]
            {
                "nodPos",
                "nodType"
            }
        },
        {
			(string name) => name == "floor",
			new string[]
			{
				"pls"
			}
		},

		{
			(string name) => name.Contains("_chest") && !name.Contains("random"),
			new string[]
			{
				"dI",
				"jI",
				"mC",
				"cL",
				"pV"
			}
		},

		{
			(string name) => name == "lost_adventurer_idle_left_001",
			new string[]
			{
				"tSP",
                "nSP_O",
                "mS"
            }
		},
        {
            (string name) => name == "winchester_target_001",
            new string[]
            {
                "tSP",
                "nSP_O",
                "WinchestTargetSpeed"
            }
        },
		{ 
            (string name) => name == "winchestermovingBumper1x3",
            new string[]
            {
                "tSP",
                "nSP_O",
                "WinchestTargetSpeed"
            }
        },

        {
            (string name) => name == "winchestermovingBumper2x2",
            new string[]
            {
                "tSP",
                "nSP_O",
                "WinchestTargetSpeed"
            }
        },
        {
            (string name) => name == "sawblade",
            new string[]
            {
                "tSP",
                "nSP_O",
                "mS"
            }
        },
        {
			(string name) => name == "minecart",
			new string[]
			{
				"tSP",
                "nSP_O",
                "mS",
				"tTMS",
                "storedenemyBodyMC",
            }
		},
		
        {
            (string name) => name == "minecartturret",
            new string[]
            {
                "tSP",
                "nSP_O",
                "mS",
                "tTMS",
                "InitialtrapDelay",
                "TrapTriggerDelay"
            }
        },
        {
            (string name) => name == "minecartboomer",
            new string[]
            {
                "tSP",
                "nSP_O",
                "mS",
                "tTMS",
            }
        },
		{
            (string name) => name == "custom_barrel",
			new string[]
			{
				"bB",
				"rS",
				"tG",
				"tGT",
				"tGW",
				"pG",
				"pGT",
				"pGW",
				"dBPR"
			}
		},
        {
            (string name) => name == "lightbulbThankYouNevernamed",
            new string[]
            {
                "lightRad",
                "lightInt",
                "lightColorR",
                "lightColorG",
                "lightColorB",
            }
        },
        {
            (string name) => name == "Boss_Pedestal",
            new string[]
            {
                "bossPdstlItmID",
                "bossPdstlItmStringID"
            }
        },
        {
            (string name) => name == "floor_spikes",
            new string[]
            {
                "TrapTriggerMethod",
                "TrapTriggerDelay",
                "InitialtrapDelay",
                "attackDelatTrap",
                "trapTriggerOnBlank"
            }
        },
        {
            (string name) => name == "flame_trap",
            new string[]
            {
                "TrapTriggerMethod",
                "TrapTriggerDelay",
                "InitialtrapDelay",
                "attackDelatTrap",
                "trapTriggerOnBlank"         
			}
        },
        {
            (string name) => name == "pitfall_trap",
            new string[]
            {
                "TrapTriggerMethod",
                "TrapTriggerDelay",
                "InitialtrapDelay",
                "attackDelatTrap",
                "trapTriggerOnBlank"            
			}
        },
        {
            (string name) => name == "gungon_lair_trap",
            new string[]
            {
                "attackDelatTrap",
                "trapTriggerOnBlank"
            }
       },
    };

    public delegate bool ValidAttribute(string name);
	public static List<SpecialDefaultValue> specialDefaultValueListings = new List<SpecialDefaultValue>
	{

        new SpecialDefaultValue("attackDelatTrap", "gungon_lair_trap", 3f ){ },


        new SpecialDefaultValue("TrapTriggerMethod", "flame_trap", "Timer" ){ },


        new SpecialDefaultValue("TrapTriggerMethod", "flame_trap", "Timer" ){ },
        new SpecialDefaultValue("TrapTriggerMethod", "pitfall_trap", "Stepped On" ){ },
        new SpecialDefaultValue("TrapTriggerMethod", "floor_spikes", "Stepped On" ){ },

        new SpecialDefaultValue("TrapTriggerDelay", "flame_trap", 3f ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "pitfall_trap", 1f ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "floor_spikes", 1f ){ },

        new SpecialDefaultValue("InitialtrapDelay", "flame_trap", 1f ){ },
        new SpecialDefaultValue("InitialtrapDelay", "pitfall_trap", 1f ){ },
        new SpecialDefaultValue("InitialtrapDelay", "floor_spikes", 1f ){ },

        new SpecialDefaultValue("attackDelatTrap", "flame_trap", 1f ){ },
        new SpecialDefaultValue("attackDelatTrap", "pitfall_trap", 0.5f ){ },
        new SpecialDefaultValue("attackDelatTrap", "floor_spikes", 0.5f ){ },

        new SpecialDefaultValue("TileSizeY_", "conveyor_belt_right", 3 ){ },
        new SpecialDefaultValue("TileSizeX_", "conveyor_belt_up", 3 ){ },

        new SpecialDefaultValue("ConveyorHorizontalVelocity", "conveyor_belt_up", 0f ){ },
        new SpecialDefaultValue("ConveyorVerticalVelocity", "conveyor_belt_right", 0f ){ },


        new SpecialDefaultValue("InitialtrapDelay", "forge_shoot_face_west", 1f ){ },
        new SpecialDefaultValue("DirectionShoot", "forge_shoot_face_west", "SOUTH" ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "forge_shoot_face_west", 1 ){ },

        new SpecialDefaultValue("InitialtrapDelay", "forge_shoot_face_west", 1f ){ },
        new SpecialDefaultValue("DirectionShoot", "forge_shoot_face_west", "SOUTH" ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "forge_shoot_face_west", 1 ){ },

        new SpecialDefaultValue("DirectionShoot", "forge_shoot_face_north", "SOUTH" ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "forge_shoot_face_north", 1f ){ },

        new SpecialDefaultValue("DirectionShoot", "forge_shoot_face_west", "EAST" ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "forge_shoot_face_west", 1f ){ },

        new SpecialDefaultValue("DirectionShoot", "forge_shoot_face_east", "WEST" ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "forge_shoot_face_east", 1f ){ },

        new SpecialDefaultValue("InitialtrapDelay", "minecartturret", 3f ){ },
        new SpecialDefaultValue("TrapTriggerDelay", "minecartturret", 0.2f ){ },

         new SpecialDefaultValue("trapTriggerOnBlank", "mouse_trap_west", true ){ },
         new SpecialDefaultValue("trapTriggerOnBlank", "mouse_trap_east", true ){ },
         new SpecialDefaultValue("trapTriggerOnBlank", "mouse_trap_north", true ){ },


        new SpecialDefaultValue("mS", "lost_adventurer_idle_left_001", 0.1f ){ },
        new SpecialDefaultValue("mS", "spinning_log_spike_horizontal_001", 3f ){ },
        new SpecialDefaultValue("mS", "spinning_ice_log_spike_horizontal_001", 3f ){ },
        new SpecialDefaultValue("mS", "spinning_log_spike_vertical_001", 3f ){ },
        new SpecialDefaultValue("mS", "spinning_ice_log_spike_vertical_001", 3f ){ },

        new SpecialDefaultValue("TileSizeX_", "sewer_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("TileSizeY_", "sewer_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("mS", "sewer_platform_moving_001", 3f ){ },

        new SpecialDefaultValue("TileSizeX_", "gungeon_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("TileSizeY_", "gungeon_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("mS", "gungeon_platform_moving_001", 3f ){ },

        new SpecialDefaultValue("TileSizeX_", "mines_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("TileSizeY_", "mines_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("mS", "mines_platform_moving_001", 3f ){ },

        new SpecialDefaultValue("TileSizeX_", "catacombs_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("TileSizeY_", "catacombs_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("mS", "catacombs_platform_moving_001", 3f ){ },

        new SpecialDefaultValue("TileSizeX_", "forge_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("TileSizeY_", "forge_platform_moving_001", 3 ){ },
        new SpecialDefaultValue("mS", "forge_platform_moving_001", 3f ){ },

    };

    public class SpecialDefaultValue
	{
		public SpecialDefaultValue(string attribute, string ObjectName, object newDefaultValue)
		{
			validAttribute = attribute;
			objectName = ObjectName;
			SpecialDefault = newDefaultValue;
        }

        public string validAttribute;
		public string objectName;
		public object SpecialDefault;

		public bool MatchingValue(string attribute, string ObjectName)
		{
			return validAttribute == attribute && objectName == ObjectName;
        }
    }
}
