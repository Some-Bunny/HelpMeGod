using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodePaletteDropdown : MonoBehaviour
{
	private void Awake()
	{
		NodePaletteDropdown.Instance = this;
		this.dropdown = base.GetComponent<Dropdown>();
		this.ActivePalette = this.palette;
		gameObject.transform.parent.gameObject.SetActive(false);
	}


	public void SetValue(TilemapHandler.MapType type)
	{
		OnValueChanged();
	}




	public TilePalette ActivePalette { get; private set; }


	public void OnValueChanged()
	{
		string val = this.dropdown.options[this.dropdown.value].text;
		TilemapHandler.MapType type;
		if (Enum.TryParse<TilemapHandler.MapType>(val, true, out type))
		{
			Debug.LogError("Node Map Active");
			if (this.palette.mapType == type)
			{
				this.palette.currentSub = "";
				this.palette.Show();
				this.palette.Populate();

				this.ActivePalette = this.palette;
			}
			else
			{
                Debug.LogError("Node Map Inactive");
                this.palette.Hide();
			}
		}
	}

	public void ToggleNodeMode()
	{
		if (InputHandler.Instance.nodeMode)
		{
			SetValue(TilemapHandler.MapType.Nodes);
		}
		else
		{
			SetValue(TilemapHandler.MapType.Environment);
		}
	}


	public TilePalette palette;

	public static NodePaletteDropdown Instance;

	private Dropdown dropdown;
}
