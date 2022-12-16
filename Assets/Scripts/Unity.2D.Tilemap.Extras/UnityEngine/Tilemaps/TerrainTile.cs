using System;

namespace UnityEngine.Tilemaps
{
	
	[CreateAssetMenu(fileName = "New Terrain Tile", menuName = "Tiles/Terrain Tile")]
	[Serializable]
	public class TerrainTile : TileBase
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
			mask += (this.TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0);
			mask += (this.TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0);
			byte original = (byte)mask;
			bool flag = (original | 254) < byte.MaxValue;
			if (flag)
			{
				mask &= 125;
			}
			bool flag2 = (original | 251) < byte.MaxValue;
			if (flag2)
			{
				mask &= 245;
			}
			bool flag3 = (original | 239) < byte.MaxValue;
			if (flag3)
			{
				mask &= 215;
			}
			bool flag4 = (original | 191) < byte.MaxValue;
			if (flag4)
			{
				mask &= 95;
			}
			int index = this.GetIndex((byte)mask);
			bool flag5 = index >= 0 && index < this.m_Sprites.Length && this.TileValue(tileMap, location);
			if (flag5)
			{
				tileData.sprite = this.m_Sprites[index];
				tileData.transform = this.GetTransform((byte)mask);
				tileData.color = Color.white;
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
			if (mask <= 197)
			{
				if (mask <= 87)
				{
					if (mask <= 23)
					{
						switch (mask)
						{
						case 0:
							return 0;
						case 1:
						case 4:
							break;
						case 2:
						case 3:
						case 6:
							goto IL_245;
						case 5:
							goto IL_20B;
						case 7:
							goto IL_20F;
						default:
							switch (mask)
							{
							case 16:
								break;
							case 17:
								goto IL_213;
							case 18:
							case 19:
							case 22:
								goto IL_245;
							case 20:
								goto IL_20B;
							case 21:
								goto IL_217;
							case 23:
								goto IL_21B;
							default:
								goto IL_245;
							}
							break;
						}
					}
					else
					{
						switch (mask)
						{
						case 28:
							goto IL_20F;
						case 29:
							goto IL_21F;
						case 30:
							goto IL_245;
						case 31:
							goto IL_223;
						default:
							switch (mask)
							{
							case 64:
								break;
							case 65:
								goto IL_20B;
							case 66:
							case 67:
							case 70:
								goto IL_245;
							case 68:
								goto IL_213;
							case 69:
								goto IL_217;
							case 71:
								goto IL_21F;
							default:
								switch (mask)
								{
								case 80:
									goto IL_20B;
								case 81:
								case 84:
									goto IL_217;
								case 82:
								case 83:
								case 86:
									goto IL_245;
								case 85:
									return 9;
								case 87:
									goto IL_22C;
								default:
									goto IL_245;
								}
								break;
							}
							break;
						}
					}
					return 1;
					IL_20B:
					return 2;
					IL_213:
					return 4;
					IL_217:
					return 5;
				}
				if (mask <= 119)
				{
					switch (mask)
					{
					case 92:
						goto IL_21B;
					case 93:
						goto IL_22C;
					case 94:
						goto IL_245;
					case 95:
						goto IL_231;
					default:
						switch (mask)
						{
						case 112:
							break;
						case 113:
							goto IL_21B;
						case 114:
						case 115:
						case 118:
							goto IL_245;
						case 116:
							goto IL_21F;
						case 117:
							goto IL_22C;
						case 119:
							goto IL_236;
						default:
							goto IL_245;
						}
						break;
					}
				}
				else
				{
					switch (mask)
					{
					case 124:
						goto IL_223;
					case 125:
						goto IL_231;
					case 126:
						goto IL_245;
					case 127:
						goto IL_23B;
					default:
						if (mask != 193)
						{
							if (mask != 197)
							{
								goto IL_245;
							}
							goto IL_21B;
						}
						break;
					}
				}
				IL_20F:
				return 3;
				IL_21B:
				return 6;
			}
			if (mask <= 221)
			{
				if (mask <= 209)
				{
					if (mask == 199)
					{
						goto IL_223;
					}
					if (mask != 209)
					{
						goto IL_245;
					}
				}
				else
				{
					if (mask == 213)
					{
						goto IL_22C;
					}
					if (mask == 215)
					{
						goto IL_231;
					}
					if (mask != 221)
					{
						goto IL_245;
					}
					goto IL_236;
				}
			}
			else if (mask <= 245)
			{
				if (mask == 223)
				{
					goto IL_23B;
				}
				if (mask == 241)
				{
					goto IL_223;
				}
				if (mask != 245)
				{
					goto IL_245;
				}
				goto IL_231;
			}
			else
			{
				if (mask == 247 || mask == 253)
				{
					goto IL_23B;
				}
				if (mask != 255)
				{
					goto IL_245;
				}
				return 14;
			}
			IL_21F:
			return 7;
			IL_223:
			return 8;
			IL_22C:
			return 10;
			IL_231:
			return 11;
			IL_236:
			return 12;
			IL_23B:
			return 13;
			IL_245:
			return -1;
		}

		
		private Matrix4x4 GetTransform(byte mask)
		{
			if (mask <= 193)
			{
				if (mask <= 71)
				{
					if (mask <= 16)
					{
						if (mask != 4)
						{
							if (mask != 16)
							{
								goto IL_1E9;
							}
							goto IL_19D;
						}
					}
					else if (mask != 20 && mask != 28)
					{
						switch (mask)
						{
						case 64:
						case 65:
						case 69:
						case 71:
							goto IL_1C3;
						case 66:
						case 67:
						case 70:
							goto IL_1E9;
						case 68:
							break;
						default:
							goto IL_1E9;
						}
					}
				}
				else if (mask <= 93)
				{
					if (mask - 80 <= 1)
					{
						goto IL_19D;
					}
					if (mask != 84 && mask - 92 > 1)
					{
						goto IL_1E9;
					}
				}
				else
				{
					switch (mask)
					{
					case 112:
					case 113:
					case 117:
						goto IL_19D;
					case 114:
					case 115:
						goto IL_1E9;
					case 116:
						break;
					default:
						if (mask - 124 > 1)
						{
							if (mask != 193)
							{
								goto IL_1E9;
							}
							goto IL_1C3;
						}
						break;
					}
				}
			}
			else if (mask <= 215)
			{
				if (mask <= 199)
				{
					if (mask != 197 && mask != 199)
					{
						goto IL_1E9;
					}
					goto IL_1C3;
				}
				else
				{
					if (mask == 209)
					{
						goto IL_19D;
					}
					if (mask != 213 && mask != 215)
					{
						goto IL_1E9;
					}
					goto IL_1C3;
				}
			}
			else if (mask <= 241)
			{
				if (mask != 221)
				{
					if (mask == 223)
					{
						goto IL_1C3;
					}
					if (mask != 241)
					{
						goto IL_1E9;
					}
					goto IL_19D;
				}
			}
			else
			{
				if (mask == 245 || mask == 247)
				{
					goto IL_19D;
				}
				if (mask != 253)
				{
					goto IL_1E9;
				}
			}
			return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -90f), Vector3.one);
			IL_19D:
			return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -180f), Vector3.one);
			IL_1C3:
			return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -270f), Vector3.one);
			IL_1E9:
			return Matrix4x4.identity;
		}

		
		[SerializeField]
		public Sprite[] m_Sprites;
	}
}
