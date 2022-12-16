using System;

namespace UnityEngine.Tilemaps
{
	
	[CreateAssetMenu(fileName = "New Animated Tile", menuName = "Tiles/Animated Tile")]
	[Serializable]
	public class AnimatedTile : TileBase
	{
		 
		public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
		{
			tileData.transform = Matrix4x4.identity;
			tileData.color = Color.white;
			bool flag = this.m_AnimatedSprites != null && this.m_AnimatedSprites.Length != 0;
			if (flag)
			{
				tileData.sprite = this.m_AnimatedSprites[this.m_AnimatedSprites.Length - 1];
				tileData.colliderType = this.m_TileColliderType;
			}
		}

		 
		public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
		{
			bool flag = this.m_AnimatedSprites.Length != 0;
			bool result;
			if (flag)
			{
				tileAnimationData.animatedSprites = this.m_AnimatedSprites;
				tileAnimationData.animationSpeed = Random.Range(this.m_MinSpeed, this.m_MaxSpeed);
				tileAnimationData.animationStartTime = this.m_AnimationStartTime;
				bool flag2 = 0 < this.m_AnimationStartFrame && this.m_AnimationStartFrame <= this.m_AnimatedSprites.Length;
				if (flag2)
				{
					Tilemap tilemapComponent = tileMap.GetComponent<Tilemap>();
					bool flag3 = tilemapComponent != null && tilemapComponent.animationFrameRate > 0f;
					if (flag3)
					{
						tileAnimationData.animationStartTime = (float)(this.m_AnimationStartFrame - 1) / tilemapComponent.animationFrameRate;
					}
				}
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		
		public Sprite[] m_AnimatedSprites;

		
		public float m_MinSpeed = 1f;

		
		public float m_MaxSpeed = 1f;

		
		public float m_AnimationStartTime;

		
		public int m_AnimationStartFrame = 0;

		
		public Tile.ColliderType m_TileColliderType;
	}
}
