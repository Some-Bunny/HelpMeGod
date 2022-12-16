using System;

namespace UnityEngine.Tilemaps
{
	
	[CreateAssetMenu(fileName = "New Weighted Random Tile", menuName = "Tiles/Weighted Random Tile")]
	[Serializable]
	public class WeightedRandomTile : Tile
	{
		
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			base.GetTileData(location, tileMap, ref tileData);
			bool flag = this.Sprites == null || this.Sprites.Length == 0;
			if (!flag)
			{
				Random.State oldState = Random.state;
				long hash = (long)location.x;
				hash = hash + (long)(-1412623820) + (hash << 15);
				hash = (hash + 159903659L ^ hash >> 11);
				hash ^= (long)location.y;
				hash = hash + 1185682173L + (hash << 7);
				hash = (hash + (long)(-1097387857) ^ hash << 11);
				Random.InitState((int)hash);
				int cumulativeWeight = 0;
				foreach (WeightedSprite spriteInfo in this.Sprites)
				{
					cumulativeWeight += spriteInfo.Weight;
				}
				int randomWeight = Random.Range(0, cumulativeWeight);
				foreach (WeightedSprite spriteInfo2 in this.Sprites)
				{
					randomWeight -= spriteInfo2.Weight;
					bool flag2 = randomWeight < 0;
					if (flag2)
					{
						tileData.sprite = spriteInfo2.Sprite;
						break;
					}
				}
				Random.state = oldState;
			}
		}

		
		[SerializeField]
		public WeightedSprite[] Sprites;
	}
}
