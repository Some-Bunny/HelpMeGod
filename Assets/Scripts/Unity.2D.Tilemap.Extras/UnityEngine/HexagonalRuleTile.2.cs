using System;

namespace UnityEngine
{
	
	[CreateAssetMenu(fileName = "New Hexagonal Rule Tile", menuName = "Tiles/Hexagonal Rule Tile")]
	[Serializable]
	public class HexagonalRuleTile : RuleTile
	{
		

		public override int m_RotationAngle
		{
			get
			{
				return 60;
			}
		}

		 
		public static Vector3 TilemapPositionToWorldPosition(Vector3Int tilemapPosition)
		{
			Vector3 worldPosition = new Vector3((float)tilemapPosition.x, (float)tilemapPosition.y);
			bool flag = tilemapPosition.y % 2 != 0;
			if (flag)
			{
				worldPosition.x += 0.5f;
			}
			worldPosition.y *= HexagonalRuleTile.m_TilemapToWorldYScale;
			return worldPosition;
		}

		 
		public static Vector3Int WorldPositionToTilemapPosition(Vector3 worldPosition)
		{
			worldPosition.y /= HexagonalRuleTile.m_TilemapToWorldYScale;
			Vector3Int tilemapPosition = default(Vector3Int);
			tilemapPosition.y = Mathf.RoundToInt(worldPosition.y);
			bool flag = tilemapPosition.y % 2 != 0;
			if (flag)
			{
				tilemapPosition.x = Mathf.RoundToInt(worldPosition.x - 0.5f);
			}
			else
			{
				tilemapPosition.x = Mathf.RoundToInt(worldPosition.x);
			}
			return tilemapPosition;
		}

		 
		public override Vector3Int GetOffsetPosition(Vector3Int location, Vector3Int offset)
		{
			Vector3Int position = location + offset;
			bool flag = offset.y % 2 != 0 && location.y % 2 != 0;
			if (flag)
			{
				position.x++;
			}
			return position;
		}

		 
		public override Vector3Int GetOffsetPositionReverse(Vector3Int position, Vector3Int offset)
		{
			Vector3Int location = position - offset;
			bool flag = offset.y % 2 != 0 && location.y % 2 != 0;
			if (flag)
			{
				location.x--;
			}
			return location;
		}

		 
		public override Vector3Int GetRotatedPosition(Vector3Int position, int rotation)
		{
			bool flag = rotation != 0;
			if (flag)
			{
				Vector3 worldPosition = HexagonalRuleTile.TilemapPositionToWorldPosition(position);
				int index = rotation / 60;
				bool flatTop = this.m_FlatTop;
				if (flatTop)
				{
					worldPosition = new Vector3(worldPosition.x * HexagonalRuleTile.m_CosAngleArr2[index] - worldPosition.y * HexagonalRuleTile.m_SinAngleArr2[index], worldPosition.x * HexagonalRuleTile.m_SinAngleArr2[index] + worldPosition.y * HexagonalRuleTile.m_CosAngleArr2[index]);
				}
				else
				{
					worldPosition = new Vector3(worldPosition.x * HexagonalRuleTile.m_CosAngleArr1[index] - worldPosition.y * HexagonalRuleTile.m_SinAngleArr1[index], worldPosition.x * HexagonalRuleTile.m_SinAngleArr1[index] + worldPosition.y * HexagonalRuleTile.m_CosAngleArr1[index]);
				}
				position = HexagonalRuleTile.WorldPositionToTilemapPosition(worldPosition);
			}
			return position;
		}

		 
		public override Vector3Int GetMirroredPosition(Vector3Int position, bool mirrorX, bool mirrorY)
		{
			bool flag = mirrorX || mirrorY;
			if (flag)
			{
				Vector3 worldPosition = HexagonalRuleTile.TilemapPositionToWorldPosition(position);
				bool flatTop = this.m_FlatTop;
				if (flatTop)
				{
					if (mirrorX)
					{
						worldPosition.y *= -1f;
					}
					if (mirrorY)
					{
						worldPosition.x *= -1f;
					}
				}
				else
				{
					if (mirrorX)
					{
						worldPosition.x *= -1f;
					}
					if (mirrorY)
					{
						worldPosition.y *= -1f;
					}
				}
				position = HexagonalRuleTile.WorldPositionToTilemapPosition(worldPosition);
			}
			return position;
		}

		
		private static float[] m_CosAngleArr1 = new float[]
		{
			Mathf.Cos(0f),
			Mathf.Cos(-1.0471976f),
			Mathf.Cos(-2.0943952f),
			Mathf.Cos(-3.1415927f),
			Mathf.Cos(-4.1887903f),
			Mathf.Cos(-5.2359877f)
		};

		
		private static float[] m_SinAngleArr1 = new float[]
		{
			Mathf.Sin(0f),
			Mathf.Sin(-1.0471976f),
			Mathf.Sin(-2.0943952f),
			Mathf.Sin(-3.1415927f),
			Mathf.Sin(-4.1887903f),
			Mathf.Sin(-5.2359877f)
		};

		
		private static float[] m_CosAngleArr2 = new float[]
		{
			Mathf.Cos(0f),
			Mathf.Cos(1.0471976f),
			Mathf.Cos(2.0943952f),
			Mathf.Cos(3.1415927f),
			Mathf.Cos(4.1887903f),
			Mathf.Cos(5.2359877f)
		};

		
		private static float[] m_SinAngleArr2 = new float[]
		{
			Mathf.Sin(0f),
			Mathf.Sin(1.0471976f),
			Mathf.Sin(2.0943952f),
			Mathf.Sin(3.1415927f),
			Mathf.Sin(4.1887903f),
			Mathf.Sin(5.2359877f)
		};

		
		[RuleTile.DontOverride]
		public bool m_FlatTop;

		
		private static float m_TilemapToWorldYScale = Mathf.Pow(1f - Mathf.Pow(0.5f, 2f), 0.5f);
	}
}
