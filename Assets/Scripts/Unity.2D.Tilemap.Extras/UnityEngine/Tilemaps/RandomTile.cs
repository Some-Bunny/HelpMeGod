using System;

namespace UnityEngine.Tilemaps
{
	
	[CreateAssetMenu(fileName = "New Random Tile", menuName = "Tiles/Random Tile")]
	[Serializable]
	public class RandomTile : Tile
	{
		 
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			base.GetTileData(location, tileMap, ref tileData);
			bool flag = this.m_Sprites != null && this.m_Sprites.Length != 0;
			if (flag)
			{
				long hash = (long)location.x;
				hash = hash + (long)(-1412623820) + (hash << 15);
				hash = (hash + 159903659L ^ hash >> 11);
				hash ^= (long)location.y;
				hash = hash + 1185682173L + (hash << 7);
				hash = (hash + (long)(-1097387857) ^ hash << 11);
				Random.State oldState = Random.state;
				Random.InitState((int)hash);
				tileData.sprite = this.m_Sprites[(int)((float)this.m_Sprites.Length * Random.value)];
				Random.state = oldState;
			}
		}

		
		[SerializeField]
		public Sprite[] m_Sprites;
	}
}
