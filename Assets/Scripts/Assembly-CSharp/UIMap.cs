using System;
using UnityEngine;
using UnityEngine.Tilemaps;


public class UIMap : TilemapHandler
{
	
	public void SetCursor(Vector3Int pos)
	{
		if (pos.Equals(cursorPos)) return;


		//map.SetTile(cursorPos, null);
		//map.SetTile(pos, InputHandler.Instance.selectedTileType);

		this.ClearCursor();
		base.SetTileWithThickness(this.map, pos, InputHandler.Instance.selectedTileType, InputHandler.Instance.brushSize);
		this.cursorPos = pos;
		
	}

	
	public void SetLineCursor(Vector3Int start, Vector3Int end)
	{
		this.map.ClearAllTiles();
		base.FillLine(this.grid.CellToWorld(start), this.grid.CellToWorld(end), InputHandler.Instance.selectedTileType, InputHandler.Instance.brushSize);
		this.cursorPos = end;
	}

	
	public void SetOutlinedRectangleCursor(Vector3Int start, Vector3Int end)
	{
		this.map.ClearAllTiles();
		base.OutlineRectangle(this.grid.CellToWorld(start), this.grid.CellToWorld(end), InputHandler.Instance.selectedTileType, InputHandler.Instance.brushSize, InputHandler.Instance.controlHeld);
		this.cursorPos = end;
	}

	
	public void SetFilledRectangleCursor(Vector3Int start, Vector3Int end)
	{
		this.map.ClearAllTiles();
		base.FillRectangle(this.grid.CellToWorld(start), this.grid.CellToWorld(end), InputHandler.Instance.selectedTileType, InputHandler.Instance.brushSize, InputHandler.Instance.controlHeld);
		this.cursorPos = end;
	}

	
	public void SetOutlinedEllipseCursor(Vector3Int start, Vector3Int end)
	{
		this.map.ClearAllTiles();
		base.OutlineEllipse(this.grid.CellToWorld(start), this.grid.CellToWorld(end), InputHandler.Instance.selectedTileType, InputHandler.Instance.brushSize, InputHandler.Instance.controlHeld);
		this.cursorPos = end;
	}

	
	public void SetFilledEllipseCursor(Vector3Int start, Vector3Int end)
	{
		this.map.ClearAllTiles();
		base.FillEllipse(this.grid.CellToWorld(start), this.grid.CellToWorld(end), InputHandler.Instance.selectedTileType, InputHandler.Instance.brushSize, InputHandler.Instance.controlHeld);
		this.cursorPos = end;
	}

	
	public void ClearCursor()
	{
		this.map.ClearAllTiles();
		this.cursorPos = new Vector3Int(-1, -1, -1);
	}

	
	public override TileDatabase InitializeDatabase()
	{
		return null;
	}

	
	public TileBase cursor;

	
	private Vector3Int cursorPos = default(Vector3Int);
}
