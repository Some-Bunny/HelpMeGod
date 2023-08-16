using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnvironmentMap : TilemapHandler
{
	 
	public void DrawBorder()
	{
        Vector3Int pos = Vector3Int.zero;
        pos.z = this.map.origin.z;
        int xmin = TilemapHandler.Bounds.xMin;
        int ymin = TilemapHandler.Bounds.yMin;
        int xmax = TilemapHandler.Bounds.xMax;
        int ymax = TilemapHandler.Bounds.yMax;
        for (int x = xmin; x < xmax; x++)
        {
            pos.x = x;

            pos.y = ymax - 1;
            this.map.SetTile(pos, this.palette["wall"]);
            pos.y = ymin;
            this.map.SetTile(pos, this.palette["wall"]);
        }
        for (int y = ymin; y < ymax; y++)
        {
            pos.y = y;

            pos.x = xmax - 1;
            this.map.SetTile(pos, this.palette["wall"]);
            pos.x = xmin;
            this.map.SetTile(pos, this.palette["wall"]);
        }
    }

    public void CreatRodentRoom()
	{
		Vector2Int pos = Vector2Int.zero;
		int xmin = TilemapHandler.Bounds.xMin;
		int ymin = TilemapHandler.Bounds.yMin;
		int xmax = TilemapHandler.Bounds.xMax;
		int ymax = TilemapHandler.Bounds.yMax;



		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin, ymin, 0), new Vector3Int(6, 2, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));
		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin, ymin, 0), new Vector3Int(2, 6, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));

		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax + 5, ymin, 0), new Vector3Int(-6, 2, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));
		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax + 1, ymin, 0), new Vector3Int(-2, 6, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));

		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin, ymax - 2, 0), new Vector3Int(6, 2, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));
		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin, ymax - 6, 0), new Vector3Int(2, 6, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));

		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax + 5, ymax - 2, 0), new Vector3Int(-6, 2, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));
		//this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax + 1, ymax - 6, 0), new Vector3Int(-2, 6, 1)), CreateTileArrayOfLength(12, this.palette["pit"]));


		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin + 6, ymax - 1, 0), new Vector3Int(24, 1, 1)), CreateTileArrayOfLength(24, this.palette["wall"]));
		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin + 6, ymin, 0), new Vector3Int(24, 1, 1)), CreateTileArrayOfLength(24, this.palette["wall"]));

		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin, ymin + 6, 0), new Vector3Int(1, 15, 1)), CreateTileArrayOfLength(15, this.palette["wall"]));
		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax - 1, ymin + 6, 0), new Vector3Int(1, 15, 1)), CreateTileArrayOfLength(15, this.palette["wall"]));


		this.map.SetTile(new Vector3Int(xmin + 1, ymin + 6, 0), this.palette["wall"]);
		this.map.SetTile(new Vector3Int(xmin + 1, ymax - 7, 0), this.palette["wall"]);

		this.map.SetTile(new Vector3Int(xmax - 2, ymin + 6, 0), this.palette["wall"]);
		this.map.SetTile(new Vector3Int(xmax - 2, ymax - 7, 0), this.palette["wall"]);

		this.map.SetTile(new Vector3Int(xmin + 6, ymin + 1, 0), this.palette["wall"]);
		this.map.SetTile(new Vector3Int(xmin + 6, ymax - 2, 0), this.palette["wall"]);

		this.map.SetTile(new Vector3Int(xmax - 7, ymin + 1, 0), this.palette["wall"]);
		this.map.SetTile(new Vector3Int(xmax - 7, ymax - 2, 0), this.palette["wall"]);

		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax - 7, ymin + 2, 0), new Vector3Int(5, 1, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));
		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax - 7, ymax - 3, 0), new Vector3Int(5, 1, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));

		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin + 2, ymin + 2, 0), new Vector3Int(5, 1, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));
		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin + 2, ymax - 3, 0), new Vector3Int(5, 1, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));

		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin + 2, ymin + 2, 0), new Vector3Int(1, 5, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));
		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmin + 2, ymax - 7, 0), new Vector3Int(1, 5, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));

		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax - 3, ymin + 2, 0), new Vector3Int(1, 5, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));
		this.map.SetTilesBlock(new BoundsInt(new Vector3Int(xmax - 3, ymax - 7, 0), new Vector3Int(1, 5, 1)), CreateTileArrayOfLength(5, this.palette["wall"]));

		this.map.FloodFill(new Vector3Int(xmax - 5, ymax - 7, 0), this.palette["floor"]);

		var exitMap = Manager.Instance.GetTilemap(TilemapHandler.MapType.Exits);
		exitMap.map.SetTile(new Vector3Int(xmin + 17, ymin, 0), exitMap.palette["door_south"]);
		exitMap.map.SetTile(new Vector3Int(xmin + 17, ymax - 1, 0), exitMap.palette["door_north"]);


		exitMap.map.SetTile(new Vector3Int(xmin, ymax - 15, 0), exitMap.palette["door_west"]);
		exitMap.map.SetTile(new Vector3Int(xmax - 1, ymax - 15, 0), exitMap.palette["door_east"]);
	}

	public TileBase[] CreateTileArrayOfLength(int length, TileBase tile)
    {
		TileBase[] tileArray = new TileBase[length];
		for (int index = 0; index < tileArray.Length; index++)
		{
			tileArray[index] = tile;
		}
		return tileArray;
	}

	public Texture2D CollectDataForExportLegecy()
	{
		Texture2D texture = new Texture2D(TilemapHandler.Bounds.size.x, TilemapHandler.Bounds.size.y);
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				Tile tile = tiles[x, y];
				if (!tile)
				{
					texture.SetPixel(x, y, Color.magenta);
				}
				else
				{
					string name = tile.name.ToLower();
					if (name.Contains("floor"))
					{
						texture.SetPixel(x, y, Color.white);
					}
					else if (name.Contains("wall"))
					{
						texture.SetPixel(x, y, Color.grey);
					}
					else if (name.Contains("pit"))
					{
						texture.SetPixel(x, y, Color.black);
					}
				}
			}
		}
		return texture;
	}

	public void CollectDataForExport(ref ImportExport.NewRoomData data)
	{
		string tileData = "";

		var pos = new List<Vector2>();
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		for (int y = 0; y < tiles.GetLength(1); y++)
		{
			for (int x = 0; x < tiles.GetLength(0); x++)
			{
				Tile tile = tiles[x, y];
				if (tile == null)
				{
					tileData += 2;
				}
				else
				{
					string name = tile.name.ToLower();
					if (name.Contains("floor"))
					{
						tileData += 1;
					}
					else if (name.Contains("wall"))
					{
						tileData += 2;
					}
					else if (name.Contains("pit"))
					{
						tileData += 3;
					}	
					else if (name.Contains("ice"))
					{
						tileData += 4;
					}
                    else if (name.Contains("effecthazard"))
                    {
                        tileData += 5;
                    }
                    else if (name.Contains("diagonal_ne"))
					{
                        tileData += 6;
                    }
                    else if (name.Contains("diagonal_nw"))
                    {
                        tileData += 7;
                    }
                    else if (name.Contains("diagonal_se"))
                    {
                        tileData += 8;
                    }
                    else if (name.Contains("diagonal_sw"))
                    {
                        tileData += 9;
                    }
                    else
                    {
						tileData += 2;
					}
				}
			}
		}

		//data.tilePositions = pos.ToArray();
		data.tileInfo = tileData;		
	}


	public override TileDatabase InitializeDatabase()
	{
		this.tileDatabase = new TileDatabase();
		this.tileDatabase.Entries = new Dictionary<string, string>
		{
			{
				"Environment",
				Manager.paletteDividerGuid
			},
			{
				"floor",
				null
			},
			{
				"wall",
				null
			},
			{
				"pit",
				null
			},
            {
                "Ice [Hollow Only]",
                Manager.paletteDividerGuid
            },
            {
                "ice",
                null
            },
            {
                "Effect Hazard [Mines / Forge]",
                Manager.paletteDividerGuid
            },
            {
                "effecthazard",
                null
            },
            {
                "Diag0nal Tiles [Keep / Proper / Hollow]",
                Manager.paletteDividerGuid
            },
            {
                "diagonal_NE",
                null
            },
            {
                "diagonal_NW",
                null
            },
            {
                "diagonal_SE",
                null
            },
            {
                "diagonal_SW",
                null
            },
        };
		this.tileDatabase.SubEntries = new Dictionary<string, Dictionary<string, string>>();
		this.tileDatabase.spriteDirectory = "sprites/environment";
		return this.tileDatabase;
	}

	 
	protected override void InitializeTiles()
	{
		base.InitializeTiles();
		PaletteDropdown.Instance.ActivePalette.Populate();
	}
}
