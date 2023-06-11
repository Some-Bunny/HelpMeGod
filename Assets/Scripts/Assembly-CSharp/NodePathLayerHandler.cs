using Assets.Scripts.Assembly_CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;


public class NodePathLayerHandler : MonoBehaviour
{	 
	public int EnemyMapIndex
	{
		get
		{
			return this.m_nodeLayer;
		}
		set
		{
			this.m_nodeLayer = Mathf.Clamp(value, 0, this.nodeMaps.Count - 1);
		}
	}

	
	public void MoveLayerUp()
	{
	}

	
	public void MoveLayerDown()
	{
	}

	
	public NodeMap GetMap(int index)
	{
		return this.nodeMaps[index];
	}
	
	
	public int LayerCount
	{
		get
		{
			return this.nodeMaps.Count;
		}
	}

	public void CollectDataForExport(ref ImportExport.NewRoomData data)
	{
		int count = this.nodeMaps.Count;
		for (int i = 0; i < count; i++)
		{
			this.nodeMaps[i].CollectDataForExport2(ref data, i, buttons[i].WrapMode);
		}
	}

	public void CollectDataForExport(ref ImportExport.RoomData data)
	{
		int count = this.nodeMaps.Count;
		for (int i = 0; i < count; i++)
		{
			this.nodeMaps[i].CollectDataForExport2(ref data, i, buttons[i].WrapMode);
		}
	}

	
	private void Awake()
	{
		this.buttons = new List<NodeLayerButton>();
		NodePathLayerHandler.Instance = this;
		this.nodeMaps = new List<NodeMap>();
	}

	
	private void Start()
	{
		AddLayer(Enums.SerializedPathWrapMode.LOOP);
		for (int i = 1; i < this.scheduledLayers; i++)
		{
			this.AddLayer();
		}
		this.gameObject.transform.parent.gameObject.SetActive(false);
	}

	
	public TilemapHandler GetActiveTilemap()
	{
		TilemapHandler result;
		if (this.nodeMaps.Count == 0)
		{
			result = null;
		}
		else
		{
			result = this.nodeMaps[this.EnemyMapIndex];
		}
		return result;
	}

	
	public void SetSelectedLayer(NodeLayerButton button)
	{
		int index = this.buttons.IndexOf(button);
		this.selectedButton = button;
		this.EnemyMapIndex = index;
		foreach (NodeLayerButton b in this.buttons)
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
		for (int i = 0; i < this.nodeMaps.Count; i++)
		{
			int dist = Math.Min(Math.Abs(index - i), 10);
			this.nodeMaps[i].GetComponent<Tilemap>().color = ((i == index) ? Color.white : new Color(1f, 1f, 1f, 0.5f - (float)dist / 3f));
        }


       

        /*
        for (int i = 0; i < GetMap(m_nodeLayer).fuckYou.Count; i++)
        {



            Debug.LogError("Node "+i);
            var node = GetMap(m_nodeLayer).fuckYou[i];
            Debug.LogError("A");

            int node2 = (i + 1) == GetMap(m_nodeLayer).fuckYou.Count ? 0 : (i + 1);//  GetMap(m_nodeLayer).fuckYou[i];
            Debug.LogError(node2);
            Debug.LogError(GetMap(m_nodeLayer).fuckYou.Count);

            Vector3 fuck = new Vector3(GetMap(m_nodeLayer).fuckYou[node2].position.x, GetMap(m_nodeLayer).fuckYou[node2].position.y, 100);
            Debug.LogError("c");
            renderer.SetPosition(1, fuck);

            Debug.LogError("d");


        }
        */


        List<object> paths2 = new List<object>();
        for (int i = 0; i < GetMap(m_nodeLayer).fuckYou.Count; i++) paths2.Add(i.ToString());
        if (paths2.Count > 0)
		{
            AttributeDatabase.allAttributes["nodPos"].possibleValues = paths2.ToArray();
        }
    }

	
	public void OnClickAddLayer()
	{
		this.AddLayer();
		//PaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Enemies);
	}

	
	public NodeMap AddLayer(Enums.SerializedPathWrapMode triggerCondition = Enums.SerializedPathWrapMode.LOOP)
	{
		var button = Instantiate(layerButtonPrefab.gameObject, buttonContainer).GetComponent<NodeLayerButton>();
		button.SetText("Node Path " + buttons.Count);

		button.triggerDropdown.value = (int)triggerCondition;
		button.WrapMode = triggerCondition;
		button.OnValueChanged();
		buttons.Add(button);
		RepositionButtons();

		var tilemap = Instantiate(nodeTilemapPrefab, nodeTilemapContainer).GetComponent<NodeMap>();
		button.map = tilemap.GetComponent<NodeMap>();
		nodeMaps.Add(tilemap);
		SetSelectedLayer(button);
		OnLayerChanged();
		return tilemap;
	}

	
	public List<NodeLayerButton> ReturnButtons()
	{
		return buttons;
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
			UnityEngine.Object.Destroy(this.nodeMaps[index].gameObject);
			this.nodeMaps.RemoveAt(index);
			UnityEngine.Object.DestroyImmediate(this.selectedButton.gameObject);
			this.buttons.RemoveAt(index);
			bool flag2 = this.nodeMaps.Count == 0;
			if (flag2)
			{
				PaletteDropdown.Instance.SetValue(TilemapHandler.MapType.Environment);
			}
			else
			{
				this.EnemyMapIndex = Mathf.Clamp(index, 0, this.nodeMaps.Count - 1);
				this.SetSelectedLayer(this.buttons[this.EnemyMapIndex]);
				this.RepositionButtons();
			}
			OnLayerChanged();
		}
	}

	public void OnLayerChanged()
    {
		List<object> paths = new List<object>();
        List<object> paths2 = new List<object>();

        for (int i = 0; i < buttons.Count; i++) paths.Add(i.ToString());
        for (int i = 0; i < GetMap(m_nodeLayer).fuckYou.Count; i++) paths2.Add(i.ToString());

        //Debug.DrawLine()
        AttributeDatabase.allAttributes["tSP"].possibleValues = paths.ToArray();
		if (paths2.Count > 0)
		{
            AttributeDatabase.allAttributes["nodPos"].possibleValues = paths2.ToArray();
        }


    }


    public static NodePathLayerHandler Instance;

	
	public Transform nodeTilemapContainer;

	
	public RectTransform buttonContainer;

	
	public GameObject nodeTilemapPrefab;

	
	public NodeLayerButton layerButtonPrefab;

	
	public List<NodeMap> nodeMaps;

	
	private List<NodeLayerButton> buttons;

	
	private NodeLayerButton selectedButton;

	
	private int m_nodeLayer;

	
	public int verticalMargin = 5;

	
	public Color outOfFocusColor = new Color(1f, 1f, 1f, 0.5f);

	
	public int scheduledLayers = 0;

}
