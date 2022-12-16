using System;
using UnityEngine;
using UnityEngine.UI;


public class PaletteDropdown : MonoBehaviour
{
	
	private void Awake()
	{
		PaletteDropdown.Instance = this;
		this.dropdown = base.GetComponent<Dropdown>();
		this.ActivePalette = this.palettes[0];
	}

	
	public void SetValue(TilemapHandler.MapType type)
	{
		for (int i = 0; i < this.dropdown.options.Count; i++)
		{
			bool flag = this.dropdown.options[i].text.Equals(type.ToString());
			if (flag)
			{
				this.dropdown.value = i;
				bool flag2 = this.dropdown.value == i;
				if (flag2)
				{
					this.OnValueChanged();
				}
				break;
			}
		}
	}

	
	
	 
	public TilePalette ActivePalette { get; private set; }

	
	public void OnValueChanged()
	{
		string val = this.dropdown.options[this.dropdown.value].text;
		TilemapHandler.MapType type;
		if (Enum.TryParse<TilemapHandler.MapType>(val, true, out type))
		{
			for (int i = 0; i < this.palettes.Length; i++)
			{
				if (this.palettes[i].mapType == type)
				{
					this.palettes[i].currentSub = "";
					this.palettes[i].Show();
					this.palettes[i].Populate();
					this.ActivePalette = this.palettes[i];
				}
				else
				{
					this.palettes[i].Hide();
				}
			}
		}
		else
		{
			for (int j = 0; j < this.palettes.Length; j++)
			{
				if (this.palettes[j].subTilesets.Contains(val))
				{
					this.palettes[j].currentSub = val;
					this.palettes[j].Show();
					this.palettes[j].Populate();
					this.ActivePalette = this.palettes[j];
				}
				else
				{
					this.palettes[j].Hide();
				}
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

	
	public TilePalette[] palettes;

	public TilePalette nodeModePallet;

	
	public static PaletteDropdown Instance;

	
	private Dropdown dropdown;
}
