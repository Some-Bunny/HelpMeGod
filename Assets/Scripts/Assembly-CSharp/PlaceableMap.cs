using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class PlaceableMap : TilemapHandler
{
	
	protected override void Awake()
	{
		base.Awake();
	}


	public void CollectDataForExport(ref ImportExport.NewRoomData data)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> guids = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		List<string> attributes = new List<string>();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				bool flag = !tiles[x, y];
				if (!flag)
				{
					Tile tile = tiles[x, y];
					guids.Add(this.tileDatabase.AllEntries[tile.name]);
					positions.Add(new Vector2((float)x, (float)y));
					DataTile dataTile;
					attributes.Add(JsonConvert.SerializeObject(AttributeDatabase.ToShortNamed(((dataTile = (tile as DataTile)) != null) ? dataTile.data : new JObject()), Formatting.None));
				}
			}
		}
		data.placeableGUIDs = guids.ToArray();
		data.placeablePositions = positions.ToArray();
		data.placeableAttributes = attributes.ToArray();
	}

	public void CollectDataForExport(ref ImportExport.RoomData data)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> guids = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		List<string> attributes = new List<string>();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				bool flag = !tiles[x, y];
				if (!flag)
				{
					Tile tile = tiles[x, y];
					guids.Add(this.tileDatabase.AllEntries[tile.name]);
					positions.Add(new Vector2((float)x, (float)y));
					DataTile dataTile;
					attributes.Add(JsonConvert.SerializeObject(AttributeDatabase.ToShortNamed(((dataTile = (tile as DataTile)) != null) ? dataTile.data : new JObject()), Formatting.None));
				}
			}
		}
		data.placeableGUIDs = guids.ToArray();
		data.placeablePositions = positions.ToArray();
		data.placeableAttributes = attributes.ToArray();
	}

	

	public override TileDatabase InitializeDatabase()
	{
		this.tileDatabase = new PlaceableDatabase();
		this.tileDatabase.spriteDirectory = "sprites/placeables/";

		CustomObjectDatabase.Instance.Start();

		//Debug.Log(CustomObjectDatabase.Instance.customPlaceables.Count);
		foreach (var enemy in CustomObjectDatabase.Instance.customPlaceables)
		{
			this.tileDatabase.Entries.Add($"customPlaceableAsset-{enemy.name}", enemy.guid);
		}

		return this.tileDatabase;
	}
}
