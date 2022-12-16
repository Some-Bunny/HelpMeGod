using System;
using UnityEngine;
using UnityEngine.UI;


public class ToolPropertiesMenu : MonoBehaviour
{
	
	private void Start()
	{
		this.m_hideable = base.GetComponent<HideableObject>();
		ToolPropertiesMenu.Instance = this;
		base.gameObject.SetActive(false);
	}

	
	public void OnCloseClicked()
	{
		this.m_hideable.Hide();
	}

	
	public void UpdateSizeSlider()
	{
		bool flag = this.sizeField.text != ((int)this.sizeSlider.value).ToString();
		if (flag)
		{
			this.sizeField.text = ((int)this.sizeSlider.value).ToString();
		}
		InputHandler.Instance.brushSize = (int)this.sizeSlider.value;
	}

	
	public void UpdateSizeField()
	{
		bool flag = string.IsNullOrWhiteSpace(this.sizeField.text);
		if (flag)
		{
			this.sizeField.text = ((int)this.sizeSlider.value).ToString();
		}
		else
		{
			int val;
			bool flag2 = int.TryParse(this.sizeField.text, out val);
			if (flag2)
			{
				val = Mathf.Clamp(val, 1, 50);
				this.sizeField.text = val.ToString();
				bool flag3 = (int)this.sizeSlider.value != val;
				if (flag3)
				{
					this.sizeSlider.value = (float)val;
				}
			}
		}
	}

	
	public InputField sizeField;

	
	public Slider sizeSlider;

	
	public static ToolPropertiesMenu Instance;

	
	private HideableObject m_hideable;
}
