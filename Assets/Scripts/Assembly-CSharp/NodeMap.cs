using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NodeMap : TilemapHandler
{

	public void CollectDataForExport(ref ImportExport.RoomData data, int index, Enums.SerializedPathWrapMode trigger)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> guids = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		//List<string> attributes = new List<string>();
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
				triggers.Add(trigger.ToString());
				layers.Add(index);
			}
		}
		data.nodeTypes = data.nodeTypes.Concat(guids.ToArray()).ToArray<string>();
		data.nodePositions = data.nodePositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
		data.nodeWrapModes = data.nodeWrapModes.Concat(triggers.ToArray()).ToArray();
	}

	public void CollectDataForExport2(ref ImportExport.NewRoomData data, int index, Enums.SerializedPathWrapMode trigger)
	{

		Tile[,] tiles = base.AllTiles();
		string[] guids = new string[nodes.Length];
		Vector2[] positions = new Vector2[nodes.Length];
		List<string> triggers = new List<string>();
		List<int> layers = new List<int>();
		//foreach (var node in nodes)
		//{
		//	guids.Add(this.tileDatabase.AllEntries[node.name]);
		//	positions.Add(node.position);
		//	triggers.Add(trigger.ToString());
		//	layers.Add(index);
		//}
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (!tiles[x, y]) continue;
				DataTile tile = tiles[x, y] as DataTile;

				if (!string.IsNullOrEmpty(this.tileDatabase.AllEntries[tile.name]))
                {
					guids[tile.placmentOrder] = (this.tileDatabase.AllEntries[tile.name]);
					positions[tile.placmentOrder] = (new Vector2((float)x, (float)y));
					triggers.Add(trigger.ToString());
					layers.Add(index);
				}

			}
		}

		data.nodeTypes = data.nodeTypes.Concat(guids.ToArray()).ToArray<string>();
		data.nodePositions = data.nodePositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
		data.nodeWrapModes = data.nodeWrapModes.Concat(triggers.ToArray()).ToArray();
	}

	public void CollectDataForExport2(ref ImportExport.RoomData data, int index, Enums.SerializedPathWrapMode trigger)
	{

		Tile[,] tiles = base.AllTiles();
		string[] guids = new string[nodes.Length];
		Vector2[] positions = new Vector2[nodes.Length];
		List<string> triggers = new List<string>();
		List<int> layers = new List<int>();
		//foreach (var node in nodes)
		//{
		//	guids.Add(this.tileDatabase.AllEntries[node.name]);
		//	positions.Add(node.position);
		//	triggers.Add(trigger.ToString());
		//	layers.Add(index);
		//}
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (!tiles[x, y]) continue;
				DataTile tile = tiles[x, y] as DataTile;

				
				guids[tile.placmentOrder] = (this.tileDatabase.AllEntries[tile.name]);
				positions[tile.placmentOrder] = (new Vector2((float)x, (float)y));
				triggers.Add(trigger.ToString());
				layers.Add(index);
			}
		}

		data.nodeTypes = data.nodeTypes.Concat(guids.ToArray()).ToArray<string>();
		data.nodePositions = data.nodePositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
		data.nodeWrapModes = data.nodeWrapModes.Concat(triggers.ToArray()).ToArray();
	}



	public void AddNewNodeTile(DataTile tile, Vector3Int position)
    {
		//nodes.Clear();

		//Debug.Log(tile.name);



		if (tile == null) return;

		var allTiles = base.AllTiles();
		var tileList = new List<Tile>();
		foreach (var t in allTiles)
        {
			if (t != null) tileList.Add(t);
		}

		nodes = new DataTile[tileList.Count > 0 ? tileList.Count : 1];
		
		foreach (var nTile in allTiles)
        {
			var dataTile = nTile as DataTile;
			if (dataTile == null) continue;

			if (dataTile.placmentOrder >= 0) nodes[dataTile.placmentOrder] = dataTile;
		}

		

		if (tile.placmentOrder < 0) tile.placmentOrder = nodes.Length - 1; nodes[tile.placmentOrder] = tile;
	}

	public DataTile[] nodes = new DataTile[0];

	public override TileDatabase InitializeDatabase()
	{
		this.tileDatabase = new TileDatabase();
		this.tileDatabase.Entries = new Dictionary<string, string>
		{
			{
				"Nodes",
				Manager.paletteDividerGuid
			},
			{
				"Center",
				"Center"
			},
			{
				"North",
				"North"
			},
			{
				"NorthEast",
				"NorthEast"
			},
			{
				"East",
				"East"
			},
			{
				"SouthEast",
				"SouthEast"
			},
			{
				"South",
				"South"
			},
			{
				"SouthWest",
				"SouthWest"
			},
			{
				"West",
				"West"
			},
			{
				"NorthWest",
				"NorthWest"
			},
		};
		this.tileDatabase.SubEntries = new Dictionary<string, Dictionary<string, string>>();
		this.tileDatabase.spriteDirectory = "sprites/node";
		return this.tileDatabase;
	}
}
