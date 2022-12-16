using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	
	[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tiles/Rule Tile")]
	[Serializable]
	public class RuleTile : TileBase
	{
		

		public virtual Type m_NeighborType
		{
			get
			{
				return typeof(RuleTile.TilingRuleOutput.Neighbor);
			}
		}

		

		public virtual int m_RotationAngle
		{
			get
			{
				return 90;
			}
		}

		

		public int m_RotationCount
		{
			get
			{
				return 360 / this.m_RotationAngle;
			}
		}

		

		public HashSet<Vector3Int> neighborPositions
		{
			get
			{
				bool flag = this.m_NeighborPositions.Count == 0;
				if (flag)
				{
					this.UpdateNeighborPositions();
				}
				return this.m_NeighborPositions;
			}
		}

		 
		public void UpdateNeighborPositions()
		{
			RuleTile.m_CacheTilemapsNeighborPositions.Clear();
			HashSet<Vector3Int> positions = this.m_NeighborPositions;
			positions.Clear();
			foreach (RuleTile.TilingRule rule in this.m_TilingRules)
			{
				foreach (KeyValuePair<Vector3Int, int> neighbor in rule.GetNeighbors())
				{
					Vector3Int position = neighbor.Key;
					positions.Add(position);
					bool flag = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.Rotated;
					if (flag)
					{
						for (int angle = this.m_RotationAngle; angle < 360; angle += this.m_RotationAngle)
						{
							positions.Add(this.GetRotatedPosition(position, angle));
						}
					}
					else
					{
						bool flag2 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.MirrorXY;
						if (flag2)
						{
							positions.Add(this.GetMirroredPosition(position, true, true));
							positions.Add(this.GetMirroredPosition(position, true, false));
							positions.Add(this.GetMirroredPosition(position, false, true));
						}
						else
						{
							bool flag3 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.MirrorX;
							if (flag3)
							{
								positions.Add(this.GetMirroredPosition(position, true, false));
							}
							else
							{
								bool flag4 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.MirrorY;
								if (flag4)
								{
									positions.Add(this.GetMirroredPosition(position, false, true));
								}
							}
						}
					}
				}
			}
		}

		 
		public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject instantiatedGameObject)
		{
			bool flag = instantiatedGameObject != null;
			if (flag)
			{
				Tilemap tmpMap = tilemap.GetComponent<Tilemap>();
				Matrix4x4 orientMatrix = tmpMap.orientationMatrix;
				Matrix4x4 iden = Matrix4x4.identity;
				Vector3 gameObjectTranslation = default(Vector3);
				Quaternion gameObjectRotation = default(Quaternion);
				Vector3 gameObjectScale = default(Vector3);
				bool ruleMatched = false;
				foreach (RuleTile.TilingRule rule in this.m_TilingRules)
				{
					Matrix4x4 transform = iden;
					bool flag2 = this.RuleMatches(rule, location, tilemap, ref transform);
					if (flag2)
					{
						transform = orientMatrix * transform;
						gameObjectTranslation = new Vector3(transform.m03, transform.m13, transform.m23);
						gameObjectRotation = Quaternion.LookRotation(new Vector3(transform.m02, transform.m12, transform.m22), new Vector3(transform.m01, transform.m11, transform.m21));
						gameObjectScale = transform.lossyScale;
						ruleMatched = true;
						break;
					}
				}
				bool flag3 = !ruleMatched;
				if (flag3)
				{
					gameObjectTranslation = new Vector3(orientMatrix.m03, orientMatrix.m13, orientMatrix.m23);
					gameObjectRotation = Quaternion.LookRotation(new Vector3(orientMatrix.m02, orientMatrix.m12, orientMatrix.m22), new Vector3(orientMatrix.m01, orientMatrix.m11, orientMatrix.m21));
					gameObjectScale = orientMatrix.lossyScale;
				}
				instantiatedGameObject.transform.localPosition = gameObjectTranslation + tmpMap.CellToLocalInterpolated(location + tmpMap.tileAnchor);
				instantiatedGameObject.transform.localRotation = gameObjectRotation;
				instantiatedGameObject.transform.localScale = gameObjectScale;
			}
			return true;
		}

		 
		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			Matrix4x4 iden = Matrix4x4.identity;
			tileData.sprite = this.m_DefaultSprite;
			tileData.gameObject = this.m_DefaultGameObject;
			tileData.colliderType = this.m_DefaultColliderType;
			tileData.flags = TileFlags.LockTransform;
			tileData.transform = iden;
			foreach (RuleTile.TilingRule rule in this.m_TilingRules)
			{
				Matrix4x4 transform = iden;
				bool flag = this.RuleMatches(rule, position, tilemap, ref transform);
				if (flag)
				{
					switch (rule.m_Output)
					{
					case RuleTile.TilingRuleOutput.OutputSprite.Single:
					case RuleTile.TilingRuleOutput.OutputSprite.Animation:
						tileData.sprite = rule.m_Sprites[0];
						break;
					case RuleTile.TilingRuleOutput.OutputSprite.Random:
					{
						int index = Mathf.Clamp(Mathf.FloorToInt(RuleTile.GetPerlinValue(position, rule.m_PerlinScale, 100000f) * (float)rule.m_Sprites.Length), 0, rule.m_Sprites.Length - 1);
						tileData.sprite = rule.m_Sprites[index];
						bool flag2 = rule.m_RandomTransform > RuleTile.TilingRuleOutput.Transform.Fixed;
						if (flag2)
						{
							transform = this.ApplyRandomTransform(rule.m_RandomTransform, transform, rule.m_PerlinScale, position);
						}
						break;
					}
					}
					tileData.transform = transform;
					tileData.gameObject = rule.m_GameObject;
					tileData.colliderType = rule.m_ColliderType;
					break;
				}
			}
		}

		 
		public static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise(((float)position.x + offset) * scale, ((float)position.y + offset) * scale);
		}

		 
		private static bool IsTilemapUsedTilesChange(Tilemap tilemap)
		{
			bool flag = !RuleTile.m_CacheTilemapsNeighborPositions.ContainsKey(tilemap);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				HashSet<TileBase> oldUsedTiles = RuleTile.m_CacheTilemapsNeighborPositions[tilemap].Key;
				int newUsedTilesCount = tilemap.GetUsedTilesCount();
				bool flag2 = newUsedTilesCount != oldUsedTiles.Count;
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = RuleTile.m_AllocatedUsedTileArr.Length < newUsedTilesCount;
					if (flag3)
					{
						Array.Resize<TileBase>(ref RuleTile.m_AllocatedUsedTileArr, newUsedTilesCount);
					}
					tilemap.GetUsedTilesNonAlloc(RuleTile.m_AllocatedUsedTileArr);
					for (int i = 0; i < newUsedTilesCount; i++)
					{
						TileBase newUsedTile = RuleTile.m_AllocatedUsedTileArr[i];
						bool flag4 = !oldUsedTiles.Contains(newUsedTile);
						if (flag4)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		 
		private static void CachingTilemapNeighborPositions(Tilemap tilemap)
		{
			int usedTileCount = tilemap.GetUsedTilesCount();
			HashSet<TileBase> usedTiles = new HashSet<TileBase>();
			HashSet<Vector3Int> neighborPositions = new HashSet<Vector3Int>();
			bool flag = RuleTile.m_AllocatedUsedTileArr.Length < usedTileCount;
			if (flag)
			{
				Array.Resize<TileBase>(ref RuleTile.m_AllocatedUsedTileArr, usedTileCount);
			}
			tilemap.GetUsedTilesNonAlloc(RuleTile.m_AllocatedUsedTileArr);
			for (int i = 0; i < usedTileCount; i++)
			{
				TileBase tile = RuleTile.m_AllocatedUsedTileArr[i];
				usedTiles.Add(tile);
				RuleTile ruleTile = null;
				bool flag2 = tile is RuleTile;
				if (flag2)
				{
					ruleTile = (tile as RuleTile);
				}
				else
				{
					bool flag3 = tile is RuleOverrideTile;
					if (flag3)
					{
						ruleTile = (tile as RuleOverrideTile).m_Tile;
					}
				}
				bool flag4 = ruleTile;
				if (flag4)
				{
					foreach (Vector3Int neighborPosition in ruleTile.neighborPositions)
					{
						neighborPositions.Add(neighborPosition);
					}
				}
			}
			RuleTile.m_CacheTilemapsNeighborPositions[tilemap] = new KeyValuePair<HashSet<TileBase>, HashSet<Vector3Int>>(usedTiles, neighborPositions);
		}

		 
		private static void ReleaseDestroyedTilemapCacheData()
		{
			RuleTile.m_CacheTilemapsNeighborPositions = (from data in RuleTile.m_CacheTilemapsNeighborPositions
			where data.Key != null
			select data).ToDictionary((KeyValuePair<Tilemap, KeyValuePair<HashSet<TileBase>, HashSet<Vector3Int>>> data) => data.Key, (KeyValuePair<Tilemap, KeyValuePair<HashSet<TileBase>, HashSet<Vector3Int>>> data) => data.Value);
		}

		 
		public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			Matrix4x4 iden = Matrix4x4.identity;
			foreach (RuleTile.TilingRule rule in this.m_TilingRules)
			{
				bool flag = rule.m_Output == RuleTile.TilingRuleOutput.OutputSprite.Animation;
				if (flag)
				{
					Matrix4x4 transform = iden;
					bool flag2 = this.RuleMatches(rule, position, tilemap, ref transform);
					if (flag2)
					{
						tileAnimationData.animatedSprites = rule.m_Sprites;
						tileAnimationData.animationSpeed = rule.m_AnimationSpeed;
						return true;
					}
				}
			}
			return false;
		}

		 
		public override void RefreshTile(Vector3Int location, ITilemap tilemap)
		{
			base.RefreshTile(location, tilemap);
			Tilemap tilemap_2 = tilemap.GetComponent<Tilemap>();
			RuleTile.ReleaseDestroyedTilemapCacheData();
			bool flag = RuleTile.IsTilemapUsedTilesChange(tilemap_2);
			if (flag)
			{
				RuleTile.CachingTilemapNeighborPositions(tilemap_2);
			}
			HashSet<Vector3Int> neighborPositions = RuleTile.m_CacheTilemapsNeighborPositions[tilemap_2].Value;
			foreach (Vector3Int offset in neighborPositions)
			{
				Vector3Int position = this.GetOffsetPositionReverse(location, offset);
				TileBase tile = tilemap_2.GetTile(position);
				RuleTile ruleTile = null;
				bool flag2 = tile is RuleTile;
				if (flag2)
				{
					ruleTile = (tile as RuleTile);
				}
				else
				{
					bool flag3 = tile is RuleOverrideTile;
					if (flag3)
					{
						ruleTile = (tile as RuleOverrideTile).m_Tile;
					}
				}
				bool flag4 = ruleTile;
				if (flag4)
				{
					bool flag5 = ruleTile.neighborPositions.Contains(offset);
					if (flag5)
					{
						base.RefreshTile(position, tilemap);
					}
				}
			}
		}

		 
		public virtual bool RuleMatches(RuleTile.TilingRule rule, Vector3Int position, ITilemap tilemap, ref Matrix4x4 transform)
		{
			bool flag = this.RuleMatches(rule, position, tilemap, 0);
			bool result;
			if (flag)
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, 0f), Vector3.one);
				result = true;
			}
			else
			{
				bool flag2 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.Rotated;
				if (flag2)
				{
					for (int angle = this.m_RotationAngle; angle < 360; angle += this.m_RotationAngle)
					{
						bool flag3 = this.RuleMatches(rule, position, tilemap, angle);
						if (flag3)
						{
							transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, (float)(-(float)angle)), Vector3.one);
							return true;
						}
					}
				}
				else
				{
					bool flag4 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.MirrorXY;
					if (flag4)
					{
						bool flag5 = this.RuleMatches(rule, position, tilemap, true, true);
						if (flag5)
						{
							transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, -1f, 1f));
							return true;
						}
						bool flag6 = this.RuleMatches(rule, position, tilemap, true, false);
						if (flag6)
						{
							transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
							return true;
						}
						bool flag7 = this.RuleMatches(rule, position, tilemap, false, true);
						if (flag7)
						{
							transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
							return true;
						}
					}
					else
					{
						bool flag8 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.MirrorX;
						if (flag8)
						{
							bool flag9 = this.RuleMatches(rule, position, tilemap, true, false);
							if (flag9)
							{
								transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
								return true;
							}
						}
						else
						{
							bool flag10 = rule.m_RuleTransform == RuleTile.TilingRuleOutput.Transform.MirrorY;
							if (flag10)
							{
								bool flag11 = this.RuleMatches(rule, position, tilemap, false, true);
								if (flag11)
								{
									transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
									return true;
								}
							}
						}
					}
				}
				result = false;
			}
			return result;
		}

		 
		public virtual Matrix4x4 ApplyRandomTransform(RuleTile.TilingRuleOutput.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
		{
			float perlin = RuleTile.GetPerlinValue(position, perlinScale, 200000f);
			Matrix4x4 result;
			switch (type)
			{
			case RuleTile.TilingRuleOutput.Transform.Rotated:
			{
				int angle = Mathf.Clamp(Mathf.FloorToInt(perlin * (float)this.m_RotationCount), 0, this.m_RotationCount - 1) * this.m_RotationAngle;
				result = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, (float)(-(float)angle)), Vector3.one);
				break;
			}
			case RuleTile.TilingRuleOutput.Transform.MirrorX:
				result = original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(((double)perlin < 0.5) ? 1f : -1f, 1f, 1f));
				break;
			case RuleTile.TilingRuleOutput.Transform.MirrorY:
				result = original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, ((double)perlin < 0.5) ? 1f : -1f, 1f));
				break;
			case RuleTile.TilingRuleOutput.Transform.MirrorXY:
				result = original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3((Math.Abs((double)perlin - 0.5) > 0.25) ? 1f : -1f, ((double)perlin < 0.5) ? 1f : -1f, 1f));
				break;
			default:
				result = original;
				break;
			}
			return result;
		}

		 
		public FieldInfo[] GetCustomFields(bool isOverrideInstance)
		{
			return (from field in base.GetType().GetFields()
			where typeof(RuleTile).GetField(field.Name) == null
			where !field.IsDefined(typeof(HideInInspector))
			where !isOverrideInstance || !field.IsDefined(typeof(RuleTile.DontOverride))
			select field).ToArray<FieldInfo>();
		}

		 
		public virtual bool RuleMatch(int neighbor, TileBase other)
		{
			bool flag = other is RuleOverrideTile;
			if (flag)
			{
				other = (other as RuleOverrideTile).m_InstanceTile;
			}
			bool result;
			if (neighbor != 1)
			{
				result = (neighbor != 2 || other != this);
			}
			else
			{
				result = (other == this);
			}
			return result;
		}

		 
		public bool RuleMatches(RuleTile.TilingRule rule, Vector3Int position, ITilemap tilemap, int angle)
		{
			int i = 0;
			while (i < rule.m_Neighbors.Count && i < rule.m_NeighborPositions.Count)
			{
				int neighbor = rule.m_Neighbors[i];
				Vector3Int positionOffset = this.GetRotatedPosition(rule.m_NeighborPositions[i], angle);
				TileBase other = tilemap.GetTile(this.GetOffsetPosition(position, positionOffset));
				bool flag = !this.RuleMatch(neighbor, other);
				if (flag)
				{
					return false;
				}
				i++;
			}
			return true;
		}

		 
		public bool RuleMatches(RuleTile.TilingRule rule, Vector3Int position, ITilemap tilemap, bool mirrorX, bool mirrorY)
		{
			int i = 0;
			while (i < rule.m_Neighbors.Count && i < rule.m_NeighborPositions.Count)
			{
				int neighbor = rule.m_Neighbors[i];
				Vector3Int positionOffset = this.GetMirroredPosition(rule.m_NeighborPositions[i], mirrorX, mirrorY);
				TileBase other = tilemap.GetTile(this.GetOffsetPosition(position, positionOffset));
				bool flag = !this.RuleMatch(neighbor, other);
				if (flag)
				{
					return false;
				}
				i++;
			}
			return true;
		}

		 
		public virtual Vector3Int GetRotatedPosition(Vector3Int position, int rotation)
		{
			if (rotation <= 90)
			{
				if (rotation == 0)
				{
					return position;
				}
				if (rotation == 90)
				{
					return new Vector3Int(position.y, -position.x, 0);
				}
			}
			else
			{
				if (rotation == 180)
				{
					return new Vector3Int(-position.x, -position.y, 0);
				}
				if (rotation == 270)
				{
					return new Vector3Int(-position.y, position.x, 0);
				}
			}
			return position;
		}

		 
		public virtual Vector3Int GetMirroredPosition(Vector3Int position, bool mirrorX, bool mirrorY)
		{
			if (mirrorX)
			{
				position.x *= -1;
			}
			if (mirrorY)
			{
				position.y *= -1;
			}
			return position;
		}

		 
		public virtual Vector3Int GetOffsetPosition(Vector3Int location, Vector3Int offset)
		{
			return location + offset;
		}

		 
		public virtual Vector3Int GetOffsetPositionReverse(Vector3Int position, Vector3Int offset)
		{
			return position - offset;
		}

		
		public Sprite m_DefaultSprite;

		
		public GameObject m_DefaultGameObject;

		
		public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.Sprite;

		
		[HideInInspector]
		public List<RuleTile.TilingRule> m_TilingRules = new List<RuleTile.TilingRule>();

		
		private HashSet<Vector3Int> m_NeighborPositions = new HashSet<Vector3Int>();

		
		private static Dictionary<Tilemap, KeyValuePair<HashSet<TileBase>, HashSet<Vector3Int>>> m_CacheTilemapsNeighborPositions = new Dictionary<Tilemap, KeyValuePair<HashSet<TileBase>, HashSet<Vector3Int>>>();

		
		private static TileBase[] m_AllocatedUsedTileArr = new TileBase[0];

		
		[Serializable]
		public class TilingRuleOutput
		{
			
			public int m_Id;

			
			public Sprite[] m_Sprites = new Sprite[1];

			
			public GameObject m_GameObject;

			
			public float m_AnimationSpeed = 1f;

			
			public float m_PerlinScale = 0.5f;

			
			public RuleTile.TilingRuleOutput.OutputSprite m_Output = RuleTile.TilingRuleOutput.OutputSprite.Single;

			
			public Tile.ColliderType m_ColliderType = Tile.ColliderType.Sprite;

			
			public RuleTile.TilingRuleOutput.Transform m_RandomTransform;

			
			public class Neighbor
			{
				
				public const int This = 1;

				
				public const int NotThis = 2;
			}

			
			public enum Transform
			{
				
				Fixed,
				
				Rotated,
				
				MirrorX,
				
				MirrorY,
				
				MirrorXY
			}

			
			public enum OutputSprite
			{
				
				Single,
				
				Random,
				
				Animation
			}
		}

		
		[Serializable]
		public class TilingRule : RuleTile.TilingRuleOutput
		{
			
			public Dictionary<Vector3Int, int> GetNeighbors()
			{
				Dictionary<Vector3Int, int> dict = new Dictionary<Vector3Int, int>();
				int i = 0;
				while (i < this.m_Neighbors.Count && i < this.m_NeighborPositions.Count)
				{
					dict.Add(this.m_NeighborPositions[i], this.m_Neighbors[i]);
					i++;
				}
				return dict;
			}

			
			public void ApplyNeighbors(Dictionary<Vector3Int, int> dict)
			{
				this.m_NeighborPositions = dict.Keys.ToList<Vector3Int>();
				this.m_Neighbors = dict.Values.ToList<int>();
			}

			
			public BoundsInt GetBounds()
			{
				BoundsInt bounds = new BoundsInt(Vector3Int.zero, Vector3Int.one);
				foreach (KeyValuePair<Vector3Int, int> neighbor in this.GetNeighbors())
				{
					bounds.xMin = Mathf.Min(bounds.xMin, neighbor.Key.x);
					bounds.yMin = Mathf.Min(bounds.yMin, neighbor.Key.y);
					bounds.xMax = Mathf.Max(bounds.xMax, neighbor.Key.x + 1);
					bounds.yMax = Mathf.Max(bounds.yMax, neighbor.Key.y + 1);
				}
				return bounds;
			}

			
			public List<int> m_Neighbors = new List<int>();

			
			public List<Vector3Int> m_NeighborPositions = new List<Vector3Int>
			{
				new Vector3Int(-1, 1, 0),
				new Vector3Int(0, 1, 0),
				new Vector3Int(1, 1, 0),
				new Vector3Int(-1, 0, 0),
				new Vector3Int(1, 0, 0),
				new Vector3Int(-1, -1, 0),
				new Vector3Int(0, -1, 0),
				new Vector3Int(1, -1, 0)
			};

			
			public RuleTile.TilingRuleOutput.Transform m_RuleTransform;
		}

		
		public class DontOverride : Attribute
		{
		}
	}
}
