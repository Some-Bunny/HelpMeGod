using System;
using UnityEngine;
using UnityEngine.EventSystems;


public class MouseListener : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public bool Hovered { get; set; }

	 
	public void OnPointerEnter(PointerEventData pointerEventData)
	{
		this.Hovered = true;
		if (OnEntered != null) { OnEntered(); }
	}
	public Action OnEntered;
    public Action OnExited;

    public void OnPointerExit(PointerEventData pointerEventData)
	{
		this.Hovered = false;
        if (OnExited != null) { OnExited(); }
    }
}
