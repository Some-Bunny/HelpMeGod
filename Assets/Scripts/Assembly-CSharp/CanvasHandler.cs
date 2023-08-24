using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;


public class CanvasHandler : MonoBehaviour
{
	
	private void Start()
	{
		CanvasHandler.Instance = this;
	}

	
	public void Trim()
	{
		Tilemap envMap = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map;

        var stored = envMap.cellBounds;


        BoundsInt bounds = CanvasHandler.GetTrimmedBounds(envMap);
		Debug.Log(bounds);

        stored.x -= bounds.x;
        stored.y -= bounds.y;

        BoundsInt newBounds = new BoundsInt(-bounds.size.x / 2, -bounds.size.y / 2, 0, bounds.size.x, bounds.size.y, 1);
		Manager.roomSize.x = newBounds.size.x;
		Manager.roomSize.y = newBounds.size.y;
		TilemapHandler.Bounds = newBounds;
		foreach (TilemapHandler map in UnityEngine.Object.FindObjectsOfType<TilemapHandler>())
		{
            if (map is NodeMap pain)
            {

                int c = pain.fuckYou.Count;
                TileBase[] tiles = pain.map.GetTilesBlock(bounds);
                Dictionary<int, DataTile> peepee = new Dictionary<int, DataTile>();
                foreach (var tile in tiles)
                {
                    if ((tile is DataTile))
                    {
                        peepee.Add((tile as DataTile).PositionInNodeMap(pain), (tile as DataTile));
                    }
                }
                map.map.ClearAllTiles();
                pain.fuckYou.Clear();
                map.ResizeBounds();
                map.map.SetTilesBlock(newBounds, tiles);
                for (int i = 0; i < c; i++)
                {
                    DataTile s;
                    peepee.TryGetValue(i, out s);
                    if (s != null)
                    {
                        pain.fuckYou.Add(s);
                    }
                }

                pain.UpdateAtrributeList();

            }
            else
            {
                TileBase[] tiles = map.map.GetTilesBlock(bounds);
                map.map.ClearAllTiles();
                map.ResizeBounds();
                map.map.SetTilesBlock(newBounds, tiles);
            }
        }

        GridMap gridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
		if (gridMap != null)
		{
			gridMap.ResetAppearance();
		}
		InputHandler.Instance.ClearUndoRedo();
        BoarderHandler.Instance.ResetAppearance();
    }

    public void Resize(int t, int b, int r, int l)
	{
		Tilemap envMap = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map;
		BoundsInt bounds = envMap.cellBounds;
		Debug.Log(bounds);

		bounds.position -= new Vector3Int(l, b, 0);
		bounds.max += new Vector3Int(r + l, t + b, 0);


        BoundsInt newBounds = new BoundsInt(-bounds.size.x / 2, -bounds.size.y / 2, 0, bounds.size.x, bounds.size.y, 1);
		Manager.roomSize.x = newBounds.size.x;
		Manager.roomSize.y = newBounds.size.y;
		TilemapHandler.Bounds = newBounds;


        foreach (TilemapHandler map in UnityEngine.Object.FindObjectsOfType<TilemapHandler>())
        {
			if (map is NodeMap pain)
			{
                int c = pain.fuckYou.Count;
                TileBase[] tiles = pain.map.GetTilesBlock(bounds);
                Dictionary<int, DataTile> peepee = new Dictionary<int, DataTile>();
                foreach (var tile in tiles)
                {
                    if ((tile is DataTile))
                    {
                        peepee.Add((tile as DataTile).PositionInNodeMap(pain), (tile as DataTile));
                    }
                }
                map.map.ClearAllTiles();
                pain.fuckYou.Clear();
                map.ResizeBounds();
                map.map.SetTilesBlock(newBounds, tiles);
                for (int i = 0; i < c; i++)
                {
                    DataTile s;
                    peepee.TryGetValue(i, out s);
                    if (s != null)
                    {
                        pain.fuckYou.Add(s);
                    }
                }

                pain.UpdateAtrributeList();

            }
            else
			{
				TileBase[] tiles = map.map.GetTilesBlock(bounds);
				map.map.ClearAllTiles();
				map.ResizeBounds();
				map.map.SetTilesBlock(newBounds, tiles);
			}
        }
        GridMap gridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
        if (gridMap != null)
        {
            gridMap.ResetAppearance();
        }
        InputHandler.Instance.ClearUndoRedo();
        BoarderHandler.Instance.ResetAppearance();
    }

    public Vector3Int? GetCellInWorld(TileBase myTile, TilemapHandler tilemap, BoundsInt bounds)
    {
        Tile[,] tiles = tilemap.AllTiles();
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[x, y] == null) continue;
                var tile = tiles[x, y];
                if (tile == myTile)
                {
                    return new Vector3Int(x, y, 0);
                }              
            }
        }
        return null;
    }




    public static BoundsInt GetTrimmedBounds(Tilemap map)
	{
		map.CompressBounds();
		BoundsInt bounds = map.cellBounds;
		map.ResizeBounds();
		return bounds;
	}

	
	public static CanvasHandler Instance;
}
