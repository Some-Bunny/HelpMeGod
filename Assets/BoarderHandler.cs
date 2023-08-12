using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BoarderHandler : TilemapHandler
{

	protected override void Awake()
	{
		base.Awake();
		BoarderHandler.Instance = this;
	}


	protected override void Start()
	{
		base.Start();
		base.gameObject.SetActive(BoarderHandler.toggled);
		this.ResetAppearance();
	}


	public void Toggle()
	{
		Debug.Log($"is on {!base.gameObject.activeSelf}");

		BoarderHandler.toggled = !base.gameObject.activeSelf;
		base.gameObject.SetActive(BoarderHandler.toggled);
		if (BoarderHandler.toggled)
		{
			this.ResetAppearance();
		}

	}


	public void ResetAppearance()
	{
		this.map.ClearAllTiles();
		base.ResizeBounds();
		BoxFill();
		//this.map.BoxFill(this.map.origin, this.gridTile, TilemapHandler.Bounds.xMin, TilemapHandler.Bounds.yMin, TilemapHandler.Bounds.xMax, TilemapHandler.Bounds.yMax);
	}


	public void BoxFill()
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
			this.map.SetTile(pos, this.gridTile);
			pos.y = ymin;
			this.map.SetTile(pos, this.gridTile);
		}
		for (int y = ymin; y < ymax; y++)
		{
			pos.y = y;

			pos.x = xmax - 1;			
			this.map.SetTile(pos, this.gridTile);
			pos.x = xmin;
			this.map.SetTile(pos, this.gridTile);
		}
	}


	public override TileDatabase InitializeDatabase()
	{
		return null;
	}


	public static bool toggled;


	public static BoarderHandler Instance;


	public TileBase gridTile;
}
