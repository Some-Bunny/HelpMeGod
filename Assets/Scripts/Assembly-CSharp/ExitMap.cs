using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class ExitMap : TilemapHandler
{

	public void CollectDataForExport(ref ImportExport.NewRoomData data)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> directions = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				bool flag = !tiles[x, y];
				if (!flag)
				{
					Tile tile = tiles[x, y];
					string name = tile.name.ToLower();
					Vector2 position = new Vector2((float)(x + 1), (float)(y + 1));
					foreach (string dir in this.Directions)
					{
						bool flag2 = name.Contains(dir);
						if (flag2)
						{
							directions.Add(dir.ToUpper());
							positions.Add(position);
						}
					}
				}
			}
		}
		data.exitDirections = data.exitDirections.Concat(directions.ToArray()).ToArray<string>();
		data.exitPositions = data.exitPositions.Concat(positions.ToArray()).ToArray<Vector2>();
	}

	public void CollectDataForExport(ref ImportExport.RoomData data)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> directions = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				bool flag = !tiles[x, y];
				if (!flag)
				{
					Tile tile = tiles[x, y];
					string name = tile.name.ToLower();
					Vector2 position = new Vector2((float)(x + 1), (float)(y + 1));
					foreach (string dir in this.Directions)
					{
						bool flag2 = name.Contains(dir);
						if (flag2)
						{
							directions.Add(dir.ToUpper());
							positions.Add(position);
						}
					}
				}
			}
		}
		data.exitDirections = data.exitDirections.Concat(directions.ToArray()).ToArray<string>();
		data.exitPositions = data.exitPositions.Concat(positions.ToArray()).ToArray<Vector2>();
	}

	 
	public Tile GetExit(string direction)
	{
		foreach (Tile tile in this.tiles)
		{
			bool flag = tile.name.ToLower().Contains(direction.ToLower());
			if (flag)
			{
				return tile;
			}
		}
		return null;
	}

	 
	public override TileDatabase InitializeDatabase()
	{
		this.tileDatabase = new TileDatabase();
		this.tileDatabase.Entries = new Dictionary<string, string>
		{
			{
				"Doors",
				Manager.paletteDividerGuid
			},
			{
				"door_west",
				null
			},
			{
				"door_north",
				null
			},
			{
				"door_south",
				null
			},
			{
				"door_east",
				null
			}
		};
		this.tileDatabase.SubEntries = new Dictionary<string, Dictionary<string, string>>();
		this.tileDatabase.spriteDirectory = "sprites/doors";
		return this.tileDatabase;
	}

	
	public string[] Directions = new string[]
	{
		"north",
		"south",
		"east",
		"west"
	};
}
