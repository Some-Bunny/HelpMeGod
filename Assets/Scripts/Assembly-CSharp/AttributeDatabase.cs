using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;


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
			"mS",
			new AC("maxSpeed", "mS", 9, new object[0])
		},
		{
			"tTMS",
			new AC("timeToMaxSpeed", "tTMS", 1.5f, new object[0])
		},
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
			(string name) => name == "sawblade",
			new string[]
			{
				"tSP"
			}
		},

		{
			(string name) => name == "minecart",
			new string[]
			{
				"tSP",
				"mS",
				"tTMS"
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
		}
	};

	
	
	public delegate bool ValidAttribute(string name);
}
