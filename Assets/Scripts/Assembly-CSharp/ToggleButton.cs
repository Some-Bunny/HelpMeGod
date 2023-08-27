using System;
using UnityEngine;


public class ToggleButton : MonoBehaviour
{
	
	
	 
	public bool Toggled
	{
		get
		{
			return this.m_toggled;
		}
		set
		{
			this.m_toggled = value;
			this.UpdateAppearance();
		}
	}

	
	private void Awake()
	{
		this.checkMark = base.transform.Find("Check mark");
	}

	
	public void OnClick()
	{
		this.Toggled = !this.Toggled;
	}

	
	public void UpdateAppearance()
	{
		bool flag = !this.checkMark;
		if (flag)
		{
			this.checkMark = base.transform.Find("Check mark");
		}
		this.checkMark.gameObject.SetActive(this.Toggled);
		if (OnToggle != null) { OnToggle(); }
	}

	public void ForceState(bool b)
	{
        bool flag = !this.checkMark;
        if (flag)
        {
            this.checkMark = base.transform.Find("Check mark");
        }
        this.checkMark.gameObject.SetActive(b);
    }


	public Action OnToggle;
	
	public bool m_toggled = false;

	
	public Transform checkMark;
}
