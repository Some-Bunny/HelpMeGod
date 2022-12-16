using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CanvasHandler : MonoBehaviour
{
	
	private void Start()
	{
		CanvasHandler.Instance = this;
	}

	
	public void Trim()
	{
		Tilemap envMap = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map;
		BoundsInt bounds = CanvasHandler.GetTrimmedBounds(envMap);
		Debug.Log(bounds);
		BoundsInt newBounds = new BoundsInt(-bounds.size.x / 2, -bounds.size.y / 2, 0, bounds.size.x, bounds.size.y, 1);
		Manager.roomSize.x = newBounds.size.x;
		Manager.roomSize.y = newBounds.size.y;
		TilemapHandler.Bounds = newBounds;
		foreach (TilemapHandler map in UnityEngine.Object.FindObjectsOfType<TilemapHandler>())
		{
			TileBase[] tiles = map.map.GetTilesBlock(bounds);
			map.map.ClearAllTiles();
			map.ResizeBounds();
			map.map.SetTilesBlock(newBounds, tiles);
		}
		GridMap gridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
		if (gridMap != null)
		{
			gridMap.ResetAppearance();
		}
		InputHandler.Instance.ClearUndoRedo();
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
			TileBase[] tiles = map.map.GetTilesBlock(bounds);
			map.map.ClearAllTiles();
			map.ResizeBounds();
			map.map.SetTilesBlock(newBounds, tiles);
		}
		GridMap gridMap = UnityEngine.Object.FindObjectOfType<GridMap>();
		if (gridMap != null)
		{
			gridMap.ResetAppearance();
		}
		InputHandler.Instance.ClearUndoRedo();
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
