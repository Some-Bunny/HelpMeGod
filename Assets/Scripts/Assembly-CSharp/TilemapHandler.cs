using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public abstract class TilemapHandler : MonoBehaviour
{
	
	
	 
	public static BoundsInt Bounds { get; set; }

	
	public void HandleMouseDown(Vector3Int gridPos, BrushButton.BrushType type)
	{
		if (!TilemapHandler.InBounds(gridPos)) return;
		Tile tile = InputHandler.Instance.selectedTileType;
		if (!tile && type != BrushButton.BrushType.ERASER) return;




		if (type == BrushButton.BrushType.PENCIL || type == BrushButton.BrushType.ERASER || type == BrushButton.BrushType.BRUSH)
		{
			this.SetTileWithThickness(this.map, gridPos, (type == BrushButton.BrushType.ERASER) ? null : tile, InputHandler.Instance.brushSize);
		}
		else
		{
			if (type == BrushButton.BrushType.BUCKET)
			{
				if (InputHandler.Instance.activeTilemap == TilemapHandler.MapType.Environment)
				{
					this.map.FloodFill(new Vector3Int(gridPos.x, gridPos.y, 0), tile);
					this.SetTileWithThickness(this.map, gridPos, tile, 1);
				}
				else
				{
					InputHandler.Instance.BrushType = BrushButton.BrushType.BRUSH;
					this.SetTileWithThickness(this.map, gridPos, tile, InputHandler.Instance.brushSize);
				}
			}
		}
	}

	public void SetTileWithThickness(Tilemap map, Vector3Int position, Tile tile, int thickness)
	{
		DataTile newTile = TilemapHandler.Clone(tile);
       

        if (InputHandler.Instance.nodeMode && InputHandler.Instance.isDrawing) (NodePathLayerHandler.Instance.GetActiveTilemap() as NodeMap).AddNewNodeTile(newTile, position);

		map.SetTile(new Vector3Int(position.x, position.y, 0), newTile);

		if (thickness > 1 && !InputHandler.Instance.nodeMode)
		{
			Vector3Int[] tiles = this.GetGridCircle(position, thickness - 1);
			for (int i = 0; i < tiles.Length; i++)
			{
				Vector3Int t = tiles[i];
				if (TilemapHandler.InBounds(t))
				{
					map.SetTile(t, newTile);
				}           
                this.FillLine(this.grid.CellToWorld(t), this.grid.CellToWorld(tiles[(i + 1) % tiles.Length]), newTile, 1);
			}
		}



	}

	public void FillLine(Vector2 start, Vector2 end, Tile tile, int thickness)
	{
		this.FillLine(this.grid.WorldToCell(start), this.grid.WorldToCell(end), tile, thickness);
	}

	
	public void FillLine(Vector3Int start, Vector3Int end, Tile tile, int thickness)
	{
		foreach (Vector3Int point in MathTools.GetPointsOnLine(start, end))
		{
			bool flag = TilemapHandler.InBounds(point);
			if (flag)
			{
				this.SetTileWithThickness(this.map, point, tile, thickness);
			}
		}
	}

	
	public void OutlineRectangle(Vector2 start, Vector2 end, Tile tile, int thickness, bool alterOrigin = false)
	{
		this.OutlineRectangle(this.grid.WorldToCell(start), this.grid.WorldToCell(end), tile, thickness, alterOrigin);
	}

	
	public void OutlineRectangle(Vector3Int start, Vector3Int end, Tile tile, int thickness, bool alterOrigin = false)
	{
		if (alterOrigin)
		{
			Vector3Int magnitude = end - start;
			start.x -= magnitude.x;
			start.y -= magnitude.y;
		}
		this.FillLine(start, new Vector3Int(start.x, end.y, 0), tile, thickness);
		this.FillLine(new Vector3Int(start.x, end.y, 0), end, tile, thickness);
		this.FillLine(start, new Vector3Int(end.x, start.y, 0), tile, thickness);
		this.FillLine(new Vector3Int(end.x, start.y, 0), end, tile, thickness);
	}

	
	public void FillRectangle(Vector2 start, Vector2 end, Tile tile, int thickness, bool alterOrigin = false)
	{
		this.FillRectangle(this.grid.WorldToCell(start), this.grid.WorldToCell(end), tile, thickness, alterOrigin);
	}

	
	public void FillRectangle(Vector3Int start, Vector3Int end, Tile tile, int thickness, bool alterOrigin = false)
	{
		if (alterOrigin)
		{
			Vector3Int magnitude = end - start;
			start.x -= magnitude.x;
			start.y -= magnitude.y;
		}
		this.OutlineRectangle(start, end, tile, thickness, false);
		int sign = Math.Sign(end.y - start.y);
		for (int i = 0; i < Math.Abs(end.y - start.y); i++)
		{
			this.FillLine(new Vector3Int(start.x, start.y + i * sign, 0), new Vector3Int(end.x, start.y + i * sign, 0), tile, 1);
		}
	}

	
	public void OutlineEllipse(Vector2 start, Vector2 end, Tile tile, int thickness, bool alterOrigin = false)
	{
		this.OutlineEllipse(this.grid.WorldToCell(start), this.grid.WorldToCell(end), tile, thickness, alterOrigin);
	}

	
	public void OutlineEllipse(Vector3Int start, Vector3Int end, Tile tile, int thickness, bool alterOrigin = false)
	{
		Vector3Int midpoint = new Vector3Int((start.x + end.x) / 2, (start.y + end.y) / 2, (start.z + end.z) / 2);
		Vector3Int en = new Vector3Int(midpoint.x, midpoint.y, midpoint.z);
		if (alterOrigin)
		{
			midpoint = start;
			en = end;
		}
		foreach (Vector3Int t in this.GetGridEllipse(midpoint, (double)Math.Abs((en - start).x), (double)Math.Abs((en - start).y)))
		{
			if (TilemapHandler.InBounds(t))
			{
				this.SetTileWithThickness(this.map, t, tile, thickness);
			}
		}
	}

	
	public void FillEllipse(Vector2 start, Vector2 end, Tile tile, int thickness, bool alterOrigin = false)
	{
		this.FillEllipse(this.grid.WorldToCell(start), this.grid.WorldToCell(end), tile, thickness, alterOrigin);
	}

	
	public void FillEllipse(Vector3Int start, Vector3Int end, Tile tile, int thickness, bool alterOrigin = false)
	{
		Vector3Int midpoint = new Vector3Int((start.x + end.x) / 2, (start.y + end.y) / 2, (start.z + end.z) / 2);
		Vector3Int en = new Vector3Int(midpoint.x, midpoint.y, midpoint.z);
		if (alterOrigin)
		{
			midpoint = start;
			en = end;
		}
		Vector3Int[] tiles = this.GetGridEllipse(midpoint, (double)Math.Abs((en - start).x), (double)Math.Abs((en - start).y));
		for (int i = 0; i < tiles.Length; i++)
		{
			Vector3Int t = tiles[i];
			bool flag = TilemapHandler.InBounds(t);
			if (flag)
			{
				this.SetTileWithThickness(this.map, t, tile, thickness);
			}
			this.FillLine(this.grid.CellToWorld(t), this.grid.CellToWorld(tiles[(i + 1) % tiles.Length]), tile, 1);
		}
	}

	
	public Tile[,] AllTiles()
	{
		int xmin = TilemapHandler.Bounds.xMin;
		int ymin = TilemapHandler.Bounds.yMin;
		int width = TilemapHandler.Bounds.size.x;
		int height = TilemapHandler.Bounds.size.y;
		Tile[,] tiles = new Tile[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				tiles[x, y] = (Tile)this.map.GetTile(new Vector3Int(xmin + x, ymin + y, 0));
			}
		}
		return tiles;
	}

	
	public Vector3Int[] GetGridNeighbors(Vector3Int gridPos)
	{
		return new Vector3Int[]
		{
			gridPos + Vector3Int.right,
			gridPos + Vector3Int.up,
			gridPos + Vector3Int.left,
			gridPos + Vector3Int.down
		};
	}

	public Vector3Int[] GetGridCircle(Vector3Int gridPos, int radius)
	{
		int centerX = gridPos.x;
		int centerY = gridPos.y;
		int d = (5 - radius * 4) / 4;
		int x = 0;
		int y = radius;
		List<Vector3Int> ret = new List<Vector3Int>();
		while (x <= y)
		{
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX + x, centerY + y, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX + x, centerY - y, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX - x, centerY + y, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX - x, centerY - y, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX + y, centerY + x, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX + y, centerY - x, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX - y, centerY + x, 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int(centerX - y, centerY - x, 0));
			if (d < 0)
			{
				d += 2 * x + 1;
			}
			else
			{
				d += 2 * (x - y) + 1;
				y--;
			}
			x++;
		}		
		return ret.ToArray();
	}

	
	public Vector3Int[] GetGridEllipse(Vector3Int gridPos, double rx, double ry)
	{
		double xc = (double)gridPos.x;
		double yc = (double)gridPos.y;
		double x = 0.0;
		double y = ry;
		double d = ry * ry - rx * rx * ry + 0.25 * rx * rx;
		double dx = 2.0 * ry * ry * x;
		double dy = 2.0 * rx * rx * y;
		List<Vector3Int> ret = new List<Vector3Int>();
		while (dx < dy)
		{
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(x + xc), (int)(y + yc), 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(-x + xc), (int)(y + yc), 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(x + xc), (int)(-y + yc), 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(-x + xc), (int)(-y + yc), 0));
			bool flag = d < 0.0;
			if (flag)
			{
				x += 1.0;
				dx += 2.0 * ry * ry;
				d = d + dx + ry * ry;
			}
			else
			{
				x += 1.0;
				y -= 1.0;
				dx += 2.0 * ry * ry;
				dy -= 2.0 * rx * rx;
				d = d + dx - dy + ry * ry;
			}
		}
		double d2 = ry * ry * ((x + 0.5) * (x + 0.5)) + rx * rx * ((y - 1.0) * (y - 1.0)) - rx * rx * ry * ry;
		while (y >= 0.0)
		{
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(x + xc), (int)(y + yc), 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(-x + xc), (int)(y + yc), 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(x + xc), (int)(-y + yc), 0));
			this.AddVector3IntIfNotPresent(ret, new Vector3Int((int)(-x + xc), (int)(-y + yc), 0));
			bool flag2 = d2 > 0.0;
			if (flag2)
			{
				y -= 1.0;
				dy -= 2.0 * rx * rx;
				d2 = d2 + rx * rx - dy;
			}
			else
			{
				y -= 1.0;
				x += 1.0;
				dx += 2.0 * ry * ry;
				dy -= 2.0 * rx * rx;
				d2 = d2 + dx - dy + rx * rx;
			}
		}
		return ret.ToArray();
	}

	
	private void AddVector3IntIfNotPresent(List<Vector3Int> positions, Vector3Int toAdd)
	{
		if (!positions.Contains(toAdd))
		{
			positions.Add(toAdd);
		}
	}

	
	public static bool InBounds(Vector3Int cellPosition)
	{
		int xmin = TilemapHandler.Bounds.xMin;
		int ymin = TilemapHandler.Bounds.yMin;
		int xmax = TilemapHandler.Bounds.xMax;
		int ymax = TilemapHandler.Bounds.yMax;
		return cellPosition.x >= xmin && cellPosition.x < xmax && cellPosition.y >= ymin && cellPosition.y < ymax;
	}

	
	protected virtual void Awake()
	{
		this.map = base.GetComponent<Tilemap>();
		this.grid = UnityEngine.Object.FindObjectOfType<GridLayout>();
	}

	
	protected virtual void Start()
	{
		this.GeneratePalette();
		this.ResizeBounds();
		this.hasStarted = true;
	}

	
	public void ResizeBounds()
	{
		this.map.origin = new Vector3Int(TilemapHandler.Bounds.x, TilemapHandler.Bounds.y, 0);
		this.map.size = new Vector3Int(TilemapHandler.Bounds.size.x, TilemapHandler.Bounds.size.y, 0);
		this.map.ResizeBounds();
	}

    public void SpecialResizeBounds()
    {
        this.map.origin = new Vector3Int(TilemapHandler.Bounds.x, TilemapHandler.Bounds.y, 0);
        this.map.size = new Vector3Int(TilemapHandler.Bounds.size.x, TilemapHandler.Bounds.size.y, 0);
        this.map.ResizeBounds();
    }


    public void BuildFromTileArray(Tile[,] tiles, int overrideNodeOrder = -1)
	{
		for (int i = 0; i < tiles.GetLength(0); i++)
		{
			for (int j = 0; j < tiles.GetLength(1); j++)
			{

				if (this is NodeMap)
                {
					//var tile = TilemapHandler.Clone(tiles[i, j]);
					var tile = tiles[i, j];

					this.map.SetTile(TilemapHandler.GameToLocalPosition(i, j), tile);

					(this as NodeMap).AddNewNodeTile(tile as DataTile, TilemapHandler.GameToLocalPosition(i, j));
				}
				else
                {
					this.map.SetTile(TilemapHandler.GameToLocalPosition(i, j), tiles[i, j]);
				}
			}
		}
	}




    protected virtual void GeneratePalette()
	{
		if (!this.m_initializedTiles)
		{
			this.InitializeTiles();
		}
		this.palette = new Dictionary<string, Tile>();
		foreach (Tile tile in this.tiles)
		{
			this.palette.Add(this.tileDatabase.Entries.Keys.ToList<string>()[this.tiles.IndexOf(tile)], tile);
		}
		foreach (KeyValuePair<string, List<Tile>> t in this.subTiles)
		{
			foreach (Tile tile2 in t.Value)
			{
				this.palette.Add(this.tileDatabase.SubEntries[t.Key].Keys.ToList<string>()[t.Value.IndexOf(tile2)], tile2);
			}
		}
	}

	
	public abstract TileDatabase InitializeDatabase();

	
	protected virtual void InitializeTiles()
	{
		if (m_initializedTiles) return;
		this.tiles = new List<Tile>();
		this.subTiles = new Dictionary<string, List<Tile>>();
		Tile emptyTile = Manager.Instance.emptyTile;

		TileDatabase tdb = this.InitializeDatabase();
		if (tdb == null)
		{
			this.m_initializedTiles = true;
			return;
		}

		foreach (string entry in tdb.Entries.Keys)
		{
			this.tiles.Add(this.SetupTile(tdb, entry, tdb.Entries[entry], emptyTile));
		}
		foreach (KeyValuePair<string, Dictionary<string, string>> t in tdb.SubEntries)
		{
			this.subTiles.Add(t.Key, new List<Tile>());
			foreach (string entry2 in t.Value.Keys)
			{
				this.subTiles[t.Key].Add(this.SetupTile(tdb, entry2, t.Value[entry2], emptyTile));
			}
		}
		this.m_initializedTiles = true;
		
		
	}

	
	public Tile SetupTile(TileDatabase tdb, string name, string guid, Tile emptyTile)
	{
		Sprite sprite = null;
		if (name.StartsWith("customEnemyAsset-"))
		{
			sprite = IMG2Sprite.LoadNewSprite(Path.Combine(Application.persistentDataPath, "CustomEnemyData", name.Replace("customEnemyAsset-", "") + ".png"));			
		}
		else if (name.StartsWith("customPlaceableAsset-"))
		{
			sprite = IMG2Sprite.LoadNewSprite(Path.Combine(Application.persistentDataPath, "CustomPlaceableData", name.Replace("customPlaceableAsset-", "") + ".png"));
		}
		else
        {
			sprite = Resources.Load<Sprite>(Path.Combine(tdb.spriteDirectory, name));
		}
		
		GameObject tileText = null;
		DataTile tile = ScriptableObject.CreateInstance<DataTile>();
		tile.sprite = (sprite ? sprite.texture.CropWhiteSpace().ToSprite() : Manager.Instance.missingImageTileSprite);
		tile.name = ((guid != Manager.paletteDividerGuid) ? name : Manager.paletteDividerGuid);
		tile.gameObject = tileText;
		tile.colliderType = emptyTile.colliderType;
		tile.color = emptyTile.color;
		//tile.flags = TileFlags.None;
		tile.flags = emptyTile.flags;
		tile.hideFlags = emptyTile.hideFlags;
		tile.transform = emptyTile.transform;
		List<AC> aCs;
		if (AttributeDatabase.TryGetListing(name, out aCs))
		{
			foreach (AC ac in aCs)
			{
                var values = AttributeDatabase.TryGetSpecialDefaults(name, ac.shortName);
                tile.data.Add(ac.longName, JToken.FromObject(values ?? ac.defaultValue));
			}
		}
		spriteSwitch.Add(name, sprite);
        return tile;
	}

    public Dictionary<string, Sprite> spriteSwitch = new Dictionary<string, Sprite>();

    public static DataTile Clone(Tile other)
	{
		if (other == null)
		{
			return null;
		}
		else
		{
			DataTile tile = ScriptableObject.CreateInstance<DataTile>();
			tile.sprite = other.sprite;
			tile.name = other.name;
			tile.gameObject = other.gameObject;
			tile.colliderType = other.colliderType;
			tile.color = other.color;
			tile.flags = other.flags;
			tile.hideFlags = other.hideFlags;
			tile.transform = other.transform;
            DataTile dataTile;
			tile.data = (((dataTile = (other as DataTile)) != null) ? JObject.Parse(dataTile.data.ToString()) : new JObject());
			if (tile is DataTile data)
			{
                tile.PositionInTileMap = data.PositionInTileMap;
                tile.worldIntPosition = data.worldIntPosition;

                tile.position = data.position;

            }
            return tile;
		}
	}

	
	public static Vector3Int GameToLocalPosition(int x, int y)
	{
		return new Vector3Int(x - Manager.roomSize.x / 2, y - Manager.roomSize.y / 2, 0);
	}

	
	public static Vector3Int GameToLocalPosition(Vector2 vector)
	{
		return TilemapHandler.GameToLocalPosition((int)vector.x, (int)vector.y);
	}

	
	public TileDatabase tileDatabase;

	
	[HideInInspector]
	public Tilemap map;

	
	[HideInInspector]
	public GridLayout grid;

	
	public List<Tile> tiles;

	
	public Dictionary<string, List<Tile>> subTiles;

	
	public Dictionary<string, Tile> palette;

	
	public TilemapHandler.MapType type;

	
	public bool hasStarted;

	
	protected bool m_initializedTiles;

	
	public enum MapType
	{
		
		Environment,
		
		Exits,
		
		Enemies,
		
		Placeables,
		
		UI,

		Nodes
	}
}
