using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public abstract class AttributeItem : MonoBehaviour
{
	
	
	
	public abstract object Value { get; set; }

	
	public void ValueChanged()
	{
		Tile tile = InputHandler.Instance.selectedTile;
		DataTile dataTile;
		if (tile && (dataTile = (tile as DataTile)) != null)
		{
			dataTile.data[this.propertyName] = JToken.FromObject(this.Value);
		}
	}

	
	public Text text;

	
	public string propertyName;
}
