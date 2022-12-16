using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DataTile : Tile
{
	
	public JObject data = new JObject();

	public int placmentOrder = -1;
	public Vector2 position;
}
