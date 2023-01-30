using System;
using System.Collections.Generic;
using System.IO;
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
		ImportExport.BuildNodeMapsFromData(data);
		ImportExport.BuildPlaceableMapFromData(data);
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
					tiles[x, y] = ImportExport.TileFromNumber(int.Parse(data.tileInfo[x + (y * width)].ToString()), tilemapHandler);
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

	private static Tile TileFromNumber(int type, TilemapHandler tilemapHandler)
	{
		Dictionary<int, string> types = new Dictionary<int, string> { { 1, "floor" }, { 2, "wall" }, { 3, "pit" } };

		return type > 0 ? tilemapHandler.palette[types[type]] : null;
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

	public static void BuildNodeMapsFromData(ImportExport.NewRoomData data)
	{
		if (NodePathLayerHandler.Instance.LayerCount != 0)
		{
			List<Tile[,]> tileArrays = new List<Tile[,]>();
			for (int i = 0; i < NodePathLayerHandler.Instance.LayerCount; i++)
			{
				tileArrays.Add(new Tile[Manager.roomSize.x, Manager.roomSize.y]);
			}
			for (int j = 0; j < data.nodeTypes.Length; j++)
			{
				int layer = data.nodePaths[j];
				NodeMap mapHandler = NodePathLayerHandler.Instance.GetMap(layer);
				string guid = data.nodeTypes[j];



				Vector2 position = data.nodePositions[j];
				string id = mapHandler.tileDatabase.GetID(guid);

				Debug.Log(id);

				if (string.IsNullOrEmpty(id))
                {
					
                }
				else if(!mapHandler.palette.ContainsKey(id))
				{
					Debug.Log(id);
				}
				else
				{
					DataTile tile = TilemapHandler.Clone(mapHandler.palette[id]);
					tileArrays[layer][(int)position.x, (int)position.y] = tile;
				}
			}
			for (int k = 0; k < NodePathLayerHandler.Instance.LayerCount; k++)
			{
				NodePathLayerHandler.Instance.GetMap(k).BuildFromTileArray(tileArrays[k]);
			}
		}
	}

	public static void BuildEnemyMapsFromData(ImportExport.NewRoomData data)
	{
		
		if (EnemyLayerHandler.Instance.LayerCount != 0)
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
				string attributes = data.enemyAttributes[j];
				string id = mapHandler.tileDatabase.GetID(guid);
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
			for (int k = 0; k < EnemyLayerHandler.Instance.LayerCount; k++)
			{
				EnemyLayerHandler.Instance.GetMap(k).BuildFromTileArray(tileArrays[k]);
			}
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
				string attributes = data.enemyAttributes[j];
				string id = mapHandler.tileDatabase.GetID(guid);
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
			for (int k = 0; k < EnemyLayerHandler.Instance.LayerCount; k++)
			{
				EnemyLayerHandler.Instance.GetMap(k).BuildFromTileArray(tileArrays[k]);
			}
		}
	}


	public static void PrepareNodeMaps(ImportExport.NewRoomData data)
	{
		if (data.nodeTypes == null || data.nodePaths == null || data.nodePositions == null || data.nodeWrapModes == null) return;

		if (data.nodeTypes.Length != data.nodePaths.Length || data.nodeTypes.Length != data.nodePositions.Length || data.nodeTypes.Length != data.nodeWrapModes.Length)
		{
			Debug.LogError($"Uneven enemy data array length: {data.nodeTypes.Length} != {data.nodePositions.Length} != {data.nodePaths.Length} != {data.nodeWrapModes.Length}");
			//return;
		}
		int numLayers = 0;
		foreach (int layer in data.nodePaths) numLayers = Mathf.Max(numLayers, layer);
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
