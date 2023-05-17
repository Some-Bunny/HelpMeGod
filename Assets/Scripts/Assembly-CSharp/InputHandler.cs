using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
	
	
	 
	public BrushButton.BrushType BrushType
	{
		get
		{
			return this.m_brushType;
		}
		set
		{
			if (value == BrushButton.BrushType.SELECT)
			{
				this.m_selectedTile = null;
				this.attributesWindow.gameObject.SetActive(true);
				this.palettesWindow.SetActive(false);
			}
			else
			{
				this.attributesWindow.gameObject.SetActive(false);
				this.palettesWindow.SetActive(true);
			}
			this.m_brushType = value;
			BrushButton.UpdateAppearances();
		}
	}

	
	
	public Vector2 MousePosition
	{
		get
		{
			return this.m_mousePosition;
		}
	}

	
	
	public Vector2 MouseLastPosition
	{
		get
		{
			return this.m_mouseLastPosition;
		}
	}

	
	
	public Tile selectedTileType
	{
		get
		{
			return this.m_selectedTile;
		}
		set
        {
			m_selectedTile = value;
        }
	}

	 
	public void SetSelectedTile(Tile tile, TilemapHandler.MapType type)
	{
		if (this.BrushType == BrushButton.BrushType.ERASER)
		{
			this.BrushType = BrushButton.BrushType.BRUSH;
		}
		this.m_selectedTile = tile;
		this.lastTiles[type] = tile;

		//selectedThingIsCustom = ;

		ToggleDeleteButton(tile.name.Contains("customEnemyAsset-") || tile.name.Contains("customPlaceableAsset-"));
	}

	 
	private void Awake()
	{
		InputHandler.Instance = this;
		this.activeTilemap = TilemapHandler.MapType.Environment;
		this.m_selectedTile = null;
		this.lastTiles = new Dictionary<TilemapHandler.MapType, Tile>();
		this.BrushType = BrushButton.BrushType.BRUSH;
		this.brushSize = 1;
		foreach (object type in Enum.GetValues(typeof(TilemapHandler.MapType)))
		{
			this.lastTiles.Add((TilemapHandler.MapType)type, null);
		}
		ToggleDeleteButton(false);
	}

	 
	private void Start()
	{
		this.uiMap = UnityEngine.Object.FindObjectOfType<UIMap>();
		BrushButton.UpdateAppearances();
		this.undoRedo = new UndoRedo();

		EnemyLayer.SetActive(!nodeMode);
		//NodeLayer.SetActive(nodeMode);
	}

	 
	public void SetActiveTilemap(TilemapHandler.MapType mapType)
	{
		this.DeselectTile();
		this.activeTilemap = mapType;
		this.m_selectedTile = this.lastTiles[mapType];
	}

	 
	private void Update()
	{
		this.m_mouseLastPosition = this.m_mousePosition;
		this.m_mousePosition = Input.mousePosition;
		this.HandleShortcuts();
		this.HandleCursor();
		this.HandleMouseInput();
		this.HandleTileSelectionColor();
	}

	 
	private void HandleShortcuts()
	{
		if (!this.shortcutsDisabled)
		{
			if (!this.AttributesPanel.Hovered)
			{
				if (Input.GetKey(KeyCode.LeftControl))
				{
					if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Z))
					{
						this.undoRedo.Redo();
					}
					else
					{
						if (Input.GetKeyDown(KeyCode.Z))
						{
							this.undoRedo.Undo();
						}
					}

					if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.S))
					{
						FileButton.SaveAs();
					}
					else
					{
						if (Input.GetKeyDown(KeyCode.S))
						{
							FileButton.Save();
						}
					}
					if (Input.GetKeyDown(KeyCode.N))
					{
						FileButton.New();
					}
					if (Input.GetKeyDown(KeyCode.O))
					{
						FileButton.Open();
					}
				}
				else
				{
					if (Input.GetKeyDown(KeyCode.G))
					{
						GridMap.Instance.Toggle();
					}
					if (Input.GetKeyDown(KeyCode.E))
					{
						this.BrushType = BrushButton.BrushType.ERASER;
					}
					if (Input.GetKeyDown(KeyCode.B))
					{
						this.BrushType = BrushButton.BrushType.BRUSH;
					}
					if (Input.GetKeyDown(KeyCode.P))
					{
						this.BrushType = BrushButton.BrushType.PENCIL;
					}
					if (Input.GetKeyDown(KeyCode.F))
					{
						this.BrushType = BrushButton.BrushType.BUCKET;
					}
					if (Input.GetKeyDown(KeyCode.L))
					{
						this.BrushType = BrushButton.BrushType.LINE;
					}
					if (Input.GetKeyDown(KeyCode.R))
					{
						this.BrushType = BrushButton.BrushType.RECTANGLE;
					}
					if (Input.GetKeyDown(KeyCode.C))
					{
						this.BrushType = BrushButton.BrushType.ELLIPSE;
					}
				}
			}
		}
	}

	 
	public void SelectTile(Vector3Int tilePosition)
	{
		this.DeselectTile();
		Tilemap map = Manager.Instance.GetTilemap(this.activeTilemap).map;

		tilePosition = new Vector3Int(tilePosition.x, tilePosition.y, 0);

		this.selectedTile = (map.GetTile(tilePosition) as Tile);
		this.selectedTilePosition = tilePosition;
		if (this.selectedTile)
		{
			map.SetTileFlags(tilePosition, TileFlags.None);
			this.attributesWindow.Repopulate();
		}


	}

	 
	public void DeselectTile()
	{
		Tilemap map = Manager.Instance.GetTilemap(this.activeTilemap).map;
		bool flag = this.selectedTile;
		if (flag)
		{
			map.SetColor(this.selectedTilePosition, Color.white);
			this.selectedTile = null;
			this.attributesWindow.Repopulate();
		}
	}


	public void ToggleDeleteButton(bool enabled)
	{
		deleteButton.interactable = enabled;

		deleteButton.GetComponentInChildren<Text>().color = (enabled ? Color.white : Color.gray);
	}

	public Button deleteButton;

	public void HandleTileSelectionColor()
	{
		bool flag = this.selectedTile;
		if (flag)
		{
			Manager.Instance.GetTilemap(this.activeTilemap).map.SetColor(this.selectedTilePosition, Color.Lerp(Color.white, new Color(0f, 1f, 0.5f), (float)(Math.Sin((double)(Time.realtimeSinceStartup * 5f)) * 0.5) + 0.5f));
		}
	}

	 
	private void HandleMouseInput()
	{
		this.controlHeld = Input.GetKey(KeyCode.LeftControl);
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (this.EditorPanel.Hovered)
			{
				this.isDrawing = true;
				if (this.m_brushType == BrushButton.BrushType.LINE || this.m_brushType == BrushButton.BrushType.RECTANGLE || this.m_brushType == BrushButton.BrushType.ELLIPSE)
				{
					if (nodeMode)
					{
						BrushType = BrushButton.BrushType.BRUSH;
					}
					else
                    {
						this.isDrawingComplex = true;
						this.complexStartPos = Camera.main.ScreenToWorldPoint(this.m_mousePosition);
						this.complexStartPos.z = 0f;
					}

				}
				if (this.m_brushType == BrushButton.BrushType.SELECT)
				{
					this.SelectTile(this.MouseToGridPosition());
				}
				else
				{
					this.undoRedo.RegisterState(Manager.Instance.GetTilemap(this.activeTilemap), this.BrushType.ToString());
				}
			}
		}
		if (Input.GetKey(KeyCode.Mouse0))
		{
			this.HideDropdowns();
			Action<Vector3> onMousePressed = this.OnMousePressed;
			if (onMousePressed != null)
			{
				onMousePressed(this.MousePosition);
			}
			if (this.EditorPanel.Hovered)
			{
				if (this.isDrawing)
				{
					Manager.Instance.GetTilemap(this.activeTilemap).HandleMouseDown(this.MouseToGridPosition(), this.m_brushType);
					if (this.isDrawingComplex)
					{
						this.complexEndPos = Camera.main.ScreenToWorldPoint(this.m_mousePosition);
						this.complexEndPos.z = 0f;
					}
				}
			}
		}
		else
		{
			this.isDrawing = false;
			if (this.isDrawingComplex)
			{
				this.DrawComplexShape();
				this.isDrawingComplex = false;
			}
		}
		if (Input.GetKeyDown(KeyCode.Mouse2))
		{
			if (this.EditorPanel.Hovered)
			{
				this.isPanning = true;
				this.panStartPos = Input.mousePosition;
			}
		}
		if (Input.GetKey(KeyCode.Mouse2))
		{
			if (this.isPanning)
			{
				this.Pan();
			}
		}
		if (this.EditorPanel.Hovered)
		{
			this.HandleScroll(Input.mouseScrollDelta.y);
		}
		if (!Input.GetKey(KeyCode.Mouse2) && this.isPanning)
		{
			this.isPanning = false;
			if (Vector3.Distance(Input.mousePosition, this.panStartPos) < 0.1f)
			{
				this.grid.transform.position = new Vector3(3f, 0f, this.grid.transform.position.z);
			}
		}
	}

	public void DrawComplexShape()
	{
		BrushButton.BrushType brushType = this.m_brushType;
		if (brushType != BrushButton.BrushType.RECTANGLE)
		{
			if (brushType != BrushButton.BrushType.ELLIPSE)
			{
				Manager.Instance.GetTilemap(this.activeTilemap).FillLine(this.complexStartPos, this.complexEndPos, this.selectedTileType, this.brushSize);
			}
			else
			{
				if (this.brushMode == 1)
				{
					Manager.Instance.GetTilemap(this.activeTilemap).FillEllipse(this.complexStartPos, this.complexEndPos, this.selectedTileType, this.brushSize, this.controlHeld);
				}
				else
				{
					Manager.Instance.GetTilemap(this.activeTilemap).OutlineEllipse(this.complexStartPos, this.complexEndPos, this.selectedTileType, this.brushSize, this.controlHeld);
				}
			}
		}
		else
		{
			if (this.brushMode == 1)
			{
				Manager.Instance.GetTilemap(this.activeTilemap).FillRectangle(this.complexStartPos, this.complexEndPos, this.selectedTileType, this.brushSize, this.controlHeld);
			}
			else
			{
				Manager.Instance.GetTilemap(this.activeTilemap).OutlineRectangle(this.complexStartPos, this.complexEndPos, this.selectedTileType, this.brushSize, this.controlHeld);
			}
		}
	}

	 
	public void HideDropdowns()
	{
		foreach (HideableObject obj in UnityEngine.Object.FindObjectsOfType<HideableObject>())
		{
			bool activeSelf = obj.gameObject.activeSelf;
			if (activeSelf)
			{
				bool flag = obj.gameObject.activeSelf && Time.time - obj.showTime > 0.1f && obj.hideOnClickElsewhere && !obj.GetComponent<MouseListener>().Hovered;
				if (flag)
				{
					obj.Hide();
				}
			}
		}
	}

	 
	private void HandleScroll(float amount)
	{
		bool flag = amount != 0f;
		if (flag)
		{
			bool flag2 = amount > 0f;
			if (flag2)
			{
			}
			Vector3 s = this.grid.transform.localScale;
			s.x = Mathf.Clamp(s.x + amount / 10f, 0.15f, 2f);
			s.y = s.x;
			this.ScaleAround(this.grid.gameObject, Camera.main.ScreenToWorldPoint(new Vector3(this.m_mousePosition.x, this.m_mousePosition.y, this.grid.transform.localPosition.z)), s);
		}
	}

	 
	private void ScaleAround(GameObject target, Vector3 pivot, Vector3 newScale)
	{
		Vector3 A = target.transform.localPosition;
		Vector3 C = A - pivot;
		float RS = newScale.x / target.transform.localScale.x;
		Vector3 FP = pivot + C * RS;
		target.transform.localScale = newScale;
		target.transform.localPosition = FP;
	}

	 
	private void Pan()
	{
		Vector3 d = this.m_mousePosition - this.m_mouseLastPosition;
		this.grid.transform.position += d / 61f;
	}

	 
	private void HandleCursor()
	{
		bool flag = !this.EditorPanel.Hovered;
		if (flag)
		{
			this.uiMap.ClearCursor();
		}
		else
		{
			bool flag2 = this.isDrawingComplex;
			if (flag2)
			{
				this.HandleComplexCursor();
			}
			else
			{
				Vector3Int hoveredCell = this.MouseToGridPosition();
				if (!TilemapHandler.InBounds(hoveredCell))
				{
					this.uiMap.ClearCursor();
					//Debug.Log("cursor is oob");
				}
				else
				{
					this.uiMap.SetCursor(hoveredCell);
				}
			}
		}
	}

	 
	private void HandleComplexCursor()
	{
		bool flag = !this.EditorPanel.Hovered;
		if (flag)
		{
			this.uiMap.ClearCursor();
		}
		else
		{
			BrushButton.BrushType brushType = this.m_brushType;
			if (brushType != BrushButton.BrushType.RECTANGLE)
			{
				if (brushType != BrushButton.BrushType.ELLIPSE)
				{
					this.uiMap.SetLineCursor(this.grid.WorldToCell(this.complexStartPos), this.grid.WorldToCell(this.complexEndPos));
				}
				else
				{
					bool flag2 = this.brushMode == 1;
					if (flag2)
					{
						this.uiMap.SetFilledEllipseCursor(this.grid.WorldToCell(this.complexStartPos), this.grid.WorldToCell(this.complexEndPos));
					}
					else
					{
						this.uiMap.SetOutlinedEllipseCursor(this.grid.WorldToCell(this.complexStartPos), this.grid.WorldToCell(this.complexEndPos));
					}
				}
			}
			else
			{
				bool flag3 = this.brushMode == 1;
				if (flag3)
				{
					this.uiMap.SetFilledRectangleCursor(this.grid.WorldToCell(this.complexStartPos), this.grid.WorldToCell(this.complexEndPos));
				}
				else
				{
					this.uiMap.SetOutlinedRectangleCursor(this.grid.WorldToCell(this.complexStartPos), this.grid.WorldToCell(this.complexEndPos));
				}
			}
		}
	}

	 
	private Vector3Int MouseToGridPosition()
	{
		return this.grid.WorldToCell(Camera.main.ScreenToWorldPoint(this.m_mousePosition));
	}

	 
	private Vector3Int MousePositionToGridPosition(Vector2 mousePosition)
	{
		return this.grid.WorldToCell(Camera.main.ScreenToWorldPoint(mousePosition));
	}

	 
	public void Undo()
	{
		this.undoRedo.Undo();
	}

	 
	public void Redo()
	{
		this.undoRedo.Redo();
	}

	 
	public void ClearUndoRedo()
	{
		this.undoRedo = new UndoRedo();
	}

	public void ToggleNodeMode()
    {
		nodeMode = !nodeMode;

		EnemyLayer.SetActive(!nodeMode);
		NodeLayer.SetActive(nodeMode);	

		InputHandler.Instance.BrushType = BrushButton.BrushType.BRUSH;

		NodePaletteDropdown.Instance.gameObject.transform.parent.gameObject.SetActive(nodeMode);
		PaletteDropdown.Instance.gameObject.transform.parent.gameObject.SetActive(!nodeMode);

		if (InputHandler.Instance.nodeMode)
		{			
			NodePaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Nodes);

			Manager.Instance.environment.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f);
			Manager.Instance.exits.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f);
			Manager.Instance.placeables.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f);
			EnemyLayerHandler.Instance.enemyMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f));
			NodePathLayerHandler.Instance.nodeMaps.ForEach(x => x.GetComponent<Tilemap>().color = Color.white);
		}
		else
		{			
			PaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Environment);

			Manager.Instance.environment.GetComponent<Tilemap>().color = Color.white;
			Manager.Instance.exits.GetComponent<Tilemap>().color = Color.white;
			Manager.Instance.placeables.GetComponent<Tilemap>().color = Color.white;
			EnemyLayerHandler.Instance.enemyMaps.ForEach(x => x.GetComponent<Tilemap>().color = Color.white);
			NodePathLayerHandler.Instance.nodeMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f));
		}

	}

	
	public static InputHandler Instance;

	
	public MouseListener EditorPanel;

	
	public MouseListener OptionsBarPanel;

	
	public MouseListener InspectorPanel;

	
	public MouseListener AttributesPanel;

	
	public UIMap uiMap;

	
	public GridLayout grid;

	
	public AttributesHandler attributesWindow;

	
	public GameObject palettesWindow;

	
	public TilemapHandler.MapType activeTilemap;

	
	private BrushButton.BrushType m_brushType;

	
	public bool isPanning;

	
	public bool isDrawing;

	
	public bool isDrawingComplex;

	
	public bool shortcutsDisabled;

	
	private Vector2 m_mousePosition;

	
	private Vector2 m_mouseLastPosition;

	
	public Action<Vector3> OnMousePressed;

	
	private UndoRedo undoRedo;

	
	public int brushSize = 1;

	
	public int brushMode = 0;

	
	private Dictionary<TilemapHandler.MapType, Tile> lastTiles;

	
	private Tile m_selectedTile;

	
	private Vector3 panStartPos;

	
	public Vector3 complexStartPos;

	
	public Vector3 complexEndPos;

	
	public bool controlHeld;
	public bool nodeMode;

	public bool selectedThingIsCustom;

	
	public Tile selectedTile = null;

	
	private Vector3Int selectedTilePosition = Vector3Int.zero;

	public GameObject EnemyLayer;
	public GameObject NodeLayer;
}
