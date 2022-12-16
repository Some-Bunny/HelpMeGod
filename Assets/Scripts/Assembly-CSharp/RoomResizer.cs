using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomResizer : MonoBehaviour
{

	private void Start()
	{
		this.m_hideable = base.GetComponent<HideableObject>();
		RoomResizer.Instance = this;
		base.gameObject.SetActive(false);		
	}

	public void ToggleOn()
	{
		dimX.text = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map.cellBounds.size.x.ToString();
		dimY.text = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map.cellBounds.size.y.ToString();
		this.m_hideable.Show();
	}

	public void OnCloseClicked()
	{
		this.dimTop.text = "0";
		this.dimBottom.text = "0";
		this.dimRight.text = "0";
		this.dimLeft.text = "0";

		this.m_hideable.Hide();
	}


	public void OnCreateClicked()
	{
		int top = int.Parse(this.dimTop.text);
		int bottom = int.Parse(this.dimBottom.text);
		int right = int.Parse(this.dimRight.text);
		int left = int.Parse(this.dimLeft.text);

		CanvasHandler.Instance.Resize(top, bottom, right, left);

		this.dimTop.text = "0";
		this.dimBottom.text = "0";
		this.dimRight.text = "0";
		this.dimLeft.text = "0";

		this.m_hideable.Hide();
	}

	public void OnValueChanged()
    {
		int x = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map.cellBounds.size.x;
		int y = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment).map.cellBounds.size.y;

		int top = int.Parse(this.dimTop.text);
		int bottom = int.Parse(this.dimBottom.text);
		int right = int.Parse(this.dimRight.text);
		int left = int.Parse(this.dimLeft.text);

		x += right + left;
		y += top + bottom;

		dimX.text = x.ToString();
		dimY.text = y.ToString();
	}


	public InputField dimX;
	public InputField dimY;

	public InputField dimTop;
	public InputField dimBottom;
	public InputField dimLeft;
	public InputField dimRight;

	public static RoomResizer Instance;


	private HideableObject m_hideable;
}
