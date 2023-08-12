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
                        if (name.Contains(dir) && name.Contains("entryonly"))
                        {
                            directions.Add(name.ToUpper());
                            positions.Add(position);
                        }
                        else if (name.Contains(dir) && name.Contains("exitonly"))
                        {
                            directions.Add(name.ToUpper());
                            positions.Add(position);
                        }
                        else if (name.Contains(dir) && !name.Contains("exitonly") && !name.Contains("entryonly"))
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
                        Debug.LogError(name.ToUpper());
                        if (name.Contains(dir) && name.Contains("entryonly"))
						{
							directions.Add(name.ToUpper());
							positions.Add(position);
						}
                        else if (name.Contains(dir) && name.Contains("exitonly"))
                        {
                            directions.Add(name.ToUpper());
                            positions.Add(position);
                        }
						else if (name.Contains(dir) && !name.Contains("exitonly") && !name.Contains("entryonly"))
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
			bool flag = tile.name.ToLower() == direction.ToLower();
			if (flag)
			{
				return tile;
			}
		}
		if (direction.ToLower() == "west")
		{
            return this.tiles.Where(self => self.name == "door_west").First();
        }
        if (direction.ToLower() == "north")
        {
            return this.tiles.Where(self => self.name == "door_north").First();
        }
        if (direction.ToLower() == "south")
        {
            return this.tiles.Where(self => self.name == "door_south").First();
        }
        if (direction.ToLower() == "east")
        {
            return this.tiles.Where(self => self.name == "door_east").First();
        }

        return this.tiles.Where(self => self.name == "door_west").First();
	}

	 
	public override TileDatabase InitializeDatabase()
	{
		this.tileDatabase = new TileDatabase();
		this.tileDatabase.Entries = new Dictionary<string, string>
		{
			{
				"Doors [Exit And Entrance]",
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
			},
            {
                "Doors [Exit Only]",
                Manager.paletteDividerGuid
            },
            {
                "door_westExitOnly",
                null
            },
            {
                "doornorthExitOnly",
                null
            },
            {
                "door_southExitOnly",
                null
            },
            {
                "door_eastExitOnly",
                null
            },
            {
                "Doors [Entry Only]",
                Manager.paletteDividerGuid
            },
            {
                "door_westEntryOnly",
                null
            },
            {
                "doornorthEntryOnly",
                null
            },
            {
                "door_southEntryOnly",
                null
            },
            {
                "door_eastEntryOnly",
                null
            },
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
