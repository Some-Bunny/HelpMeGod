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
			new AC("Start At Node:", "nSP_O", "0", new object[] { "0" })
		},
		{
			"mS",
			new AC("maxSpeed", "mS", 9, new object[0])
		},
		{
			"tTMS",
			new AC("timeToMaxSpeed", "tTMS", 1.5f, new object[0])
		},
		{
			"storedBodyMC",
			new AC("Stored Body", "storedBodyMC", "None", new object[] {"None", "ExplosiveBarrel", "Turret" })
		},
		{
			"storedenemyBodyMC",
			new AC("Enemy GUID In Cart", "storedenemyBodyMC", "None", new object[0])
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
			new AC("Trap Trigger", "TrapTriggerMethod", "Timer", new object[] {"Timer", "PlaceableFootprint", "SpecRigidbody", "Script", })
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
    };
    public static Dictionary<AttributeDatabase.ValidAttribute, string[]> attributeListings = new Dictionary<AttributeDatabase.ValidAttribute, string[]>
	{
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
                "nSP_O"
            }
		},
        {
            (string name) => name == "sawblade",
            new string[]
            {
                "tSP",
                "nSP_O"
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
                "storedBodyMC",
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
            }
        },
        {
            (string name) => name == "flame_trap",
            new string[]
            {
                "TrapTriggerMethod",
            }
        },
        {
            (string name) => name == "pitfall_trap",
            new string[]
            {
                "TrapTriggerMethod",
            }
        },
    };
    public delegate bool ValidAttribute(string name);
	public static List<SpecialDefaultValue> specialDefaultValueListings = new List<SpecialDefaultValue>
	{
		new SpecialDefaultValue("TrapTriggerMethod", "flame_trap", "Timer" ){ },
        new SpecialDefaultValue("TrapTriggerMethod", "pitfall_trap", "PlaceableFootprint" ){ },
        new SpecialDefaultValue("TrapTriggerMethod", "floor_spikes", "PlaceableFootprint" ){ },
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
