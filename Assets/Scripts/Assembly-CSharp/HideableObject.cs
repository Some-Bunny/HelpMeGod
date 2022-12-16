using System;
using UnityEngine;


[RequireComponent(typeof(MouseListener))]
public class HideableObject : MonoBehaviour
{
	
	public void Hide()
	{
		if (this.disableShortcutsOnShow)
		{
			InputHandler.Instance.shortcutsDisabled = false;
		}
		base.gameObject.SetActive(false);
		if (OnHide != null)
		{
			OnHide();
		}
	}

	
	public void Show()
	{
		InputHandler.Instance.shortcutsDisabled = this.disableShortcutsOnShow;
		this.showTime = Time.time;
		base.gameObject.SetActive(true);
		if (OnShow != null)
		{
			OnShow();
		}
	}

	
	public bool hideOnClickElsewhere = true;

	
	public bool disableShortcutsOnShow = false;

	
	public float showTime;

	
	public Action OnShow;

	
	public Action OnHide;
}
