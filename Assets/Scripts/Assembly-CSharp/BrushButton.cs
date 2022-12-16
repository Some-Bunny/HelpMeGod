using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BrushButton : MonoBehaviour
{
	
	public void Awake()
	{
		base.GetComponent<Image>().material = new Material(base.GetComponent<Image>().material);
		this.currentMode = 0;
	}

	
	public void OnClick()
	{
		if (InputHandler.Instance.BrushType == this.type)
		{
			this.currentMode = (this.currentMode + 1) % this.sprites.Count;
		}
		InputHandler.Instance.brushMode = this.currentMode;
		InputHandler.Instance.BrushType = this.type;
		BrushButton.UpdateAppearances();
	}

	
	public static void UpdateAppearances()
	{
		foreach (BrushButton button in UnityEngine.Object.FindObjectsOfType<BrushButton>())
		{
			button.GetComponent<Image>().overrideSprite = button.sprites[button.currentMode];
			button.GetComponent<Image>().material.SetFloat("_InvertColors", (float)((button.type == InputHandler.Instance.BrushType) ? 1 : 0));
			if (button.type == BrushButton.BrushType.SELECT && button.type != InputHandler.Instance.BrushType)
			{
				InputHandler.Instance.DeselectTile();
			}
		}
	}

	
	public BrushButton.BrushType type;

	
	public List<Sprite> sprites = new List<Sprite>(1);

	
	private int currentMode = 0;

	
	public enum BrushType
	{
		
		PENCIL,
		
		BRUSH,
		
		ERASER,
		
		BUCKET,
		
		LINE,
		
		RECTANGLE,
		
		ELLIPSE,
		
		SELECT
	}
}
