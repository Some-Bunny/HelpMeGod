using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Scripting.APIUpdating;

namespace UnityEngine.Tilemaps
{
	
	[MovedFrom(true, "UnityEngine", null, null)]
	[CreateAssetMenu(fileName = "New Rule Override Tile", menuName = "Tiles/Rule Override Tile")]
	[Serializable]
	public class RuleOverrideTile : TileBase
	{
		
		public Sprite this[Sprite originalSprite]
		{
			get
			{
				foreach (RuleOverrideTile.TileSpritePair spritePair in this.m_Sprites)
				{
					bool flag = spritePair.m_OriginalSprite == originalSprite;
					if (flag)
					{
						return spritePair.m_OverrideSprite;
					}
				}
				return null;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					this.m_Sprites = (from spritePair in this.m_Sprites
					where spritePair.m_OriginalSprite != originalSprite
					select spritePair).ToList<RuleOverrideTile.TileSpritePair>();
				}
				else
				{
					foreach (RuleOverrideTile.TileSpritePair spritePair2 in this.m_Sprites)
					{
						bool flag2 = spritePair2.m_OriginalSprite == originalSprite;
						if (flag2)
						{
							spritePair2.m_OverrideSprite = value;
							return;
						}
					}
					this.m_Sprites.Add(new RuleOverrideTile.TileSpritePair
					{
						m_OriginalSprite = originalSprite,
						m_OverrideSprite = value
					});
				}
			}
		}

		
		public GameObject this[GameObject originalGameObject]
		{
			get
			{
				foreach (RuleOverrideTile.TileGameObjectPair gameObjectPair in this.m_GameObjects)
				{
					bool flag = gameObjectPair.m_OriginalGameObject == originalGameObject;
					if (flag)
					{
						return gameObjectPair.m_OverrideGameObject;
					}
				}
				return null;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					this.m_GameObjects = (from gameObjectPair in this.m_GameObjects
					where gameObjectPair.m_OriginalGameObject != originalGameObject
					select gameObjectPair).ToList<RuleOverrideTile.TileGameObjectPair>();
				}
				else
				{
					foreach (RuleOverrideTile.TileGameObjectPair gameObjectPair2 in this.m_GameObjects)
					{
						bool flag2 = gameObjectPair2.m_OriginalGameObject == originalGameObject;
						if (flag2)
						{
							gameObjectPair2.m_OverrideGameObject = value;
							return;
						}
					}
					this.m_GameObjects.Add(new RuleOverrideTile.TileGameObjectPair
					{
						m_OriginalGameObject = originalGameObject,
						m_OverrideGameObject = value
					});
				}
			}
		}

		 
		public void ApplyOverrides(IList<KeyValuePair<Sprite, Sprite>> overrides)
		{
			bool flag = overrides == null;
			if (flag)
			{
				throw new ArgumentNullException("overrides");
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				this[overrides[i].Key] = overrides[i].Value;
			}
		}

		 
		public void ApplyOverrides(IList<KeyValuePair<GameObject, GameObject>> overrides)
		{
			bool flag = overrides == null;
			if (flag)
			{
				throw new ArgumentNullException("overrides");
			}
			for (int i = 0; i < overrides.Count; i++)
			{
				this[overrides[i].Key] = overrides[i].Value;
			}
		}

		 
		public void GetOverrides(List<KeyValuePair<Sprite, Sprite>> overrides, ref int validCount)
		{
			bool flag = overrides == null;
			if (flag)
			{
				throw new ArgumentNullException("overrides");
			}
			overrides.Clear();
			List<Sprite> originalSprites = new List<Sprite>();
			bool flag2 = this.m_Tile;
			if (flag2)
			{
				bool flag3 = this.m_Tile.m_DefaultSprite;
				if (flag3)
				{
					originalSprites.Add(this.m_Tile.m_DefaultSprite);
				}
				foreach (RuleTile.TilingRule rule in this.m_Tile.m_TilingRules)
				{
					foreach (Sprite sprite in rule.m_Sprites)
					{
						bool flag4 = sprite && !originalSprites.Contains(sprite);
						if (flag4)
						{
							originalSprites.Add(sprite);
						}
					}
				}
			}
			validCount = originalSprites.Count;
			foreach (RuleOverrideTile.TileSpritePair pair in this.m_Sprites)
			{
				bool flag5 = !originalSprites.Contains(pair.m_OriginalSprite);
				if (flag5)
				{
					originalSprites.Add(pair.m_OriginalSprite);
				}
			}
			foreach (Sprite sprite2 in originalSprites)
			{
				overrides.Add(new KeyValuePair<Sprite, Sprite>(sprite2, this[sprite2]));
			}
		}

		 
		public void GetOverrides(List<KeyValuePair<GameObject, GameObject>> overrides, ref int validCount)
		{
			bool flag = overrides == null;
			if (flag)
			{
				throw new ArgumentNullException("overrides");
			}
			overrides.Clear();
			List<GameObject> originalGameObjects = new List<GameObject>();
			bool flag2 = this.m_Tile;
			if (flag2)
			{
				bool flag3 = this.m_Tile.m_DefaultGameObject;
				if (flag3)
				{
					originalGameObjects.Add(this.m_Tile.m_DefaultGameObject);
				}
				foreach (RuleTile.TilingRule rule in this.m_Tile.m_TilingRules)
				{
					bool flag4 = rule.m_GameObject && !originalGameObjects.Contains(rule.m_GameObject);
					if (flag4)
					{
						originalGameObjects.Add(rule.m_GameObject);
					}
				}
			}
			validCount = originalGameObjects.Count;
			foreach (RuleOverrideTile.TileGameObjectPair pair in this.m_GameObjects)
			{
				bool flag5 = !originalGameObjects.Contains(pair.m_OriginalGameObject);
				if (flag5)
				{
					originalGameObjects.Add(pair.m_OriginalGameObject);
				}
			}
			foreach (GameObject gameObject in originalGameObjects)
			{
				overrides.Add(new KeyValuePair<GameObject, GameObject>(gameObject, this[gameObject]));
			}
		}

		
		public virtual void Override()
		{
			bool flag = !this.m_Tile || !this.m_InstanceTile;
			if (!flag)
			{
				this.PrepareOverride();
				RuleTile tile = this.m_InstanceTile;
				tile.m_DefaultSprite = (this[tile.m_DefaultSprite] ?? tile.m_DefaultSprite);
				tile.m_DefaultGameObject = (this[tile.m_DefaultGameObject] ?? tile.m_DefaultGameObject);
				foreach (RuleTile.TilingRule rule in tile.m_TilingRules)
				{
					for (int i = 0; i < rule.m_Sprites.Length; i++)
					{
						Sprite sprite = rule.m_Sprites[i];
						rule.m_Sprites[i] = (this[sprite] ?? sprite);
					}
					rule.m_GameObject = (this[rule.m_GameObject] ?? rule.m_GameObject);
				}
			}
		}

		
		public void PrepareOverride()
		{
			Dictionary<FieldInfo, object> customData = this.m_InstanceTile.GetCustomFields(true).ToDictionary((FieldInfo field) => field, (FieldInfo field) => field.GetValue(this.m_InstanceTile));
			JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(this.m_Tile), this.m_InstanceTile);
			foreach (KeyValuePair<FieldInfo, object> kvp in customData)
			{
				kvp.Key.SetValue(this.m_InstanceTile, kvp.Value);
			}
		}

		
		public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			bool flag = !this.m_InstanceTile;
			return !flag && this.m_InstanceTile.GetTileAnimationData(position, tilemap, ref tileAnimationData);
		}

		
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			bool flag = !this.m_InstanceTile;
			if (!flag)
			{
				this.m_InstanceTile.GetTileData(position, tilemap, ref tileData);
			}
		}

		
		public override void RefreshTile(Vector3Int position, ITilemap tilemap)
		{
			bool flag = !this.m_InstanceTile;
			if (!flag)
			{
				this.m_InstanceTile.RefreshTile(position, tilemap);
			}
		}

		
		public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
		{
			bool flag = !this.m_InstanceTile;
			return flag || this.m_InstanceTile.StartUp(position, tilemap, go);
		}

		
		public RuleTile m_Tile;

		
		public List<RuleOverrideTile.TileSpritePair> m_Sprites = new List<RuleOverrideTile.TileSpritePair>();

		
		public List<RuleOverrideTile.TileGameObjectPair> m_GameObjects = new List<RuleOverrideTile.TileGameObjectPair>();

		
		[HideInInspector]
		public RuleTile m_InstanceTile;

		
		[Serializable]
		public class TileSpritePair
		{
			
			public Sprite m_OriginalSprite;

			
			public Sprite m_OverrideSprite;
		}

		
		[Serializable]
		public class TileGameObjectPair
		{
			
			public GameObject m_OriginalGameObject;

			
			public GameObject m_OverrideGameObject;
		}
	}
}
