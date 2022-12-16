using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class GridMap : TilemapHandler
{
	 
	protected override void Awake()
	{
		base.Awake();
		GridMap.Instance = this;
	}

	
	protected override void Start()
	{
		base.Start();
		base.gameObject.SetActive(GridMap.toggled);
		this.ResetAppearance();
	}

	
	public void Toggle()
	{
		bool active = !base.gameObject.activeSelf;
		GridMap.toggled = active;
		base.gameObject.SetActive(active);
		bool flag = active;
		if (flag)
		{
			this.ResetAppearance();
		}
	}

	
	public void ResetAppearance()
	{
		this.map.ClearAllTiles();
		base.ResizeBounds();
		this.map.FloodFill(this.map.origin, this.gridTile);
	}

	
	public void BoxFill()
	{
		Vector3Int pos = Vector3Int.zero;
		int xmin = TilemapHandler.Bounds.xMin;
		int ymin = TilemapHandler.Bounds.yMin;
		int xmax = TilemapHandler.Bounds.xMax;
		int ymax = TilemapHandler.Bounds.yMax;
		for (int x = xmin; x < xmax; x++)
		{
			for (int y = ymin; y < ymax; y++)
			{
				pos.x = x;
				pos.y = y;
				this.map.SetTile(pos, this.gridTile);
			}
		}
	}

	
	public override TileDatabase InitializeDatabase()
	{
		return null;
	}

	
	public static bool toggled;

	
	public static GridMap Instance;

	
	public TileBase gridTile;
}
