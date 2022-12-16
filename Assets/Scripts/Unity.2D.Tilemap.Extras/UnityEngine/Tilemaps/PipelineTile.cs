using System;

namespace UnityEngine.Tilemaps
{
	
	[CreateAssetMenu(fileName = "New Pipeline Tile", menuName = "Tiles/Pipeline Tile")]
	[Serializable]
	public class PipelineTile : TileBase
	{
		 
		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			for (int yd = -1; yd <= 1; yd++)
			{
				for (int xd = -1; xd <= 1; xd++)
				{
					Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);
					bool flag = this.TileValue(tileMap, position);
					if (flag)
					{
						tileMap.RefreshTile(position);
					}
				}
			}
		}

		 
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			this.UpdateTile(location, tileMap, ref tileData);
		}

		 
		private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;
			int mask = this.TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
			mask += (this.TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 2 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 4 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 8 : 0);
			int index = this.GetIndex((byte)mask);
			bool flag = index >= 0 && index < this.m_Sprites.Length && this.TileValue(tileMap, location);
			if (flag)
			{
				tileData.sprite = this.m_Sprites[index];
				tileData.transform = this.GetTransform((byte)mask);
				tileData.flags = TileFlags.LockAll;
				tileData.colliderType = Tile.ColliderType.Sprite;
			}
		}

		 
		private bool TileValue(ITilemap tileMap, Vector3Int position)
		{
			TileBase tile = tileMap.GetTile(position);
			return tile != null && tile == this;
		}

		 
		private int GetIndex(byte mask)
		{
			int result;
			switch (mask)
			{
			case 0:
				result = 0;
				break;
			case 1:
			case 2:
			case 4:
			case 5:
			case 8:
			case 10:
				result = 2;
				break;
			case 3:
			case 6:
			case 9:
			case 12:
				result = 1;
				break;
			case 7:
			case 11:
			case 13:
			case 14:
				result = 3;
				break;
			case 15:
				result = 4;
				break;
			default:
				result = -1;
				break;
			}
			return result;
		}

		 
		private Matrix4x4 GetTransform(byte mask)
		{
			switch (mask)
			{
			case 2:
			case 7:
			case 8:
			case 9:
			case 10:
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);
			case 3:
			case 14:
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);
			case 6:
			case 13:
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -270f), Vector3.one);
			}
			return Matrix4x4.identity;
		}

		
		[SerializeField]
		public Sprite[] m_Sprites;
	}
}
