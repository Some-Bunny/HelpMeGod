using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class DataTile : Tile/*, IEquatable<DataTile>, IComparable<DataTile>*/
{
	
	public JObject data = new JObject();
    public bool isNode = false;
	public Vector2 position;

    public GameObject controllerDummy;

    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        worldIntPosition = position;
        return base.StartUp(position, tilemap, go);
    }

    public int StoreOrder = -1;

    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        worldIntPosition = position;
        base.RefreshTile(position, tilemap);
    }

    public void SavePositionInList(NodeMap map)
    {
        SavedPosition = PositionInNodeMap(map);
    }
    public int SavedPosition = -1;

    public void ReleasePosition() { SavedPosition = -1; }


    public Vector3Int worldIntPosition;


    public int PositionInTileMap;



    public int TryGetPositionInNodeMap()
    {
        foreach (TilemapHandler map in UnityEngine.Object.FindObjectsOfType<TilemapHandler>())
        {
            if (map is NodeMap pain)
            {
                if (pain.fuckYou.Contains(this))
                {
                    return pain.fuckYou.FindIndex(map1 => map1 == this);
                }
            }
        }
        return -1;
    }

    public int PositionInNodeMap(NodeMap map)
    {
        return map.fuckYou.FindIndex(map2 => map2 == this);
    }

    public Vector3Int positionFromZeroZero;  




   
}
