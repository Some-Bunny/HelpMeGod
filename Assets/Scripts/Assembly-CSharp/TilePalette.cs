using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class TilePalette : MonoBehaviour
{
	public bool Populated { get; set; }
	public void Show()
	{
		if (Manager.Instance.GetTilemap(this.mapType))
		{
			base.gameObject.SetActive(true);
			InputHandler.Instance.SetActiveTilemap(this.mapType);
			this.Populate();
			Debug.Log($"{this.mapType} shown");
		}
	}

	
	public void Hide()
	{
		base.gameObject.SetActive(false);
	}

	
	public void Depopulate()
	{
		Transform content = base.transform.Find("Viewport").Find("Content");
		foreach (Transform t in content.GetComponentsInChildren<Transform>())
		{
			bool flag = t != content;
			if (flag)
			{
				UnityEngine.Object.Destroy(t.gameObject);
			}
		}
		this.buttons = new Dictionary<string, List<TileButton>>();
		this.Populated = false;
	}

	
	private void Awake()
	{
		this.content = base.transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
		this.buttons = new Dictionary<string, List<TileButton>>();
		this.m_scrollbar = base.GetComponentInChildren<Scrollbar>();
	}

	
	private void Start()
	{
	}

	
	public void Populate()
	{
		if (this.Populated && this.currentSub != this.shownSub)
		{
			this.Depopulate();
		}
		if (!this.Populated)
		{
			TilemapHandler map = Manager.Instance.GetTilemap(this.mapType);
			if (map)
			{
				if (string.IsNullOrEmpty(this.currentSub))
				{
					foreach (Tile tile in map.tiles)
					{
						if (tile.name != Manager.paletteDividerGuid)
						{
                            //Debug.Log("A");

                            this.AddTileButton(tile);
						}
						else
						{
							//Debug.Log("B");
							this.AddPaletteDivider(map.tileDatabase.Entries.Keys.ToList<string>()[map.tiles.IndexOf(tile)]);
						}
						this.content.sizeDelta = new Vector2(this.content.sizeDelta.x, -this.GetCellPosition(this.buttons.Last<KeyValuePair<string, List<TileButton>>>().Value.Count - 1).y + this.CellSize);
					}
				}
				else
				{
					foreach (Tile tile2 in map.subTiles[this.currentSub])
					{
						if (tile2.name != Manager.paletteDividerGuid)
						{
							this.AddTileButton(tile2);
						}
						else
						{
							this.AddPaletteDivider(map.tileDatabase.SubEntries[this.currentSub].Keys.ToList<string>()[map.subTiles[this.currentSub].IndexOf(tile2)]);
						}
						this.content.sizeDelta = new Vector2(this.content.sizeDelta.x, -this.GetCellPosition(this.buttons.Last<KeyValuePair<string, List<TileButton>>>().Value.Count - 1).y + this.CellSize);
					}
				}
				this.Populated = true;
				this.shownSub = this.currentSub;
			}
		}
	}

	
	public void AddTileButton(Tile tile)
	{
		TileButton button = UnityEngine.Object.Instantiate<GameObject>(this.tileButtonPrefab, this.content.transform).GetComponent<TileButton>();
		button.tile = tile;
		button.SetTile(tile, this.mapType);
		this.buttons[this.currentSubsection].Add(button);
		this.PositionButton(this.buttons[this.currentSubsection].Count - 1);
	}

	
	public void AddPaletteDivider(string name)
	{
		PaletteDividerController pal = UnityEngine.Object.Instantiate<GameObject>(this.paletteDividerPrefab, this.content.transform).GetComponent<PaletteDividerController>();
		pal.text.text = name;
		RectTransform rect = pal.GetComponent<RectTransform>();
		this.currentSubsection = name;
		this.buttons.Add(name, new List<TileButton>());
		rect.localPosition = new Vector2(7f, -7f + this.GetCellPosition(0).y + this.paletteSize);
	}

	
	public void PositionButton(int index)
	{
		TileButton button = this.buttons[this.currentSubsection][index];
		float width = button.GetComponent<RectTransform>().rect.width;
		bool flag = width > this.CellSize;
		if (flag)
		{
			Debug.LogError("Tile buttons are too big! Reduce size to be less than " + this.CellSize);
		}
		else
		{
			float offset = (this.CellSize - width) / 2f + this.margin;
			button.transform.localPosition = this.GetCellPosition(index) + new Vector2(offset, -offset);
		}
	}

	
	public Vector2 GetCellPosition(int index)
	{
		float startingY = -this.paletteSize;
		for (int i = 0; i < this.buttons.Count - 1; i++)
		{
			int b = this.buttons.Values.ElementAt(i).Count;
			startingY += (float)(-(float)((b + ((b % 6 == 0) ? 0 : this.columns)) / this.columns)) * this.CellSize;
			startingY -= this.paletteSize;
		}
		return new Vector2((float)(index % this.columns) * this.CellSize, (float)(-(float)(index / this.columns)) * this.CellSize + startingY);
	}

	
	
	public float CellSize
	{
		get
		{
			return (this.content.rect.width - this.m_scrollbar.GetComponent<RectTransform>().rect.width - this.margin * 2f) / (float)this.columns;
		}
	}

	
	public int columns = 6;

	
	public GameObject tileButtonPrefab;

	
	public GameObject paletteDividerPrefab;

	
	public RectTransform content;

	
	public Dictionary<string, List<TileButton>> buttons;

	
	public List<string> subTilesets = new List<string>();

	
	public string currentSub = "";

	
	public TilemapHandler.MapType mapType;

	
	private float margin = 5f;

	
	private string currentSubsection = "default";

	
	private float paletteSize = 55f;

	
	private string shownSub = "";

	
	private Scrollbar m_scrollbar;
}
