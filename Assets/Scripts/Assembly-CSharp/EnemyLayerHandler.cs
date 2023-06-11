using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EnemyLayerHandler : MonoBehaviour
{
	
	
	 
	public int EnemyMapIndex
	{
		get
		{
			return this.m_enemyLayer;
		}
		set
		{
			this.m_enemyLayer = Mathf.Clamp(value, 0, this.enemyMaps.Count - 1);
		}
	}

	
	public void MoveLayerUp()
	{
	}

	
	public void MoveLayerDown()
	{
	}

	
	public EnemyMap GetMap(int index)
	{
		return this.enemyMaps[index];
	}

	
	
	public int LayerCount
	{
		get
		{
			return this.enemyMaps.Count;
		}
	}

	
	public void CollectDataForExport(ref ImportExport.RoomData data)
	{
		int count = this.enemyMaps.Count;
		for (int i = 0; i < count; i++)
		{
			this.enemyMaps[i].CollectDataForExport(ref data, i, buttons[i].triggerCondition);
		}
	}

	public void CollectDataForExport(ref ImportExport.NewRoomData data)
	{
		int count = this.enemyMaps.Count;
		for (int i = 0; i < count; i++)
		{
			this.enemyMaps[i].CollectDataForExport(ref data, i, buttons[i].triggerCondition);
		}
	}


	private void Awake()
	{
		this.buttons = new List<EnemyLayerButton>();
		EnemyLayerHandler.Instance = this;
		this.enemyMaps = new List<EnemyMap>();
	}

	
	private void Start()
	{
		AddLayer(Enums.RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES);
		for (int i = 1; i < this.scheduledLayers; i++)
		{
			this.AddLayer();
		}
	}

	
	public TilemapHandler GetActiveTilemap()
	{
		bool flag = this.enemyMaps.Count == 0;
		TilemapHandler result;
		if (flag)
		{
			result = null;
		}
		else
		{
			result = this.enemyMaps[this.EnemyMapIndex];
		}
		return result;
	}

	
	public void SetSelectedLayer(EnemyLayerButton button)
	{
		int index = this.buttons.IndexOf(button);
		this.selectedButton = button;
		this.EnemyMapIndex = index;
		foreach (EnemyLayerButton b in this.buttons)
		{
			bool flag = b != button;
			if (flag)
			{
				b.Selected = false;
			}
			else
			{
				b.Selected = true;
			}
		}
		for (int i = 0; i < this.enemyMaps.Count; i++)
		{
			int dist = Math.Min(Math.Abs(index - i), 10);
			this.enemyMaps[i].GetComponent<Tilemap>().color = ((i == index) ? Color.white : new Color(1f, 1f, 1f, 0.5f - (float)dist / 3f));
		}
	}

	public void DoTransparency()
	{
        int index = m_enemyLayer;
        for (int i = 0; i < this.enemyMaps.Count; i++)
		{
            int dist = Math.Min(Math.Abs((index - i)-1), 10);
            this.enemyMaps[i].GetComponent<Tilemap>().color = ((i == index) ? new Color(1f, 1f, 1f, 0.5f - 2) : new Color(1f, 1f, 1f, 0.5f - (float)dist / 4f));
        }
    }

	
	public void OnClickAddLayer()
	{
		this.AddLayer();
		//PaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Enemies);
	}

	
	public EnemyMap AddLayer(Enums.RoomEventTriggerCondition triggerCondition = Enums.RoomEventTriggerCondition.ON_ENEMIES_CLEARED)
	{
		var button = Instantiate(layerButtonPrefab.gameObject, buttonContainer).GetComponent<EnemyLayerButton>();
		button.SetText("Enemy Wave " + buttons.Count);

		button.triggerDropdown.value = (int)triggerCondition;
		button.triggerCondition = triggerCondition;
		button.OnValueChanged();
		buttons.Add(button);
		RepositionButtons();

		var tilemap = Instantiate(enemyTilemapPrefab, enemyTilemapContainer).GetComponent<EnemyMap>();
		button.map = tilemap.GetComponent<EnemyMap>();
		enemyMaps.Add(tilemap);
		SetSelectedLayer(button);
		return tilemap;
	}

	
	public void RepositionButtons()
	{
		int count = this.buttons.Count;
		for (int i = 0; i < count; i++)
		{
			bool flag = this.buttons[i];
			if (flag)
			{
				this.buttons[i].transform.localPosition = new Vector2(0f, (float)(-(float)i) * this.ButtonHeight - (float)this.verticalMargin);
			}
		}
		this.buttonContainer.sizeDelta = new Vector2(this.buttonContainer.sizeDelta.x, -this.buttons[count - 1].transform.localPosition.y + this.ButtonHeight);
	}

	
	
	public float ButtonHeight
	{
		get
		{
			return this.layerButtonPrefab.GetComponent<RectTransform>().rect.height;
		}
	}

	
	public void RemoveLayer()
	{
		bool flag = !this.selectedButton;
		if (!flag)
		{
			int index = this.buttons.IndexOf(this.selectedButton);
			UnityEngine.Object.Destroy(this.enemyMaps[index].gameObject);
			this.enemyMaps.RemoveAt(index);
			UnityEngine.Object.DestroyImmediate(this.selectedButton.gameObject);
			this.buttons.RemoveAt(index);
			bool flag2 = this.enemyMaps.Count == 0;
			if (flag2)
			{
				PaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Environment);
			}
			else
			{
				this.EnemyMapIndex = Mathf.Clamp(index, 0, this.enemyMaps.Count - 1);
				this.SetSelectedLayer(this.buttons[this.EnemyMapIndex]);
				this.RepositionButtons();
			}
		}
	}

	public List<EnemyLayerButton> ReturnButtons()
	{
		return buttons;
	}

	
	public static EnemyLayerHandler Instance;

	
	public Transform enemyTilemapContainer;

	
	public RectTransform buttonContainer;

	
	public GameObject enemyTilemapPrefab;

	
	public EnemyLayerButton layerButtonPrefab;

	
	public List<EnemyMap> enemyMaps;

	
	private List<EnemyLayerButton> buttons;

	
	private EnemyLayerButton selectedButton;

	
	private int m_enemyLayer;

	
	public int verticalMargin = 5;

	
	public Color outOfFocusColor = new Color(1f, 1f, 1f, 0.5f);

	
	public int scheduledLayers = 0;
}
