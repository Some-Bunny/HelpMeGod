using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


public class TileButton : MonoBehaviour
{
	
	private void Awake()
	{
		this.image = base.GetComponent<Image>();
		this.text = base.GetComponentInChildren<Text>();
		this.selectedOutline = base.transform.Find("Selected Outline").gameObject;
	}

	
	public void SetTile(Tile tile, TilemapHandler.MapType mapType)
	{
		this.tile = tile;
		this.mapType = mapType;
		this.UpdateAppearance();
	}

	
	public void UpdateAppearance()
	{
		bool flag = !this.tile;
		if (flag)
		{
			Debug.LogError("Attempting to initialize tile button with no assigned tile!");
		}
		else
		{
			bool flag2 = this.tile.sprite != Manager.Instance.missingImageTileSprite;
			if (flag2)
			{
				Texture2D squaredTexture = this.tile.sprite.texture.Square();
				this.image.sprite = squaredTexture.ToSprite();
				this.text.enabled = false;
			}
			else
			{
				this.text.text = this.tile.name.Replace("_", " ");
				this.image.enabled = false;
			}
		}
	}

	
	private void FixedUpdate()
	{
		InputHandler instance = InputHandler.Instance;
		bool flag = ((instance != null) ? instance.selectedTileType : null) != this.tile;
		if (flag)
		{
			this.selectedOutline.gameObject.SetActive(false);
		}
	}

	
	public void OnClick()
	{
		this.selectedOutline.SetActive(true);
		InputHandler.Instance.SetSelectedTile(this.tile, this.mapType);
	}

	
	public Tile tile;

	
	public Text text;

	
	public Image image;

	
	private GameObject selectedOutline;

	
	private TilemapHandler.MapType mapType;
}
