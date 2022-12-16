using System;
using UnityEngine;
using UnityEngine.UI;


public class NewRoomMenu : MonoBehaviour
{
	
	private void Start()
	{
		this.m_hideable = base.GetComponent<HideableObject>();
		NewRoomMenu.Instance = this;
		base.gameObject.SetActive(false);
	}

	
	public void OnCloseClicked()
	{
		this.m_hideable.Hide();
	}

	
	public void OnCreateClicked()
	{
		int x = int.Parse(this.dimX.text);
		int y = int.Parse(this.dimY.text);
		bool flag = x < 2 || y < 2;
		if (flag)
		{
			Debug.LogError("Dimensions must be greater than 0!");
		}
		else
		{
			Manager.FilePath = null;
			Manager.drawBorder = this.borderButton.Toggled;
			Manager.roomSize = new Vector2Int(x, y);
			Manager.Reload();
		}
	}

	
	public InputField dimX;

	
	public InputField dimY;

	
	public ToggleButton borderButton;

	
	public static NewRoomMenu Instance;

	
	private HideableObject m_hideable;
}
