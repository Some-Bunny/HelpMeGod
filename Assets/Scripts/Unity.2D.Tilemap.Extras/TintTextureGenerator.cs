using System;
using UnityEngine;
using UnityEngine.Tilemaps;


[ExecuteInEditMode]
public class TintTextureGenerator : MonoBehaviour
{
	 
	public void Start()
	{
		this.Refresh(base.GetComponent<Grid>());
	}

	
	
	private Texture2D tintTexture
	{
		get
		{
			bool flag = this.m_TintTexture == null;
			if (flag)
			{
				this.m_TintTexture = new Texture2D(this.k_TintMapSize, this.k_TintMapSize, TextureFormat.ARGB32, false);
				this.m_TintTexture.hideFlags = HideFlags.HideAndDontSave;
				this.m_TintTexture.wrapMode = TextureWrapMode.Clamp;
				this.m_TintTexture.filterMode = FilterMode.Bilinear;
				this.RefreshGlobalShaderValues();
			}
			return this.m_TintTexture;
		}
	}

	 
	public void Refresh(Grid grid)
	{
		bool flag = grid == null;
		if (!flag)
		{
			int w = this.tintTexture.width;
			int h = this.tintTexture.height;
			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					Vector3Int world = this.TextureToWorld(new Vector3Int(x, y, 0));
					this.tintTexture.SetPixel(x, y, this.GetGridInformation(grid).GetPositionProperty(world, "Tint", Color.white));
				}
			}
			this.tintTexture.Apply();
		}
	}

	 
	public void Refresh(Grid grid, Vector3Int position)
	{
		bool flag = grid == null;
		if (!flag)
		{
			this.RefreshGlobalShaderValues();
			Vector3Int texPosition = this.WorldToTexture(position);
			this.tintTexture.SetPixel(texPosition.x, texPosition.y, this.GetGridInformation(grid).GetPositionProperty(position, "Tint", Color.white));
			this.tintTexture.Apply();
		}
	}

	 
	public Color GetColor(Grid grid, Vector3Int position)
	{
		bool flag = grid == null;
		Color result;
		if (flag)
		{
			result = Color.white;
		}
		else
		{
			result = this.GetGridInformation(grid).GetPositionProperty(position, "Tint", Color.white);
		}
		return result;
	}

	 
	public void SetColor(Grid grid, Vector3Int position, Color color)
	{
		bool flag = grid == null;
		if (!flag)
		{
			this.GetGridInformation(grid).SetPositionProperty(position, "Tint", color);
			this.Refresh(grid, position);
		}
	}

	 
	private Vector3Int WorldToTexture(Vector3Int world)
	{
		return new Vector3Int(world.x + this.tintTexture.width / 2, world.y + this.tintTexture.height / 2, 0);
	}

	 
	private Vector3Int TextureToWorld(Vector3Int texpos)
	{
		return new Vector3Int(texpos.x - this.tintTexture.width / 2, texpos.y - this.tintTexture.height / 2, 0);
	}

	 
	private GridInformation GetGridInformation(Grid grid)
	{
		GridInformation gridInformation = grid.GetComponent<GridInformation>();
		bool flag = gridInformation == null;
		if (flag)
		{
			gridInformation = grid.gameObject.AddComponent<GridInformation>();
		}
		return gridInformation;
	}

	 
	private void RefreshGlobalShaderValues()
	{
		Shader.SetGlobalTexture("_TintMap", this.m_TintTexture);
		Shader.SetGlobalFloat("_TintMapSize", (float)this.k_TintMapSize);
	}

	
	public int k_TintMapSize = 256;

	
	private Texture2D m_TintTexture;
}
