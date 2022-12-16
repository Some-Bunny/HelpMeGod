using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TileDatabase
{
	
	
	 
	public virtual Dictionary<string, string> Entries { get; set; }

	
	
	 
	public virtual Dictionary<string, Dictionary<string, string>> SubEntries { get; set; }

	
	
	public Dictionary<string, string> AllEntries
	{
		get
		{
			Dictionary<string, string> ret = new Dictionary<string, string>(this.Entries);
			foreach (Dictionary<string, string> sub in this.SubEntries.Values)
			{
				List<KeyValuePair<string, string>> list = sub.ToList<KeyValuePair<string, string>>();
				foreach(var x in list)
                {
					ret.Add(x.Key, x.Value);
				}
				

			}
			return ret;
		}
	}

	 
	public string GetGUID(string enemyID)
	{
		return this.AllEntries[enemyID];
	}

	 
	public string GetID(string guid)
	{
		foreach (string key in this.AllEntries.Keys)
		{
			bool flag = this.AllEntries[key].Equals(guid);
			if (flag)
			{
				return key;
			}
		}
		Debug.LogError("EnemyDatabase: Could not find enemy with GUID " + guid);
		return null;
	}

	
	public string spriteDirectory;
}
