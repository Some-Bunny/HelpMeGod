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


	public List<TilemapHandler.MapType> disabledOnMapType = new List<TilemapHandler.MapType>()
	{

	};

	public void OnClick()
	{
		if (disabledOnMapType.Contains(InputHandler.Instance.activeTilemap)) { return; }
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
            if (button.disabledOnMapType.Contains(InputHandler.Instance.activeTilemap))
            {
				button.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);

            }
			else 
			{
                button.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
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
		
		SELECT,

		COPY
	}

	

}
