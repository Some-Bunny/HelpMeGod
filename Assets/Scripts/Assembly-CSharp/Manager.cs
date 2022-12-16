using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class Manager : MonoBehaviour
{
	 
	private void Awake()
	{
		Manager.Instance = this;
		SceneManager.sceneLoaded += this.SceneLoaded;
		this.roomProperties = new RoomProperties();

		EnemyOutputPath = Path.Combine(Application.persistentDataPath, "CustomEnemyData");
		PlaceableOutputPath = Path.Combine(Application.persistentDataPath, "CustomPlaceableData");

		if (!Directory.Exists(EnemyOutputPath)) Directory.CreateDirectory(EnemyOutputPath);
		if (!Directory.Exists(PlaceableOutputPath)) Directory.CreateDirectory(PlaceableOutputPath);

	}


	public static string EnemyOutputPath;
	public static string PlaceableOutputPath;


	public void SceneLoaded(Scene scene, LoadSceneMode mode)
	{
		while (Manager.OnSceneLoaded.Count > 0)
		{
			Action action = Manager.OnSceneLoaded.Dequeue();
			if (action != null)
			{
				action();
			}
		}
	}

	 
	public TilemapHandler GetTilemap(TilemapHandler.MapType type)
	{
		TilemapHandler result;
		switch (type)
		{
		case TilemapHandler.MapType.Environment:
			result = this.environment;
			break;
		case TilemapHandler.MapType.Exits:
			result = this.exits;
			break;
		case TilemapHandler.MapType.Enemies:
			result = EnemyLayerHandler.Instance.GetActiveTilemap();
			break;
		case TilemapHandler.MapType.Nodes:
			result = NodePathLayerHandler.Instance.GetActiveTilemap();
			break;
		case TilemapHandler.MapType.Placeables:
			result = this.placeables;
			break;
		default:
			result = null;
			break;
		}
		return result;
	}

	 
	private void Start()
	{
		TilemapHandler.Bounds = new BoundsInt(-Manager.roomSize.x / 2, -Manager.roomSize.y / 2, 0, Manager.roomSize.x, Manager.roomSize.y, 0);
		base.StartCoroutine("WaitForStart");
		this.postStart = delegate()
		{
			if (string.IsNullOrEmpty(Manager.FilePath) && Manager.r0dent)
            {
				((EnvironmentMap)this.environment).DrawBorder();
				((EnvironmentMap)this.environment).CreatRodentRoom();
			}
			else if (string.IsNullOrEmpty(Manager.FilePath) && Manager.drawBorder)
			{
				((EnvironmentMap)this.environment).DrawBorder();
			}
		};
		BoarderHandler.Instance.gameObject.SetActive(false);
		BoarderHandler.Instance.Toggle();
		Manager.OnPostStart.Enqueue(this.postStart);
	}

	 
	public static void Reload()
	{
		SceneManager.LoadScene("RoomEditor");
	}

	 
	public IEnumerator WaitForStart()
	{
		foreach (TilemapHandler map in UnityEngine.Object.FindObjectsOfType<TilemapHandler>())
		{
			bool flag = !map.hasStarted;
			if (flag)
			{
				yield return new WaitForSeconds(0.05f);
			}
		}
		TilemapHandler[] array = null;
		while (Manager.OnPostStart.Count > 0)
		{
			Action action = Manager.OnPostStart.Dequeue();
			if (action != null)
			{
				action();
			}
		}
		this.LateStartCompleted = true;
		yield break;
	}

	
	public static Vector2Int roomSize = new Vector2Int(30, 30);

	
	public static bool drawBorder = true;

	public static bool r0dent = false;

	
	public static Queue<Action> OnSceneLoaded = new Queue<Action>();

	
	public static Queue<Action> OnPostStart = new Queue<Action>();

	
	public static bool OpeningFile = false;

	
	public static Manager Instance;

	
	public static string FilePath;

	
	public static string paletteDividerGuid = "PALETTEDIVIDER---NOTANOBJECT";

	
	public bool LateStartCompleted = false;

	
	public InputHandler inputHandler;

	
	public TilemapHandler environment;

	
	public TilemapHandler exits;

	
	public TilemapHandler placeables;

	
	public GameObject editorPanel;

	
	public Tile emptyTile;

	
	public GameObject tileText;

	
	public Sprite missingImageTileSprite;

	
	public Action postStart;

	
	public RoomProperties roomProperties;

	public BoarderHandler boarderHandler;
}
