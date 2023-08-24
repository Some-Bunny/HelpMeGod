using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BrushSizeButton : MonoBehaviour
{
	
	public void Awake()
	{
		base.GetComponent<Image>().material = new Material(base.GetComponent<Image>().material);
	}


	public List<TilemapHandler.MapType> disabledOnMapType = new List<TilemapHandler.MapType>()
	{

	};

	public void OnClick()
	{
		if (disabledOnMapType.Contains(InputHandler.Instance.activeTilemap)) { return; }
		if (type == ChangeType.ADD) { if (InputHandler.Instance.brushSize < 10) { InputHandler.Instance.brushSize++; } }
		else if (type == ChangeType.SUBTRACT) { if (InputHandler.Instance.brushSize > 1) { InputHandler.Instance.brushSize--; } }
		textBox.text = "Brush Size: " + InputHandler.Instance.brushSize.ToString();
        BrushSizeButton.UpdateAppearances();
	}
	public static void UpdateAppearances()
	{
		
		foreach (BrushSizeButton button in UnityEngine.Object.FindObjectsOfType<BrushSizeButton>())
		{
            //button.GetComponent<Image>().overrideSprite = button.sprites[button.currentMode];
            //button.GetComponent<Image>().material.SetFloat("_InvertColors", (float)((button.type == InputHandler.Instance.BrushType) ? 1 : 0));
            if (button.disabledOnMapType.Contains(InputHandler.Instance.activeTilemap))
            {
                button.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);

            }
            else
            {
                if (InputHandler.Instance.brushSize == (button.type == ChangeType.SUBTRACT ? 1 : 10))
                {
                    button.GetComponent<Image>().color = new Color(1, 1, 1, 0.1f);
                }
                else
                {
                    button.GetComponent<Image>().color = new Color(1, 1, 1, 1f);
                }
            }         
        }
    }
	[SerializeField]
	public Text textBox;

	public BrushSizeButton.ChangeType type;
	public enum ChangeType
	{		
		ADD,
		SUBTRACT
	}
}
