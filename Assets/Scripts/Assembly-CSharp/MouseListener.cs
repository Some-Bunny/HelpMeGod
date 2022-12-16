using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseListener : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	
	
	 
	public bool Hovered { get; set; }

	 
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		this.Hovered = true;
	}

	 
	public void OnPointerExit(PointerEventData pointerEventData)
	{
		this.Hovered = false;
	}
}
