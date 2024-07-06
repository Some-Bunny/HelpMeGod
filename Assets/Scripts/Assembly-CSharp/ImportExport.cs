using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public static class ImportExport
{
	 
	public static void Export(string path)
	{
		if (string.IsNullOrEmpty(path))
		{
			path = Manager.FilePath;
			if (string.IsNullOrEmpty(path))
			{
				Debug.LogError("No file path to export to.");
				return;
			}
		}

		CanvasHandler.Instance.Trim();

		Debug.Log(TilemapHandler.Bounds.size);

		ImportExport.NewRoomData data = new ImportExport.NewRoomData
		{
			roomSize = new Vector2Int(TilemapHandler.Bounds.size.x, TilemapHandler.Bounds.size.y),
			enemyGUIDs = new string[0],
			enemyPositions = new Vector2[0],
			enemyAttributes = new string[0],
			waveTriggers = new string[0],
			enemyReinforcementLayers = new int[0],
			nodePaths = new int[0],
			nodeOrder = new int[0],
			nodePositions = new Vector2[0],
			nodeTypes = new string[0],
			nodeWrapModes = new string[0],
            additionalPauseDelay = new float[0],
            exitDirections = new string[0],
			exitPositions = new Vector2[0],
			floors = new string[0],
			category = "",
			placeablePositions = new Vector2[0],
			placeableGUIDs = new string[0],
			placeableAttributes = new string[0],
			visualSubtype = -1,
            weight = 1f,
			superSpecialRoomType = null,

			AmbientLight_R = 1f,
            AmbientLight_G = 1f,
            AmbientLight_B = 1f,
            usesAmbientLight = false,
            nodePathVisible = new bool[0],
			specialRoomPool = null,
        };
		
		Manager i = Manager.Instance;

		i.GetTilemap(TilemapHandler.MapType.Environment).GetComponent<EnvironmentMap>().CollectDataForExport(ref data);

		EnemyLayerHandler.Instance.CollectDataForExport(ref data);
		NodePathLayerHandler.Instance.CollectDataForExport(ref data);
		i.GetTilemap(TilemapHandler.MapType.Exits).GetComponent<ExitMap>().CollectDataForExport(ref data);
		i.GetTilemap(TilemapHandler.MapType.Placeables).GetComponent<PlaceableMap>().CollectDataForExport(ref data);

        Manager.Instance.roomProperties.CollectRoomProperties(ref data);
		using (StreamWriter sw = new StreamWriter(path, false))
		{			
			sw.WriteLine(JsonUtility.ToJson(data));
		}
		NotificationHandler.Instance.Notify("Saved to " + path);
	}

	public static void LegecyExport(string path)
	{
		bool flag = string.IsNullOrEmpty(path);
		if (flag)
		{
			path = Manager.FilePath;
			bool flag2 = string.IsNullOrEmpty(path);
			if (flag2)
			{
				Debug.LogError("No file path to export to.");
				return;
			}
		}
		ImportExport.RoomData data = new ImportExport.RoomData
		{
			enemyGUIDs = new string[0],
			enemyPositions = new Vector2[0],
			enemyAttributes = new string[0],
			waveTriggers = new string[0],
			enemyReinforcementLayers = new int[0],
			nodePaths = new int[0],
			nodePositions = new Vector2[0],
			nodeTypes = new string[0],
			nodeWrapModes = new string[0],
			exitDirections = new string[0],
			exitPositions = new Vector2[0],
			floors = new string[0],
			category = "",
			placeablePositions = new Vector2[0],
			placeableGUIDs = new string[0],
			placeableAttributes = new string[0],
			weight = 1f
		};
		CanvasHandler.Instance.Trim();
		Manager i = Manager.Instance;
		Texture2D texture = i.GetTilemap(TilemapHandler.MapType.Environment).GetComponent<EnvironmentMap>().CollectDataForExportLegecy();
		ImportExport.DumpTexture(texture);
		EnemyLayerHandler.Instance.CollectDataForExport(ref data);
		NodePathLayerHandler.Instance.CollectDataForExport(ref data);
		i.GetTilemap(TilemapHandler.MapType.Exits).GetComponent<ExitMap>().CollectDataForExport(ref data);
		i.GetTilemap(TilemapHandler.MapType.Placeables).GetComponent<PlaceableMap>().CollectDataForExport(ref data);
		Manager.Instance.roomProperties.CollectRoomProperties(ref data);
		using (StreamWriter sw = new StreamWriter(path, true))
		{
			sw.WriteLine("\n");
			sw.WriteLine(ImportExport.dataHeader);
			sw.WriteLine(JsonUtility.ToJson(data));
		}
		NotificationHandler.Instance.Notify("Saved to " + path);
	}


	public static void DumpTexture(Texture2D texture)
	{
		string path = Manager.FilePath;
		File.WriteAllBytes(path, texture.EncodeToPNG());
	}


	public static void ConvertToNewRoomFormat(string path)
	{
		if (Directory.Exists(path) && Directory.GetFiles(path)?.Length > 0)
        {

			string newFilesPath = Path.Combine(path, "Updated Room Files");

			Directory.CreateDirectory(newFilesPath);

			foreach (var filePath in Directory.GetFiles(path))
			{
				Debug.Log(filePath);
				if (!filePath.EndsWith(".room")) continue;

				Texture2D texture = ImportExport.GetTextureFromFile(filePath);
				Manager.roomSize = new Vector2Int(texture.width, texture.height);
				var data = TurnOldDataIntoNewFormat(ImportExport.ExtractRoomData(filePath), ImportExport.GetTextureFromFile(filePath));

				string pathForExport = Path.Combine(newFilesPath, Path.GetFileName(filePath).Replace(".room", "") + ".newroom");

				File.Create(pathForExport).Close();

				using (StreamWriter sw = new StreamWriter(pathForExport, true))
				{
					sw.WriteLine(JsonUtility.ToJson(data));
				}
			}		
		}	
	}

	public static ImportExport.NewRoomData TurnOldDataIntoNewFormat(ImportExport.RoomData data, Texture2D texture)
    {
		var newData = new ImportExport.NewRoomData
		{
			category = data.category,
			normalSubCategory = data.normalSubCategory,
			specialSubCategory = data.specialSubCategory,
			bossSubCategory = data.bossSubCategory,
			enemyPositions = data.enemyPositions,
			enemyGUIDs = data.enemyGUIDs,
			enemyAttributes = data.enemyAttributes,
			waveTriggers = data.waveTriggers,
			nodeTypes = data.nodeTypes,
			nodeWrapModes = data.nodeWrapModes,
			nodePositions = data.nodePositions,
			nodePaths = data.nodePaths,
			placeablePositions = data.placeablePositions,
			placeableGUIDs = data.placeableGUIDs,
			placeableAttributes = data.placeableAttributes,
			enemyReinforcementLayers = data.enemyReinforcementLayers,
			exitDirections = data.exitDirections,
			exitPositions = data.exitPositions,
			floors = data.floors,
			weight = data.weight,
			isSpecialRoom = data.isSpecialRoom,
			shuffleReinforcementPositions = data.shuffleReinforcementPositions,
			darkRoom = data.darkRoom,
			doFloorDecoration = data.doFloorDecoration,
			doWallDecoration = data.doWallDecoration,
			doLighting = data.doLighting,

			roomSize = new Vector2Int(texture.width, texture.height),
			tileInfo = "",
		};


		var pos = new List<Vector2>();

		for (int y = 0; y < texture.height; y++)
        {
			for (int x = 0; x < texture.width; x++)
			{
				var p = texture.GetPixel(x, y);
				Debug.Log(p);

				newData.tileInfo += GetTypeFromColour(p).ToString();
				//pos.Add(new Vector2(x, y));
			}
		}

		//newData.tilePositions = pos.ToArray();
		return newData;
	}

	public static int GetTypeFromColour(Color color)
    {
		if (color == Color.magenta) return 0;
		if (color == Color.white) return 1;
		if (color == Color.grey) return 2;
		if (color == Color.black) return 3;
		return 2;
	}


	public static void ImportGateKeeper(string path)
	{

		if (path.EndsWith(".newroom"))
        {
			ImportNew(path);
        }
		else
        {
			Import(path);
		}

	}

	public static void ImportNew(string path)
	{
		ImportExport.NewRoomData data = ImportExport.ExtractNewRoomData(path);
		if (data.roomSize.x < 2 || data.roomSize.y < 2 || data.roomSize.x > 999 || data.roomSize.y > 999)
		{
			Debug.LogError("Invalid room size");
		}
		else
		{
			Manager.roomSize = data.roomSize;
			Manager.Reload();
			data = ImportExport.ExtractNewRoomData(path);
			ImportExport.onLoad = delegate ()
			{
				ImportExport.PrepareEnemyMaps(data);
				ImportExport.PrepareNodeMaps(data);
			};
			ImportExport.postStart = delegate ()
			{
				ImportExport.BuildMapsFromNewData(data);
				Manager.Instance.roomProperties.ImportRoomProperties(data);
			};
			Manager.OnSceneLoaded.Enqueue(ImportExport.onLoad);
			Manager.OnPostStart.Enqueue(ImportExport.postStart);
		}
	}



	public static void Import(string path)
	{
		Texture2D texture = ImportExport.GetTextureFromFile(path);
		if (texture.width < 2 || texture.height < 2 || texture.width > 999 || texture.height > 999)
		{
			Debug.LogError("Invalid room size");
		}
		else
		{
			Manager.roomSize = new Vector2Int(texture.width, texture.height);
			Manager.Reload();
			ImportExport.RoomData data = ImportExport.ExtractRoomData(path);
			ImportExport.onLoad = delegate()
			{
				ImportExport.PrepareEnemyMaps(data);
			};
			ImportExport.postStart = delegate()
			{
				ImportExport.BuildMapsFromData(texture, data);
				Manager.Instance.roomProperties.ImportRoomProperties(data);
			};
			Manager.OnSceneLoaded.Enqueue(ImportExport.onLoad);
			Manager.OnPostStart.Enqueue(ImportExport.postStart);
		}
	}


	public static void BuildMapsFromNewData(ImportExport.NewRoomData data)
	{
		ImportExport.BuildEnvironmentMapFromNewData(data);
		ImportExport.BuildExitMapFromData(data);
		ImportExport.BuildEnemyMapsFromData(data);
        ImportExport.BuildPlaceableMapFromData(data);
        ImportExport.BuildNodeMapsFromData(data);
	}

	public static void BuildMapsFromData(Texture2D texture, ImportExport.RoomData data)
	{
		ImportExport.BuildEnvironmentMapFromData(texture);
		ImportExport.BuildExitMapFromData(data);
		ImportExport.BuildEnemyMapsFromData(data);
		ImportExport.BuildPlaceableMapFromData(data);
	}

	public static void BuildExitMapFromData(ImportExport.NewRoomData data)
	{
		if (data.exitDirections == null || data.exitPositions == null)
		{
			Debug.Log("Incomplete or no exit data found");
		}
		else
		{
			if (data.exitDirections.Length != data.exitPositions.Length)
			{
				Debug.LogError(string.Format("Uneven exit data array length: {0} != {1}", data.exitPositions.Length, data.exitDirections.Length));
			}
			else
			{
				ExitMap tilemapHandler = (ExitMap)Manager.Instance.GetTilemap(TilemapHandler.MapType.Exits);
				Tile[,] tiles = new Tile[Manager.roomSize.x, Manager.roomSize.y];
				for (int i = 0; i < data.exitDirections.Length; i++)
				{
					Vector2 position = data.exitPositions[i];
					string direction = data.exitDirections[i];
					tiles[(int)position.x - 1, (int)position.y - 1] = tilemapHandler.GetExit(direction);
				}
				tilemapHandler.BuildFromTileArray(tiles);
			}
		}
	}


	public static void BuildExitMapFromData(ImportExport.RoomData data)
	{
		if (data.exitDirections == null || data.exitPositions == null)
		{
			Debug.Log("Incomplete or no exit data found");
		}
		else
		{
			if (data.exitDirections.Length != data.exitPositions.Length)
			{
				Debug.LogError(string.Format("Uneven exit data array length: {0} != {1}", data.exitPositions.Length, data.exitDirections.Length));
			}
			else
			{
				ExitMap tilemapHandler = (ExitMap)Manager.Instance.GetTilemap(TilemapHandler.MapType.Exits);
				Tile[,] tiles = new Tile[Manager.roomSize.x, Manager.roomSize.y];
				for (int i = 0; i < data.exitDirections.Length; i++)
				{
					Vector2 position = data.exitPositions[i];
					string direction = data.exitDirections[i];
					tiles[(int)position.x - 1, (int)position.y - 1] = tilemapHandler.GetExit(direction);
				}
				tilemapHandler.BuildFromTileArray(tiles);
			}
		}
	}

	private static void BuildEnvironmentMapFromNewData(NewRoomData data)
	{
		try
		{
			int width = Manager.roomSize.x;
			int height = Manager.roomSize.y;
			TilemapHandler tilemapHandler = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment);
			Tile[,] tiles = new Tile[width, height];

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					tiles[x, y] = ImportExport.TileFromNumber(data.tileInfo[x + (y * width)].ToString(), tilemapHandler);
				}
			}
	
			tilemapHandler.BuildFromTileArray(tiles);
		}
		catch (Exception e)
		{
			Debug.LogError(e.Message);
			Debug.LogError(e.StackTrace);
		}
	}


	private static void BuildEnvironmentMapFromData(Texture2D texture)
	{
		try
		{
			int width = Manager.roomSize.x;
			int height = Manager.roomSize.y;
			TilemapHandler tilemapHandler = Manager.Instance.GetTilemap(TilemapHandler.MapType.Environment);
			Tile[,] tiles = new Tile[width, height];
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					tiles[x, y] = ImportExport.TileFromColor(texture.GetPixel(x, y), tilemapHandler);
				}
			}
			tilemapHandler.BuildFromTileArray(tiles);
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			Debug.Log(e.StackTrace);
		}
	}

	private static Tile TileFromNumber(string type, TilemapHandler tilemapHandler)
	{
		Dictionary<string, string> types = new Dictionary<string, string> { { "1", "floor" }, { "2", "wall" }, { "3", "pit" }, { "4", "ice" } , { "5", "effecthazard" }, { "6", "diagonal_NE" }, { "7", "diagonal_NW" }, { "8", "diagonal_SE" }, { "9", "diagonal_SW" }, { "X", "no_pickup_tile" }, { "G", "grass" } };

		return type != null ? tilemapHandler.palette[types[type]] : null;
	}


    private static Tile TileFromColor(Color color, TilemapHandler tilemapHandler)
	{
		Tile result;
		if (color == Color.black)
		{
			result = tilemapHandler.palette["pit"];
		}
		else
		{
			if (color == Color.white)
			{
				result = tilemapHandler.palette["floor"];
			}
			else
			{
				if (color == Color.magenta)
				{
					result = null;
				}
				else
				{
					result = tilemapHandler.palette["wall"];
				}
			}
		}
		return result;
	}

	private static void BuildPlaceableMapFromData(ImportExport.NewRoomData data)
	{
		try
		{
			TilemapHandler mapHandler = Manager.Instance.GetTilemap(TilemapHandler.MapType.Placeables);
			mapHandler.InitializeDatabase();
			for (int i = 0; i < data.placeableGUIDs.Length; i++)
			{
				string guid = data.placeableGUIDs[i];
				Vector2 position = data.placeablePositions[i];

                string attributes = String.Empty;

                if (data.enemyAttributes == null)
                {
                    
                }
                else
                {
                    attributes = data.enemyAttributes[i];
                }
                string id = mapHandler.tileDatabase.GetID(guid);
				bool flag = id == null || !mapHandler.palette.ContainsKey(id);
				if (flag)
				{
					Debug.LogError("Tile ID not found: " + id);
				}
				else
				{
					DataTile tile = TilemapHandler.Clone(mapHandler.palette[id]);
                    if (attributes != String.Empty)
                    {
                        tile.data = AttributeDatabase.ToLongNamed(JObject.Parse(attributes));
                    }
                    mapHandler.map.SetTile(TilemapHandler.GameToLocalPosition((int)position.x, (int)position.y), tile);
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			Debug.Log(e.StackTrace);
		}
	}

	private static void BuildPlaceableMapFromData(ImportExport.RoomData data)
	{
		try
		{
			TilemapHandler mapHandler = Manager.Instance.GetTilemap(TilemapHandler.MapType.Placeables);
			mapHandler.InitializeDatabase();
			for (int i = 0; i < data.placeableGUIDs.Length; i++)
			{
				string guid = data.placeableGUIDs[i];
				Vector2 position = data.placeablePositions[i];
				string attributes = data.placeableAttributes[i];
				string id = mapHandler.tileDatabase.GetID(guid);
				bool flag = id == null || !mapHandler.palette.ContainsKey(id);
				if (flag)
				{
					Debug.LogError("Tile ID not found: " + id);
				}
				else
				{
					DataTile tile = TilemapHandler.Clone(mapHandler.palette[id]);
					tile.data = AttributeDatabase.ToLongNamed(JObject.Parse(attributes));
					mapHandler.map.SetTile(TilemapHandler.GameToLocalPosition((int)position.x, (int)position.y), tile);
				}
			}
		}
		catch (Exception e)
		{
			Debug.Log(e.Message);
			Debug.Log(e.StackTrace);
		}
	}

	public static Enums.SerializedPathWrapMode ReturnWrap(string s)
	{
		switch(s)
		{
			case "LOOP":
				return Enums.SerializedPathWrapMode.LOOP;
            case "PINGPONG":
                return Enums.SerializedPathWrapMode.PINGPONG;
            case "ONCE":
                return Enums.SerializedPathWrapMode.ONCE;
            default:
                return Enums.SerializedPathWrapMode.LOOP;

        }
	}

    public static void BuildNodeMapsFromData(ImportExport.NewRoomData data)
	{
        //nodePaths
        if (NodePathLayerHandler.Instance.LayerCount != 0)
		{
			List<Dictionary<Vector2Int, Tile>> tileArrays = new List<Dictionary<Vector2Int, Tile>>();
            List<Dictionary<Vector2Int, float>> tileArrays_ = new List<Dictionary<Vector2Int, float>>();

            //Dictionary<Vector2Int, float> tileArrays_ = new Dictionary<Vector2Int, float>();

            for (int i = 0; i < NodePathLayerHandler.Instance.LayerCount; i++)
            {
                //tileArrays.Add(new Tile[Manager.roomSize.x, Manager.roomSize.y]);
                tileArrays.Add(new Dictionary<Vector2Int, Tile>());
                tileArrays_.Add(new Dictionary<Vector2Int, float>());

            }

            Dictionary<int, string> bastard = new Dictionary<int, string>();
            Dictionary<int, bool> bastard_2 = new Dictionary<int, bool>();

            List<Tuple<int, int>> stupidJankyPieceOfShit = new List<Tuple<int, int>>();  //Dictionary<int, int> stupidJankyPieceOfShit = new Dictionary<int, int>();

            for (int j = 0; j < data.nodeOrder.Length; j++)
			{
				//Debug.LogError($"data.nodeOrder[]{data.nodeOrder[j]} - j{j}");

				var pPp = data.nodePaths[j];
                stupidJankyPieceOfShit.Add(new Tuple<int, int>(pPp, j));
				if (!bastard.ContainsKey(pPp))
				{
                    bastard.Add(pPp, data.nodeWrapModes[j]);
                }
                if (!bastard_2.ContainsKey(pPp) && data.nodePathVisible != null)
				{
					//Debug.LogError(data.nodePathVisible[j]);
                    bastard_2.Add(pPp, data.nodePathVisible[j]);
                }
            }

            for (int i = 0; i < data.nodeOrder.Length; i++)
			{
                JToken value = null;

                int j = stupidJankyPieceOfShit[i].Item2;
				 //.ToString/.TryGetValue(i, out j);
				//Debug.LogError($"{data.nodeTypes[j]} - j{j} - i{i} - {data.nodeTypes[j]}");

				int layer = data.nodePaths[j];
				NodeMap mapHandler = NodePathLayerHandler.Instance.GetMap(layer);
				string guid = data.nodeTypes[j];



				Vector2 position = data.nodePositions[j];
				string id = mapHandler.tileDatabase.GetID(guid);

				Debug.Log(id);

				if (string.IsNullOrEmpty(id))
				{

				}
				else if (!mapHandler.palette.ContainsKey(id))
				{
					//Debug.Log(id);
				}
				else
				{
					DataTile tile = TilemapHandler.Clone(mapHandler.palette[id]);
					//tile.placmentOrder = stupidJankyPieceOfShit[i].Item1;
                    //Debug.Log($"{(int)position.x}, {(int)position.y} --- {Manager.roomSize.x}, {Manager.roomSize.y} --- {data.nodePositions[j]}");

                    //tileArrays[layer][(int)position.x, (int)position.y] = tile;
                    if (!tileArrays[layer].ContainsKey(new Vector2Int((int)position.x, (int)position.y)))
					{
                        tileArrays[layer].Add(new Vector2Int((int)position.x, (int)position.y), tile);
                    }
                    if (!tileArrays_[layer].ContainsKey(new Vector2Int((int)position.x, (int)position.y)))
					{
                        tileArrays_[layer].Add(new Vector2Int((int)position.x, (int)position.y), data.additionalPauseDelay != null ? data.additionalPauseDelay[i] : 0f);
                    }
                }
            }

			for (int k = 0; k < NodePathLayerHandler.Instance.LayerCount; k++)
			{

                foreach (var tileShit in tileArrays[k])
                {
					//NodePathLayerHandler.Instance.GetMap(k)

					NodePathLayerHandler.Instance.ReturnButtons()[k].WrapMode = ReturnWrap(bastard[k]);
					NodePathLayerHandler.Instance.ReturnButtons()[k].triggerDropdown.value = (int)ReturnWrap(bastard[k]);

                    NodePathLayerHandler.Instance.GetMap(k).map.SetTile(TilemapHandler.GameToLocalPosition(tileShit.Key), tileShit.Value);

					(NodePathLayerHandler.Instance.GetMap(k) as NodeMap).AddNewNodeTile(tileShit.Value as DataTile, TilemapHandler.GameToLocalPosition(tileShit.Key), tileArrays_[k][tileShit.Key]);
				}
				if (bastard_2.Count > 0)
				{
                    NodePathLayerHandler.Instance.ReturnButtons()[k].toggleVisibility.Toggled = data.nodePathVisible != null ? bastard_2[k] : true;
                }
                //NodePathLayerHandler.Instance.GetMap(k).BuildFromTileArray(tileArrays[k]);
            }
		}
	}

	public static void BuildEnemyMapsFromData(ImportExport.NewRoomData data)
	{
		
		if (EnemyLayerHandler.Instance.LayerCount != 0)
		{
            Dictionary<int, string> bastard = new Dictionary<int, string>();
            List<Tile[,]> tileArrays = new List<Tile[,]>();
			for (int i = 0; i < EnemyLayerHandler.Instance.LayerCount; i++)
			{
				tileArrays.Add(new Tile[Manager.roomSize.x, Manager.roomSize.y]);
			}
			for (int j = 0; j < data.enemyGUIDs.Length; j++)
			{

                int layer = data.enemyReinforcementLayers[j];
                if (!bastard.ContainsKey(layer))
                {
					Debug.LogError("WaveTrigger: "+layer +" : " + data.waveTriggers[j]);
                    bastard.Add(layer, data.waveTriggers[j]);
                }
                EnemyMap mapHandler = EnemyLayerHandler.Instance.GetMap(layer);
				string guid = data.enemyGUIDs[j];
				Vector2 position = data.enemyPositions[j];
				string attributes = data.enemyAttributes[j];
				string id = mapHandler.tileDatabase.GetID(guid);
				if (id != null)
				{
                    bool flag2 = !mapHandler.palette.ContainsKey(id);
                    if (flag2)
                    {
                        Debug.Log(id);
                    }
                    else
                    {
                        DataTile tile = TilemapHandler.Clone(mapHandler.palette[id]);
                        tile.data = AttributeDatabase.ToLongNamed(JObject.Parse(attributes));
                        tileArrays[layer][(int)position.x, (int)position.y] = tile;
                    }
                }
				
			}
			for (int k = 0; k < EnemyLayerHandler.Instance.LayerCount; k++)
			{
                //Debug.LogError("dadsaklokjioojuik " + k);

				if (bastard.ContainsKey(k))
				{
                    EnemyLayerHandler.Instance.ReturnButtons()[k].triggerCondition = ReturnTrigger(bastard[k]);
                    EnemyLayerHandler.Instance.ReturnButtons()[k].triggerDropdown.value = (int)ReturnTrigger(bastard[k]);

                    EnemyLayerHandler.Instance.GetMap(k).BuildFromTileArray(tileArrays[k]);
                }
			}
		}
	}
	//I want to krill myself
    public static Enums.RoomEventTriggerCondition ReturnTrigger(string s)
    {
        switch (s)
        {
            case "ON_ENEMIES_CLEARED":
                return Enums.RoomEventTriggerCondition.ON_ENEMIES_CLEARED;
            case "ENEMY_BEHAVIOR":
                return Enums.RoomEventTriggerCondition.ENEMY_BEHAVIOR;
            case "NPC_TRIGGER_A":
                return Enums.RoomEventTriggerCondition.NPC_TRIGGER_A;
            case "ON_HALF_ENEMY_HP_DEPLETED":
                return Enums.RoomEventTriggerCondition.ON_HALF_ENEMY_HP_DEPLETED;
            case "ON_ONE_QUARTER_ENEMY_HP_DEPLETED":
                return Enums.RoomEventTriggerCondition.ON_ONE_QUARTER_ENEMY_HP_DEPLETED;
            case "ON_THREE_QUARTERS_ENEMY_HP_DEPLETED":
                return Enums.RoomEventTriggerCondition.ON_THREE_QUARTERS_ENEMY_HP_DEPLETED;
            case "SEQUENTIAL_WAVE_TRIGGER":
                return Enums.RoomEventTriggerCondition.SEQUENTIAL_WAVE_TRIGGER;
            case "SHRINE_WAVE_A":
                return Enums.RoomEventTriggerCondition.SHRINE_WAVE_A;
            case "SHRINE_WAVE_B":
                return Enums.RoomEventTriggerCondition.SHRINE_WAVE_B;
            case "SHRINE_WAVE_C":
                return Enums.RoomEventTriggerCondition.SHRINE_WAVE_C;
            case "NPC_TRIGGER_B":
                return Enums.RoomEventTriggerCondition.NPC_TRIGGER_B;
            case "NPC_TRIGGER_C":
                return Enums.RoomEventTriggerCondition.NPC_TRIGGER_C;
            case "ON_ENTER":
                return Enums.RoomEventTriggerCondition.ON_ENTER;
            case "ON_ENTER_WITH_ENEMIES":
                return Enums.RoomEventTriggerCondition.ON_ENTER_WITH_ENEMIES;
            case "ON_EXIT":
                return Enums.RoomEventTriggerCondition.ON_EXIT;
            case "ON_NINETY_PERCENT_ENEMY_HP_DEPLETED":
                return Enums.RoomEventTriggerCondition.ON_NINETY_PERCENT_ENEMY_HP_DEPLETED;
            case "TIMER":
                return Enums.RoomEventTriggerCondition.TIMER;
            default:
                return Enums.RoomEventTriggerCondition.ON_ENEMIES_CLEARED;

        }
    }

    public static void BuildEnemyMapsFromData(ImportExport.RoomData data)
	{
		bool flag = EnemyLayerHandler.Instance.LayerCount == 0;
		if (!flag)
		{
			List<Tile[,]> tileArrays = new List<Tile[,]>();
			for (int i = 0; i < EnemyLayerHandler.Instance.LayerCount; i++)
			{
				tileArrays.Add(new Tile[Manager.roomSize.x, Manager.roomSize.y]);
			}
			for (int j = 0; j < data.enemyGUIDs.Length; j++)
			{

                int layer = data.enemyReinforcementLayers[j];

                EnemyMap mapHandler = EnemyLayerHandler.Instance.GetMap(layer);

                string guid = data.enemyGUIDs[j];

                Vector2 position = data.enemyPositions[j];

                string attributes = String.Empty;

                if (data.enemyAttributes == null)
				{

                }
				else
				{
					attributes = data.enemyAttributes[j];
                }
				string id = mapHandler.tileDatabase.GetID(guid);
				bool flag2 = !mapHandler.palette.ContainsKey(id);
				if (flag2)
				{
					Debug.Log(id);

                }
                else
				{
                    DataTile tile = TilemapHandler.Clone(mapHandler.palette[id]);
					if (attributes != String.Empty)
					{
                        tile.data = AttributeDatabase.ToLongNamed(JObject.Parse(attributes));
                    }
                    tileArrays[layer][(int)position.x, (int)position.y] = tile;

                }
            }
			for (int k = 0; k < EnemyLayerHandler.Instance.LayerCount; k++)
			{
                EnemyLayerHandler.Instance.GetMap(k).BuildFromTileArray(tileArrays[k]);
            }
        }
	}


	public static void PrepareNodeMaps(ImportExport.NewRoomData data)
	{
		if (data.nodeTypes == null || data.nodePaths == null || data.nodePositions == null || data.nodeWrapModes == null || data.nodeOrder == null || data.nodePathVisible == null) return;

		if (data.nodeTypes.Length != data.nodePaths.Length || data.nodeTypes.Length != data.nodePositions.Length || data.nodeTypes.Length != data.nodeWrapModes.Length)
		{
			Debug.LogError($"Uneven Node data array length: {data.nodeTypes.Length} != {data.nodePositions.Length} != {data.nodePaths.Length} != {data.nodeWrapModes.Length} != {data.nodePathVisible.Length}");
			//return;
		}
		int numLayers = 0;
		foreach (int layer in data.nodePaths) 
		{
            numLayers = Mathf.Max(numLayers, layer); 
			//data.nodeTypes[layer] = 

        } 
		NodePathLayerHandler.Instance.scheduledLayers = numLayers + 1;

	}

	public static void PrepareEnemyMaps(ImportExport.NewRoomData data)
	{
		if (data.enemyGUIDs == null || data.enemyReinforcementLayers == null || data.enemyPositions == null || data.waveTriggers == null) return;

		if (data.enemyGUIDs.Length != data.enemyReinforcementLayers.Length || data.enemyGUIDs.Length != data.enemyPositions.Length || data.enemyGUIDs.Length != data.waveTriggers.Length)
		{
			Debug.LogError($"Uneven enemy data array length: {data.enemyGUIDs.Length} != {data.enemyPositions.Length} != {data.enemyReinforcementLayers.Length} != {data.waveTriggers.Length}");
			return;
		}
		int numLayers = 0;
		foreach (int layer in data.enemyReinforcementLayers) numLayers = Mathf.Max(numLayers, layer);
		EnemyLayerHandler.Instance.scheduledLayers = numLayers + 1;

	}


	public static void PrepareEnemyMaps(ImportExport.RoomData data)
	{
		if (data.enemyGUIDs == null || data.enemyReinforcementLayers == null || data.enemyPositions == null || data.waveTriggers == null) return;
		
		if (data.enemyGUIDs.Length != data.enemyReinforcementLayers.Length || data.enemyGUIDs.Length != data.enemyPositions.Length || data.enemyGUIDs.Length != data.waveTriggers.Length)
		{
			Debug.LogError($"Uneven enemy data array length: {data.enemyGUIDs.Length} != {data.enemyPositions.Length} != {data.enemyReinforcementLayers.Length} != {data.waveTriggers.Length}");
			return;
		}
		int numLayers = 0;
		foreach (int layer in data.enemyReinforcementLayers) numLayers = Mathf.Max(numLayers, layer);
		EnemyLayerHandler.Instance.scheduledLayers = numLayers + 1;

	}

	public static ImportExport.NewRoomData ExtractNewRoomData(string path)
	{
		string data = File.ReadAllText(path);
		return JsonUtility.FromJson<ImportExport.NewRoomData>(data);
	}

	public static ImportExport.RoomData ExtractRoomData(string path)
	{
		string data = File.ReadAllText(path);
		int end = data.Length - ImportExport.dataHeader.Length - 1;
		for (int i = end; i > 0; i--)
		{
			string sub = data.Substring(i, ImportExport.dataHeader.Length);
			bool flag = sub.Equals(ImportExport.dataHeader);
			if (flag)
			{
				return JsonUtility.FromJson<ImportExport.RoomData>(data.Substring(i + ImportExport.dataHeader.Length));
			}
		}
		Debug.Log("Failed to load data");
		return default(ImportExport.RoomData);
	}

	 
	public static Texture2D GetTextureFromFile(string path)
	{
		Texture2D texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
		texture.LoadImage(File.ReadAllBytes(path));
		texture.filterMode = FilterMode.Point;
		texture.name = Path.GetFileName(path);
		return texture;
	}

	
	public static readonly string dataHeader = "***DATA***";

	
	public static Action postStart;

	
	public static Action onLoad;

	public struct NewRoomData
	{
		public string tileInfo;
		//public Vector2[] tilePositions;

		public Vector2Int roomSize;

		public string category;
		public string normalSubCategory;
		public string specialSubCategory;
		public string bossSubCategory;

		public Vector2[] enemyPositions;
		public string[] enemyGUIDs;
		public string[] enemyAttributes;
		public string[] waveTriggers;

		public string[] nodeTypes;
		public string[] nodeWrapModes;
        public bool[] nodePathVisible;
        public Vector2[] nodePositions;
		public int[] nodePaths;
		public int[] nodeOrder;
        public float[] additionalPauseDelay;


        public Vector2[] placeablePositions;
		public string[] placeableGUIDs;
		public string[] placeableAttributes;

		public int[] enemyReinforcementLayers;

		public Vector2[] exitPositions;
		public string[] exitDirections;

		public string[] floors;

		public float weight;

		public bool isSpecialRoom;
		public bool shuffleReinforcementPositions;
		public bool darkRoom;
		public bool doFloorDecoration;
		public bool doWallDecoration;
		public bool doLighting;

		public int visualSubtype;
        public string superSpecialRoomType;

        public float AmbientLight_R;
        public float AmbientLight_G;
        public float AmbientLight_B;

        public bool usesAmbientLight;

        public string specialRoomPool;


    }

    public struct RoomData
	{

		public string category;

		
		public string normalSubCategory;

		
		public string specialSubCategory;

		
		public string bossSubCategory;

		
		public Vector2[] enemyPositions;

		
		public string[] enemyGUIDs;

		
		public string[] enemyAttributes;
		public string[] waveTriggers;

		public string[] nodeTypes;
		public string[] nodeWrapModes;
		public Vector2[] nodePositions;
		public int[] nodePaths;

		
		public Vector2[] placeablePositions;

		
		public string[] placeableGUIDs;

		
		public string[] placeableAttributes;

		
		public int[] enemyReinforcementLayers;

		
		public Vector2[] exitPositions;

		
		public string[] exitDirections;

		
		public string[] floors;

		
		public float weight;

		
		public bool isSpecialRoom;

		
		public bool shuffleReinforcementPositions;

		
		public bool darkRoom;

		
		public bool doFloorDecoration;

		
		public bool doWallDecoration;

		
		public bool doLighting;


	}
}
