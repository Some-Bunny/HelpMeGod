using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyMap : TilemapHandler
{
	 
	protected override void Awake()
	{
		base.Awake();
	}

	public void CollectDataForExport(ref ImportExport.NewRoomData data, int index, Enums.RoomEventTriggerCondition trigger)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> guids = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		List<string> attributes = new List<string>();
		List<string> triggers = new List<string>();
		List<int> layers = new List<int>();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (!tiles[x, y]) continue;
				Tile tile = tiles[x, y];
				guids.Add(this.tileDatabase.AllEntries[tile.name]);
				positions.Add(new Vector2((float)x, (float)y));
				DataTile dataTile;
				triggers.Add(trigger.ToString());
				attributes.Add(JsonConvert.SerializeObject(AttributeDatabase.ToShortNamed(((dataTile = (tile as DataTile)) != null) ? dataTile.data : new JObject()), Formatting.None));
				layers.Add(index);
			}
		}
		data.enemyGUIDs = data.enemyGUIDs.Concat(guids.ToArray()).ToArray<string>();
		data.enemyPositions = data.enemyPositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.enemyReinforcementLayers = data.enemyReinforcementLayers.Concat(layers.ToArray()).ToArray<int>();
		data.enemyAttributes = data.enemyAttributes.Concat(attributes.ToArray()).ToArray<string>();
		data.waveTriggers = data.waveTriggers.Concat(triggers.ToArray()).ToArray();
	}

	public void CollectDataForExport(ref ImportExport.RoomData data, int index, Enums.RoomEventTriggerCondition trigger)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> guids = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		List<string> attributes = new List<string>();
		List<string> triggers = new List<string>();
		List<int> layers = new List<int>();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (!tiles[x, y]) continue;
				Tile tile = tiles[x, y];
				guids.Add(this.tileDatabase.AllEntries[tile.name]);
				positions.Add(new Vector2((float)x, (float)y));
				DataTile dataTile;
				triggers.Add(trigger.ToString());
				attributes.Add(JsonConvert.SerializeObject(AttributeDatabase.ToShortNamed(((dataTile = (tile as DataTile)) != null) ? dataTile.data : new JObject()), Formatting.None));
				layers.Add(index);
			}
		}
		data.enemyGUIDs = data.enemyGUIDs.Concat(guids.ToArray()).ToArray<string>();
		data.enemyPositions = data.enemyPositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.enemyReinforcementLayers = data.enemyReinforcementLayers.Concat(layers.ToArray()).ToArray<int>();
		data.enemyAttributes = data.enemyAttributes.Concat(attributes.ToArray()).ToArray<string>();
		data.waveTriggers = data.waveTriggers.Concat(triggers.ToArray()).ToArray();
	}

	 
	public override TileDatabase InitializeDatabase()
	{
		this.tileDatabase = new EnemyDatabase();
		this.tileDatabase.spriteDirectory = "sprites/enemies/";

		foreach(var enemy in CustomObjectDatabase.Instance.customEnemies)
        {
			this.tileDatabase.Entries.Add($"customEnemyAsset-{enemy.name}", enemy.guid);
			Debug.Log(enemy.name);
		}

		return this.tileDatabase;
	}

	public void ReloadTiles()
    {
		InitializeTiles();
	}
	 
	protected override void InitializeTiles()
	{
		base.InitializeTiles();
		foreach (Tile tile in this.tiles)
		{
			DataTile dataTile = tile as DataTile;
			List<AC> aCs;
			AttributeDatabase.TryGetListing("every_single_enemy_ever", out aCs);
			foreach (AC ac in aCs)
			{
				if (!dataTile.data.ContainsKey(ac.longName)) dataTile.data.Add(ac.longName, JToken.FromObject(ac.defaultValue));
			}
		}
	}
}
