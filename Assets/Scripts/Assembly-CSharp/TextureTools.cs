using System;
using UnityEngine;


public static class TextureTools
{
	 
	public static Texture2D CropWhiteSpace(this Texture2D orig)
	{
		Rect bounds = orig.GetTrimmedBounds();
		Texture2D result = new Texture2D((int)bounds.width, (int)bounds.height);
		result.name = orig.name;
		int x = (int)bounds.x;
		while ((float)x < bounds.x + bounds.width)
		{
			int y = (int)bounds.y;
			while ((float)y < bounds.y + bounds.height)
			{
				result.SetPixel(x - (int)bounds.x, y - (int)bounds.y, orig.GetPixel(x, y));
				y++;
			}
			x++;
		}
		result.filterMode = orig.filterMode;
		result.Apply(false, false);
		return result;
	}

	 
	public static Rect GetTrimmedBounds(this Texture2D t)
	{
		int xMin = t.width;
		int yMin = t.height;
		int xMax = 0;
		int yMax = 0;
		for (int x = 0; x < t.width; x++)
		{
			for (int y = 0; y < t.height; y++)
			{
				bool flag = t.GetPixel(x, y).a != 0f;
				if (flag)
				{
					bool flag2 = x < xMin;
					if (flag2)
					{
						xMin = x;
					}
					bool flag3 = y < yMin;
					if (flag3)
					{
						yMin = y;
					}
					bool flag4 = x > xMax;
					if (flag4)
					{
						xMax = x;
					}
					bool flag5 = y > yMax;
					if (flag5)
					{
						yMax = y;
					}
				}
			}
		}
		return new Rect((float)xMin, (float)yMin, (float)(xMax - xMin + 1), (float)(yMax - yMin + 1));
	}

	 
	public static Texture2D Square(this Texture2D texture)
	{
		int w = texture.width;
		int h = texture.height;
		int square = Mathf.Max(w, h);
		Texture2D result = new Texture2D(square, square, TextureFormat.RGBAFloat, false);
		int adjX = 0;
		int adjY = 0;
		bool flag = h == square;
		if (flag)
		{
			adjX = (square - w) / 2;
		}
		bool flag2 = w == square;
		if (flag2)
		{
			adjY = (square - h) / 2;
		}
		for (int i = adjX; i < square; i++)
		{
			for (int j = adjY; j < square; j++)
			{
				result.SetPixel(i, j, Color.clear);
			}
		}
		for (int k = 0; k < w; k++)
		{
			for (int l = 0; l < h; l++)
			{
				result.SetPixel(k + adjX, l + adjY, texture.GetPixel(k, l));
			}
		}
		result.filterMode = texture.filterMode;
		result.Apply(false, false);
		return result;
	}

	 
	public static Sprite ToSprite(this Texture2D texture)
	{
		return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), new Vector2(0f, 0f), 16f);
	}
}
