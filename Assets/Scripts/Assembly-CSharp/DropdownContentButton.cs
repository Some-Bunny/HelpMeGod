using System;
using UnityEngine;


public class DropdownContentButton : MonoBehaviour
{
	
	public virtual void OnClick()
	{
		bool flag = this.hideParentOnClick;
		if (flag)
		{
			base.transform.parent.gameObject.SetActive(false);
		}
	}

	
	public bool hideParentOnClick = true;
}
