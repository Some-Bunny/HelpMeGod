using Assets.Scripts.Assembly_CSharp;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using static DataTile;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Tile = UnityEngine.Tilemaps.Tile;

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
				if (this.activeTilemap != TilemapHandler.MapType.Nodes)
				{
                    this.palettesWindow.SetActive(true);
                }
            }
			this.m_brushType = value;
			BrushButton.UpdateAppearances();
            BrushSizeButton.UpdateAppearances();
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

        var t = Manager.Instance.GetTilemap(this.activeTilemap);


        this.m_selectedTile = t.CopiedTile != null ? t.CopiedTile : tile;
		this.lastTiles[type] = t.CopiedTile != null ? t.CopiedTile : tile;

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

		this.StartCoroutine(Delay());

    }

	private IEnumerator Delay()
	{
		yield return null;
        if (LineRenderer_Dynamic_Instance == null) LineRenderer_Dynamic_Instance = new GameObject("Line Renderer");

        //if (LineRenderer != null) { Debug.LogError("NOT NULL"); }
        var renderer = LineRenderer_Dynamic_Instance.gameObject.GetOrAddComponent<LineRenderer>();
        renderer.enabled = true;//(i == index);
        renderer.material = new Material(Shader.Find("Sprites/Default"));
        renderer.startColor = Color.yellow;
        renderer.endColor = Color.yellow;
		renderer.renderingLayerMask = 1;
		var tt = renderer.transform.localPosition;
		tt.z = -10;
        renderer.colorGradient = new Gradient()
        {
            mode = GradientMode.Fixed,
            colorKeys = new GradientColorKey[]
            {
                    new GradientColorKey() { color = Color.yellow * 2, time = 0},
            },
            alphaKeys = new GradientAlphaKey[]
            {
                    new GradientAlphaKey() { alpha =1, time = 1},
            }
        };
        renderer.startWidth = 0.1f * this.grid.transform.localScale.x;
        renderer.loop = true;
        renderer.positionCount = 0;//inst.GetMap(inst.EnemyMapIndex).fuckYou.Count;



        LineRenderer_Dynamic_Instance.transform.parent = this.grid.gameObject.transform;
        LineRenderer_Dynamic_Instance.transform.position = this.grid.gameObject.transform.position;
        yield break;
	}

	
    private void Start()
	{
		this.uiMap = UnityEngine.Object.FindObjectOfType<UIMap>();
		BrushButton.UpdateAppearances();
        BrushSizeButton.UpdateAppearances();

        this.undoRedo = new UndoRedo();

		EnemyLayer.SetActive(!nodeMode);
		//NodeLayer.SetActive(nodeMode);
	}

	 
	public void SetActiveTilemap(TilemapHandler.MapType mapType)
	{
		this.DeselectTile();
		this.activeTilemap = mapType;
		this.m_selectedTile = this.lastTiles[mapType];
		BrushButton.UpdateAppearances();
        BrushSizeButton.UpdateAppearances();
    }


    private void Update()
	{
		this.m_mouseLastPosition = this.m_mousePosition;
		this.m_mousePosition = Input.mousePosition;
		this.HandleShortcuts();
		this.HandleCursor();
		this.HandleMouseInput();
		this.HandleTileSelectionColor();
        UpdateNodeLines();

		if (selectedTile != null && dynamicVisualsComponents.ContainsKey(selectedTile.name))
		{
			Action<DataTile, LineRenderer> a;
            dynamicVisualsComponents.TryGetValue(selectedTile.name, out a);
            if (a != null && LineRenderer_Dynamic_Instance != null)
			{
                a(selectedTile as DataTile, LineRenderer_Dynamic_Instance.gameObject.GetOrAddComponent<LineRenderer>());

                if (p != false)
				{
                    p = false;
                    Tilemap map = Manager.Instance.GetTilemap(this.activeTilemap).map;
                    Manager.Instance.environment.GetComponent<Tilemap>().color = map == Manager.Instance.environment.GetComponent<Tilemap>() ? new Color(1f, 1f, 1f, 0.65f) : new Color(1f, 1f, 1f, 0.3f);
                    Manager.Instance.exits.GetComponent<Tilemap>().color = map == Manager.Instance.exits.GetComponent<Tilemap>() ? new Color(1f, 1f, 1f, 0.65f) : new Color(1f, 1f, 1f, 0.3f);
                    Manager.Instance.placeables.GetComponent<Tilemap>().color = map == Manager.Instance.placeables.GetComponent<Tilemap>() ? new Color(1f, 1f, 1f, 0.65f) : new Color(1f, 1f, 1f, 0.3f);
                    EnemyLayerHandler.Instance.DoTransparency();//.enemyMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f));
                    NodePathLayerHandler.Instance.nodeMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.1f));
                }
            }
        }
		else
		{
            if (LineRenderer_Dynamic_Instance != null)
            {
                var t = LineRenderer_Dynamic_Instance.gameObject.GetOrAddComponent<LineRenderer>();
                t.positionCount = 0;
            }
			if (!InputHandler.Instance.nodeMode && p != true)
			{
                //Debug.LogError("A + " + p.ToString());
                p = true;
				//Debug.LogError("A + " + p.ToString());
                Manager.Instance.environment.GetComponent<Tilemap>().color = Color.white;
                Manager.Instance.exits.GetComponent<Tilemap>().color = Color.white;
                Manager.Instance.placeables.GetComponent<Tilemap>().color = Color.white;
                EnemyLayerHandler.Instance.SetSelectedLayer(EnemyLayerHandler.Instance.ReturnButtons()[EnemyLayerHandler.Instance.EnemyMapIndex]);
                NodePathLayerHandler.Instance.nodeMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f));
            }
        }
    }

	private bool p;


    public static Dictionary<string, Action<DataTile, LineRenderer>> dynamicVisualsComponents = new Dictionary<string, Action<DataTile, LineRenderer>>()
    {
        {"winchesterCameraPanPlacer",  DynamicTileMethods.DynamicWinchester },
        {"conveyor_belt_up",  DynamicTileMethods.DynamicConveyors },
        {"conveyor_belt_right",  DynamicTileMethods.DynamicConveyors },
        {"pew",  DynamicTileMethods.DynamicPews },

        {"spinning_log_spike_horizontal_001",  DynamicTileMethods.DynamicRollersHeight },
        {"spinning_ice_log_spike_horizontal_001001",  DynamicTileMethods.DynamicRollersHeight },
        {"spinning_log_spike_vertical_001",  DynamicTileMethods.DynamicRollersLength },

        {"spinning_ice_log_spike_vertical_001",  DynamicTileMethods.DynamicRollersLength },

        {"sewer_platform_moving_001",  DynamicTileMethods.DynamicMovingTiles },
        {"gungeon_platform_moving_001",  DynamicTileMethods.DynamicMovingTiles },
        {"mines_platform_moving_001",  DynamicTileMethods.DynamicMovingTiles },
        {"catacombs_platform_moving_001",  DynamicTileMethods.DynamicMovingTiles },
        {"forge_platform_moving_001",  DynamicTileMethods.DynamicMovingTiles },
    };


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
                    if (Input.GetKeyDown(KeyCode.N))
                    {
						this.ToggleNodeMode();

                    }
                    if (Input.GetKeyDown(KeyCode.X))
                    {
						if (this.nodeMode == true)
						{
                            NodesVisualEnabled = !NodesVisualEnabled;
                            NotificationHandler.Instance.Notify("Node Path Visual is" + (NodesVisualEnabled == true ? " enabled." : " disabled."));
                        }
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


    public void CopyTile(Vector3Int tilePosition)
    {
        this.DeselectTile();
		var t = Manager.Instance.GetTilemap(this.activeTilemap);
        Tilemap map = t.map;

        tilePosition = new Vector3Int(tilePosition.x, tilePosition.y, 0);

        this.selectedTile = (map.GetTile(tilePosition) as Tile);
        this.selectedTilePosition = tilePosition;
        if (this.selectedTile)
        {
            NotificationHandler.Instance.Notify("[ Copied Tile: " + this.selectedTile.name + " ]");
            map.SetTileFlags(tilePosition, TileFlags.None);
            this.attributesWindow.Repopulate();
            t.CopiedTile = this.selectedTile;
        }
    }



    public void DeselectTile()
	{
		Tilemap map = Manager.Instance.GetTilemap(this.activeTilemap).map;
		bool flag = this.selectedTile;
		if (flag)
		{
            var p = Manager.Instance.GetTilemap(this.activeTilemap);
            var tiles = p.AllTiles();
            foreach (var tile in tiles)
            {
                if (tile is DataTile)
                {
                    var data = (tile as DataTile);
                    if (tile == selectedTile)
                    {
                        data.color = Color.white;
                        RefreshTilesUniversal();

                    }
                }
            }
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

        if (this.selectedTile == null && BrushType == BrushButton.BrushType.SELECT)
        {
            p2 = true;
            var map = Manager.Instance.GetTilemap(this.activeTilemap);

            var tiles = map.AllTiles();
            foreach (var tile in tiles)
            {
                if (tile is DataTile)
                {
                    var data = (tile as DataTile);
                    int count = 0;
                    foreach (KeyValuePair<string, JToken> att in data.data)
                    {
                        count++;
                    }
                    if (count > 0)
                    {
                        data.color = Color.Lerp(Color.white, new Color(1f, 0.2f, 0f), (float)(Math.Sin((double)(Time.realtimeSinceStartup * 5f)) * 0.5) + 0.5f);
                    }
                }
            }
            RefreshTilesUniversal();
        }
		else if (this.selectedTile == null)
        {
			if (p2 == true)
			{
                p2 = false;
                var map = Manager.Instance.GetTilemap(this.activeTilemap);
                var tiles = map.AllTiles();
                foreach (var tile in tiles)
                {
                    if (tile is DataTile)
                    {
                        var data = (tile as DataTile);
                        int count = 0;
                        foreach (KeyValuePair<string, JToken> att in data.data)
                        {
                            count++;
                        }
                        if (count > 0)
                        {
                            data.color = Color.white;
                        }
                    }
                }
                RefreshTilesUniversal();
            }
		}
        bool flag = this.selectedTile;
        if (flag)
        {
            if (p2 == true)
            {
                p2 = false;
                var amap = Manager.Instance.GetTilemap(this.activeTilemap);
                var atiles = amap.AllTiles();
                foreach (var tile in atiles)
                {
                    if (tile is DataTile)
                    {
                        var data = (tile as DataTile);
                        int count = 0;
                        foreach (KeyValuePair<string, JToken> att in data.data)
                        {
                            count++;
                        }
                        if (count > 0)
                        {
                            data.color = Color.white;
                            RefreshTilesUniversal();
                        }
                    }
                }
            }

            var map = Manager.Instance.GetTilemap(this.activeTilemap);
            var tiles = map.AllTiles();
            foreach (var tile in tiles)
            {
                if (tile is DataTile)
                {
                    var data = (tile as DataTile);
					if (tile == selectedTile)
					{
                        data.color = Color.Lerp(Color.white, new Color(0f, 1f, 0.5f), (float)(Math.Sin((double)(Time.realtimeSinceStartup * 5f)) * 0.5) + 0.5f);
                        RefreshTilesUniversal();

                    }
                }
            }
        }
    }



	private bool p2;



	public static void RefreshTilesUniversal()
	{
        var fuckYou = FindObjectsOfType<Tilemap>();
        foreach (var pieceoffuckingshit in fuckYou)
        {
            pieceoffuckingshit.RefreshAllTiles();
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
				if (m_brushType == BrushButton.BrushType.COPY)
				{
                    CopyTile(this.MouseToGridPosition());
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
        if (Input.GetKey(KeyCode.Mouse1))
        {
			//if (this.m_brushType == BrushButton.BrushType.COPY)
			{
				var tileMap = Manager.Instance.GetTilemap(this.activeTilemap);

                if (tileMap.CopiedTile != null)
				{
                    NotificationHandler.Instance.Notify("[ Deselected Tile: " + tileMap.CopiedTile.name + " ]");
                    tileMap.CopiedTile = null;
                }
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



    public void UpdateNodeLines()
	{
        if (LineRenderer_Instance)
        {
            var inst = NodePathLayerHandler.Instance;
            var renderer = LineRenderer_Instance.gameObject.GetOrAddComponent<LineRenderer>();

            renderer.startWidth = 0.1f * this.grid.transform.localScale.x;
            renderer.loop = inst.ReturnButtons()[inst.EnemyMapIndex].WrapMode == Enums.SerializedPathWrapMode.LOOP;
			renderer.enabled = NodesVisualEnabled;
			if (NodesVisualEnabled == true)
			{


				List<string> cum = new List<string>()
				{
				};
                List<Vector2?> p = new List<Vector2?>();


				var Mapping = inst.GetMap(inst.EnemyMapIndex);

				//int center = TilemapHandler.Bounds.center;
				if (Mapping.fuckYou == null || Mapping.fuckYou.Count == 0)
				{
					renderer.positionCount = 0;
					renderer.enabled = false;

                }
                for (int e = 0; e < Mapping.fuckYou.Count; e++)
                {
                    var t = Mapping.fuckYou[e];

                    //H.Add(t.placmentOrder);
                    //cum.Add();



                    var nodeMap = inst.GetMap(inst.EnemyMapIndex);
					Vector2 pos = nodeMap.map.CellToWorld(t.worldIntPosition);
                    //p.Add);

                    //if (c != null && pos != null)
                    {
                        //var scaler = new Vector2(0f, 0f);
                        var localScale = this.grid.transform.localScale;
                        //var calc = (pos + (scaler)); //(pos + localScale) * localposition;

                        Vector3 peepee = new Vector3((pos).x, (pos).y, 20);
                        renderer.positionCount = Mapping.fuckYou.Count;
                        var g = ReturnOffset(Mapping.tileDatabase.AllEntries[t.name]);
                        renderer.SetPosition(e, peepee + new Vector3(g.x * localScale.x, g.y * localScale.y));
                    }


                    /*
                    Tile[,] tiles = nodeMap.AllTiles();
                    for (int x = 0; x < tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < tiles.GetLength(1); y++)
                        {
                            if (!tiles[x, y]) continue;
                            DataTile tile = tiles[x, y] as DataTile;
                            if (tile.isNode && tile == Mapping.fuckYou[e])
                            {
                                p.Add(nodeMap.map.CellToWorld(tile.worldIntPosition));
                            }
                        }
                    }
                    */
                }
                //H.Sort();
				/*
                for (int e = 0; e < H.Count; e++)
                {
                    Vector2? pos = null;
                    string c = null;
                    p.TryGetValue(H[e], out pos);
                    cum.TryGetValue(H[e], out c);

                }
				*/
                /*
                renderer.positionCount = inst.GetMap(inst.EnemyMapIndex).fuckYou.Count;
                
				var localScale = this.grid.transform.localScale;
				var hhh = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map.cellBounds.position;

                Vector2 localposition = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map.transform.position + new Vector3(hhh.x, hhh.y);


                
				*/
            }

            LineRenderer_Instance.transform.parent = NodePathLayerHandler.Instance.gameObject.transform;
            LineRenderer_Instance.transform.position = this.grid.transform.position;
        }
    }
	 
	public Vector2 ReturnOffset(string name)
	{
		switch (name)
		{
			case "Center":
				
				return new Vector2(0.5f,0.5f);
            case "North":
                return new Vector2(0.5f, 1f);
            case "NorthEast":
                return new Vector2(1f, 1f);
            case "East":
                return new Vector2(1f, 0.5f);
            case "SouthEast":
                return new Vector2(1f, 0f);
            case "South":
                return new Vector2(0.5f, 0f);
            case "SouthWest":
                return new Vector2(0f, 0f);
            case "West":
                return new Vector2(0, 0.5f);
            
			case "NorthWest":
                return new Vector2(0f, 1f);
            default:
                return new Vector2(0.5f, 0.5f);
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

	public static bool NodesVisualEnabled = true;
	 
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
			EnemyLayerHandler.Instance.DoTransparency();//.enemyMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f));
			NodePathLayerHandler.Instance.nodeMaps.ForEach(x => x.GetComponent<Tilemap>().color = Color.white);

			//ReturnButtons
			var inst = NodePathLayerHandler.Instance;
            inst.SetSelectedLayer(inst.ReturnButtons()[inst.EnemyMapIndex]);


			
            if (LineRenderer_Instance == null) LineRenderer_Instance = new GameObject("Line Renderer");

			//if (LineRenderer != null) { Debug.LogError("NOT NULL"); }
            var renderer = LineRenderer_Instance.gameObject.GetOrAddComponent<LineRenderer>();
            renderer.enabled = NodesVisualEnabled;//(i == index);
            renderer.material = new Material(Shader.Find("Sprites/Default"));
            renderer.startColor = Color.green;
            renderer.endColor = Color.red;
			renderer.colorGradient = new Gradient()
			{
				mode = GradientMode.Blend,
				colorKeys = new GradientColorKey[] 
				{ 
					new GradientColorKey() { color = Color.green * 2, time = 0},
                    new GradientColorKey() { color = Color.red* 2, time = 1}
                },
                alphaKeys = new GradientAlphaKey[]
				{
                    new GradientAlphaKey() { alpha =1, time = 0},
                    new GradientAlphaKey() { alpha =1, time = 1},
                }
            };
            renderer.startWidth = 0.1f * this.grid.transform.localScale.x;
            renderer.loop = true;
            renderer.positionCount = inst.GetMap(inst.EnemyMapIndex).fuckYou.Count;



            var localScale = this.grid.transform.localScale;
            for (int e = 0; e < inst.GetMap(inst.EnemyMapIndex).fuckYou.Count; e++)
            {
                var pos = inst.GetMap(inst.EnemyMapIndex).fuckYou[e].position;
                Vector3 peepee = new Vector3((pos * localScale).x, (pos * localScale).y, 1);
                Vector3 farrtfart = (new Vector2(0.5f, 0.5f) * localScale);
                renderer.SetPosition(e, peepee + (localScale) + farrtfart);
            }
            LineRenderer_Instance.transform.parent = NodePathLayerHandler.Instance.gameObject.transform;
			LineRenderer_Instance.transform.position = this.grid.transform.position;

        }
        else
		{
			PaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Environment);

			Manager.Instance.environment.GetComponent<Tilemap>().color = Color.white;
			Manager.Instance.exits.GetComponent<Tilemap>().color = Color.white;
			Manager.Instance.placeables.GetComponent<Tilemap>().color = Color.white;
            EnemyLayerHandler.Instance.SetSelectedLayer(EnemyLayerHandler.Instance.ReturnButtons()[EnemyLayerHandler.Instance.EnemyMapIndex]);

            if (NodePathLayerHandler.Instance.nodeMaps.Count > 0)
			{
                NodePathLayerHandler.Instance.nodeMaps.ForEach(x => x.GetComponent<Tilemap>().color = new Color(1f, 1f, 1f, 0.5f));
            }

        }
	}


    public GameObject LineRenderer_Dynamic_Instance;




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

	
	public Dictionary<TilemapHandler.MapType, Tile> lastTiles;

	
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

    public GameObject LineRenderer_Instance;

    [SerializeField]
    public GameObject LineRenderer;





}
