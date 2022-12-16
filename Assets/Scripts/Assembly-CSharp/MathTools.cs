using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


public static class MathTools
{
	 
	public static IEnumerable<Vector3Int> GetPointsOnLine(Vector3Int start, Vector3Int end)
	{
		int x0 = start.x;
		int y0 = start.y;
		int x = end.x;
		int y = end.y;
		bool steep = Math.Abs(y - y0) > Math.Abs(x - x0);
		bool flag = steep;
		if (flag)
		{
			int t = x0;
			x0 = y0;
			y0 = t;
			t = x;
			x = y;
			y = t;
		}
		bool flag2 = x0 > x;
		if (flag2)
		{
			int t2 = x0;
			x0 = x;
			x = t2;
			t2 = y0;
			y0 = y;
			y = t2;
		}
		int dx = x - x0;
		int dy = Math.Abs(y - y0);
		int error = dx / 2;
		int ystep = (y0 < y) ? 1 : -1;
		int y2 = y0;
		int num;
		for (int x2 = x0; x2 <= x; x2 = num + 1)
		{
			yield return new Vector3Int(steep ? y2 : x2, steep ? x2 : y2, 0);
			error -= dy;
			bool flag3 = error < 0;
			if (flag3)
			{
				y2 += ystep;
				error += dx;
			}
			num = x2;
		}
		yield break;
	}

	 
	public static string SplitCamelCase(this string inputCamelCaseString)
	{
		string sTemp = Regex.Replace(inputCamelCaseString, "([A-Z][a-z])", " $1", RegexOptions.Compiled).Trim();
		return Regex.Replace(sTemp, "([A-Z][A-Z])", " $1", RegexOptions.Compiled).Trim();
	}

	 
	public static string UppercaseFirst(this string s)
	{
		bool flag = string.IsNullOrEmpty(s);
		string result;
		if (flag)
		{
			result = string.Empty;
		}
		else
		{
			result = char.ToUpper(s[0]).ToString() + s.Substring(1);
		}
		return result;
	}
}
