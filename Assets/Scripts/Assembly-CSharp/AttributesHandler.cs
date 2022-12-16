using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;


public class AttributesHandler : MonoBehaviour
{
	
	
	 
	public bool Populated { get; set; }

	
	public void Show()
	{
		this.Repopulate();
	}

	
	public void Hide()
	{
		this.ClearAllAttributes();
		this.Populated = false;
	}

	
	public void ClearAllAttributes()
	{
		foreach (AttributeItem att in this.attributes)
		{
			UnityEngine.Object.Destroy(att.gameObject);
		}
		this.attributes.Clear();
	}

	
	private void Awake()
	{
		this.content = base.transform.Find("Viewport").Find("Content").GetComponent<RectTransform>();
		this.attributes = new List<AttributeItem>();
		this.m_scrollbar = base.GetComponentInChildren<Scrollbar>();
	}

	
	private void Start()
	{
		this.Repopulate();
	}

	
	public void Populate()
	{
		if (!this.Populated)
		{
			bool flag = !InputHandler.Instance.selectedTile;
			if (!flag)
			{
				DataTile dataTile;
				bool flag2 = (dataTile = (InputHandler.Instance.selectedTile as DataTile)) == null;
				if (!flag2)
				{
					bool flag3 = this.nothingSelectObject;
					if (flag3)
					{
						UnityEngine.Object.Destroy(this.nothingSelectObject);
						this.nothingSelectObject = null;
					}
					this.propertiesText.text = new CultureInfo("en-US", false).TextInfo.ToTitleCase(dataTile.name.Replace('_', ' ')) + " Properties";
					this.ClearAllAttributes();
					foreach (KeyValuePair<string, JToken> att in dataTile.data)
					{
						this.AddTileAttribute(att.Key, att.Value);
					}
					bool flag4 = this.attributes.Count == 0;
					if (flag4)
					{
						RectTransform nothing = UnityEngine.Object.Instantiate<GameObject>(this.nothingSelectedPrefab, this.content.transform).GetComponent<RectTransform>();
						nothing.localPosition = new Vector2(11f, -this.totalHeight);
						nothing.GetComponent<Text>().text = "\nSelected object has no properties.";
						this.nothingSelectObject = nothing.gameObject;
					}
					this.content.sizeDelta = new Vector2(530f, this.totalHeight + this.margin);
					this.Populated = true;
				}
			}
		}
	}

	
	public void Repopulate()
	{
		this.Populated = false;
		this.ClearAllAttributes();
		this.propertiesText.text = "Properties";
		this.totalHeight = 60f;
		this.Populate();
		bool flag = !this.Populated;
		if (flag)
		{
			bool flag2 = this.nothingSelectObject;
			if (flag2)
			{
				UnityEngine.Object.Destroy(this.nothingSelectObject);
			}
			RectTransform nothing = UnityEngine.Object.Instantiate<GameObject>(this.nothingSelectedPrefab, this.content.transform).GetComponent<RectTransform>();
			nothing.localPosition = new Vector2(11f, -this.totalHeight);
			this.nothingSelectObject = nothing.gameObject;
		}
	}

	
	public void AddTileAttribute(string attributeName, JToken attribute)
	{
		GameObject prefab;
		switch (attribute.Type)
		{
		case JTokenType.Integer:
			prefab = this.intAttributePrefab;
			break;
		case JTokenType.Float:
			prefab = ((AttributeDatabase.allAttributes[AttributeDatabase.LongToShortName(attributeName)].possibleValues.Length == 2) ? this.floatBarAttributePrefab : this.floatAttributePrefab);
			break;
		case JTokenType.String:
			prefab = ((AttributeDatabase.allAttributes[AttributeDatabase.LongToShortName(attributeName)].possibleValues.Length != 0) ? this.dropdownStringAttributePrefab : this.stringAttributePrefab);
			break;
		case JTokenType.Boolean:
			prefab = this.boolAttributePrefab;
			break;
		default:
			return;
		}
		AttributeItem att = UnityEngine.Object.Instantiate<GameObject>(prefab, this.content.transform).GetComponent<AttributeItem>();
		att.propertyName = attributeName;
		att.text.text = attributeName.SplitCamelCase().UppercaseFirst();
		att.Value = attribute.ToObject<object>();
		this.attributes.Add(att);
		this.PositionAttribute(this.attributes.Count - 1);
	}

	
	public void PositionAttribute(int index)
	{
		AttributeItem att = this.attributes[index];
		RectTransform rect = att.GetComponent<RectTransform>();
		float height = rect.rect.height;
		rect.localPosition = new Vector2(11f, -this.totalHeight);
		this.totalHeight += height + this.margin;
	}

	
	public GameObject boolAttributePrefab;

	
	public GameObject stringAttributePrefab;

	
	public GameObject intAttributePrefab;

	
	public GameObject floatAttributePrefab;

	
	public GameObject floatBarAttributePrefab;

	
	public GameObject dropdownStringAttributePrefab;

	
	public GameObject nothingSelectedPrefab;

	
	public Text propertiesText;

	
	public RectTransform content;

	
	public List<AttributeItem> attributes;

	
	public float margin = 5f;

	
	private Scrollbar m_scrollbar;

	
	private float totalHeight = 60f;

	
	private GameObject nothingSelectObject = null;
}
