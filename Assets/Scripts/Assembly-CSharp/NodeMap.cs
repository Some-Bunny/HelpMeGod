using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityScript.Lang;
using static NodeMap;
using static UnityEngine.EventSystems.EventTrigger;

public class NodeMap : TilemapHandler
{

	public void CollectDataForExport(ref ImportExport.RoomData data, int index, Enums.SerializedPathWrapMode trigger)
	{
		BoundsInt boundsInt = new BoundsInt(TilemapHandler.Bounds.position, TilemapHandler.Bounds.size);
		Tile[,] tiles = base.AllTiles();
		List<string> guids = new List<string>();
		List<Vector2> positions = new List<Vector2>();
		//List<string> attributes = new List<string>();
		List<string> triggers = new List<string>();
		List<int> layers = new List<int>();

        for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (!tiles[x, y]) continue;
				Tile tile = tiles[x, y];
				guids.Add(this.tileDatabase.AllEntries[tile.name]);
				positions.Add(new Vector2((float)x, (float)y));
				triggers.Add(trigger.ToString());
				layers.Add(index);
            }
        }
		data.nodeTypes = data.nodeTypes.Concat(guids.ToArray()).ToArray<string>();
		data.nodePositions = data.nodePositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
		data.nodeWrapModes = data.nodeWrapModes.Concat(triggers.ToArray()).ToArray();
    }


	public class ValueStoragetoPreventHeadaches
	{
		public List<int> placmentOrders = new List<int>();
		public List<Vector2> positions = new List<Vector2>();
        public List<string> tileName = new List<string>();
		public int Order;
    }

	public Vector2? returnTilePositionWorld(Tile[,] tiles, DataTile myTile)
    {
        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (x >= 0 && x < tiles.GetLength(0))
                {
                    if (y >= 0 && y < tiles.GetLength(1))
                    {
                        if (!tiles[x, y]) continue;
                        DataTile tile = tiles[x, y] as DataTile;
                        if (tile.isNode && myTile == tile)
                        {
                            return new Vector2(x, y);
                        }
                    }
                }         
            }
        }
		return null;
    }


	//CHECK HERE



	
    /*
    public void Reset()
    {
        Dictionary<DataTile, DataTile> tilesToUpdate = new Dictionary<DataTile, DataTile>();
        foreach (var entr2y in fuckYou)
        {
            foreach (var entry in fuckYou)
            {
                if (entry.placmentOrder > entr2y.placmentOrder)
                {
                    tilesToUpdate.Add(entry, entr2y);
                }
            }
            foreach (var entry in tilesToUpdate)
            {
                while (entry.Key.placmentOrder > entry.Value.placmentOrder+1)
                {
                    entry.Key.placmentOrder--;
                    entry.Key.data["Node Order"] = JToken.FromObject(entry.Key.placmentOrder.ToString());
                }
            }
        }            
    }
    */
    public void CollectDataForExport2(ref ImportExport.NewRoomData data, int index, Enums.SerializedPathWrapMode trigger, List<DataTile> indexesData, bool isVisible)
	{

        UpdateAtrributeList();
        //DataTile[,] tiles = (DataTile[,])base.AllTiles();
        Tile[,] tiles = base.AllTiles();
		//string[] guids = new string[nodes.Length];
		SortedDictionary<int, ValueStoragetoPreventHeadaches> fuck = new SortedDictionary<int, ValueStoragetoPreventHeadaches>() { };
		//Tuple<int, Vector2> piss = new Tuple<int, Vector2>();
		//Dictionary<int, List<Vector2>> shit = new Dictionary<int, List<Vector2>>() { };

		//	List<Dictionary<int, List<int>>> order_and_placements = new List<Dictionary<int, List<int>>>();
		/*
		List<string> guids = new List<string>();
        //Vector2[] positions = new Vector2[nodes.Length];
		List<Vector2> positions = new List<Vector2>();
		List<int> layers = new List<int>();
		List<int> order = new List<int>();
		*/
		List<string> triggers = new List<string>();
        List<bool> isVisiblly = new List<bool>();

        //List<string> guids = new List<string>();
        //List<Vector2> positions = new List<Vector2>();

        //foreach (var node in nodes)
        //{
        //	guids.Add(this.tileDatabase.AllEntries[node.name]);
        //	positions.Add(node.position);
        //	triggers.Add(trigger.ToString());
        //	layers.Add(index);
        //}
        foreach (var entry in indexesData)
        {
            DataTile tile = entry as DataTile;
            tile.isNode = true;
            Vector2? vector2 = returnTilePositionWorld(tiles, tile);
            if (vector2 != null)
            {
                if (!fuck.ContainsKey(index))
                {
                    fuck.Add(index, new ValueStoragetoPreventHeadaches()
                    {
                        placmentOrders = new List<int>() { tile.PositionInNodeMap(this) },
                        positions = new List<Vector2>() { vector2.Value },
                        tileName = new List<string>() { this.tileDatabase.AllEntries[tile.name] },
                        Order = index
                    }); //new List<int>() { tile.placmentOrder });
                        //Debug.LogError("new index of" + index + ": " + tile.placmentOrder);
                }
                else
                {

                    ValueStoragetoPreventHeadaches pp;
                    fuck.TryGetValue(index, out pp);
                    pp.placmentOrders.Add(tile.PositionInNodeMap(this));
                    Debug.LogWarning(tile.PositionInNodeMap(this));
                    pp.positions.Add(vector2.Value);
                    pp.tileName.Add(this.tileDatabase.AllEntries[tile.name]);
                }
                triggers.Add(trigger.ToString());
                isVisiblly.Add(isVisible);
            }



            //positions.Add(new Vector2((float)x, (float)y));
        }



        List<Vector2> cextors = new List<Vector2> { };
            List<string> asad = new List<string> { };

        foreach (var entry in fuck)
        {
            entry.Value.placmentOrders.Sort();

        }

        foreach (var entry in fuck)
        {
            foreach (var entry2 in entry.Value.placmentOrders)
            {
                cextors.Add(entry.Value.positions[entry2]);
                asad.Add(entry.Value.tileName[entry2]);

                //data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
                //data.nodeOrder =  //data.nodeOrder.Concat().ToArray<int>();
            }
        }


        //entry.Keyint[]
        List<int> indexes = new List<int> { };
            List<int> order = new List<int> { };

            data.nodeTypes = data.nodeTypes.Concat(asad.ToArray()).ToArray<string>();
        //data.nodePositions = data.nodePositions.Concat(positions.ToArray()).ToArray<Vector2>();


        foreach (var entry in fuck)
        {
            foreach (var entry2 in entry.Value.placmentOrders)
            {


                indexes.Add(entry.Key);
                order.Add(entry2);

                //Debug.LogError("added " + entry.Key + " to indexes");
                //Debug.LogError("added " + entry2 + " to order");

                //data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
                //data.nodeOrder =  //data.nodeOrder.Concat().ToArray<int>();
            }
        }


        data.nodePositions = data.nodePositions.Concat(cextors.ToArray()).ToArray<Vector2>();

        data.nodePaths = data.nodePaths.Concat(indexes.ToArray()).ToArray<int>();
        data.nodeOrder = data.nodeOrder.Concat(order.ToArray()).ToArray<int>();
        data.nodeWrapModes = data.nodeWrapModes.Concat(triggers.ToArray()).ToArray();
        data.nodePathVisible = data.nodePathVisible.Concat(isVisiblly.ToArray()).ToArray();


        //var diediediediediediediediedie = 0;
        /*foreach (var cunt in nodes)
        {
			if (cunt == null) continue;
			DataTile tile = cunt as DataTile;

			if (!string.IsNullOrEmpty(this.tileDatabase.AllEntries[tile.name]))
			{

				if (diediediediediediediediedie != tile.placmentOrder - 1) Debug.LogWarning(diediediediediediediediedie + " - " + tile.placmentOrder);

				guids[tile.placmentOrder] = (this.tileDatabase.AllEntries[tile.name]);
				positions[tile.placmentOrder] = cunt.position;
				triggers.Add(trigger.ToString());
				layers.Add(index);
				diediediediediediediediedie = tile.placmentOrder;
			}
		}*/



        /*
		for (int x = 0; x < tiles.GetLength(0); x++)
		{
			var diediediediediediediediedie = 0;
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (tiles[x, y] == null) continue;
				DataTile tile = tiles[x, y] as DataTile;
				
				if (!string.IsNullOrEmpty(this.tileDatabase.AllEntries[tile.name]))
                {

					if (diediediediediediediediedie != tile.placmentOrder - 1) Debug.LogWarning(diediediediediediediediedie + " - " + tile.placmentOrder);

					guids[tile.placmentOrder] = (this.tileDatabase.AllEntries[tile.name]);
					positions[tile.placmentOrder] = (new Vector2((float)x, (float)y));
					triggers.Add(trigger.ToString());
					layers.Add(index);
					diediediediediediediediedie = tile.placmentOrder;
				}

			}
		
		}*/


    }

    public void CollectDataForExport2(ref ImportExport.RoomData data, int index, Enums.SerializedPathWrapMode trigger)
	{

		Tile[,] tiles = base.AllTiles();
		string[] guids = new string[nodes.Length];
		Vector2[] positions = new Vector2[nodes.Length];
		List<string> triggers = new List<string>();
		List<int> layers = new List<int>();

        //foreach (var node in nodes)
        //{
        //	guids.Add(this.tileDatabase.AllEntries[node.name]);
        //	positions.Add(node.position);
        //	triggers.Add(trigger.ToString());
        //	layers.Add(index);
        //}
        for (int x = 0; x < tiles.GetLength(0); x++)
		{
			for (int y = 0; y < tiles.GetLength(1); y++)
			{
				if (!tiles[x, y]) continue;
				DataTile tile = tiles[x, y] as DataTile;
                tile.isNode = true;


                guids[tile.PositionInNodeMap(this)] = (this.tileDatabase.AllEntries[tile.name]);
                positions[tile.PositionInNodeMap(this)] = (new Vector2((float)x, (float)y));
				triggers.Add(trigger.ToString());
				layers.Add(index);
            }
        }

        data.nodeTypes = data.nodeTypes.Concat(guids.ToArray()).ToArray<string>();
		data.nodePositions = data.nodePositions.Concat(positions.ToArray()).ToArray<Vector2>();
		data.nodePaths = data.nodePaths.Concat(layers.ToArray()).ToArray<int>();
        data.nodeWrapModes = data.nodeWrapModes.Concat(triggers.ToArray()).ToArray();

    }



	public void AddNewNodeTile(DataTile tile, Vector3Int position)
    {
		//overrideOrder = -1;
		//Debug.LogError("ORDER:");
        //nodes.Clear();

        //Debug.Log(tile.name);



        /*if (tile == null) return;

		var allTiles = base.AllTiles();
		var tileList = new List<Tile>();
		foreach (var t in allTiles)
        {
			if (t != null) tileList.Add(t);
		}

		nodes = new DataTile[tileList.Count > 0 ? tileList.Count : 1];
		
		foreach (var nTile in allTiles)
        {
			var dataTile = nTile as DataTile;
			if (dataTile == null) continue;

			if (dataTile.placmentOrder >= 0) nodes[dataTile.placmentOrder] = dataTile;
		}

		//if (tile.placmentOrder < 0) tile.placmentOrder = nodes.Length - 1; nodes[tile.placmentOrder] = tile;

		//var fuckYou = nodes.ToList();
		//var fuckYou = new List<DataTile>();*/

        if (tile != null)
		{
            //Debug.Log(position);
            tile.isNode = true;
            var pos = this.GetComponent<Tilemap>().LocalToCell(position);
			tile.position = new Vector2(pos.x, pos.y);
			//Debug.Log(pos);
		} 
		else
        {

            var tileToDelete = fuckYou.Find(item => item != null && item.position == new Vector2(position.x, position.y));
			if (tileToDelete != null)
			{
                /*
                var st = tileToDelete.name ?? "NULL";
                var fadjaik = tileToDelete != null ? tileToDelete.placmentOrder : -999;
                Debug.LogError("Deleted:" + st + " | " + fadjaik);
                List<DataTile> tilesToUpdate = new List<DataTile>();
                foreach (var entry in fuckYou)
                {
					if (entry.placmentOrder > fadjaik)
					{
						tilesToUpdate.Add(entry);
                    }
                }
				foreach (var entry in tilesToUpdate)
				{
					//entry.placmentOrder--;
                }
                */
                fuckYou.Remove(tileToDelete);
                foreach (var entry in fuckYou)
                {
                    entry.data["Node Order"] = entry.PositionInNodeMap(this).ToString();
                }
                UpdateAtrributeList();
                return;
            }
        }

        fuckYou.Add(tile);

        fuckYou.RemoveAll(item => item == null || (item.position == tile?.position && item != tile));

        /*
        if (overrideOrder > 0)
        {
            tile.placmentOrder = overrideOrder;
			fuckYou.Sort();
		}
		else
        {
			foreach (var dTile in fuckYou)
			{
                Debug.Log(dTile.name);

                dTile.pla = fuckYou.IndexOf(dTile);
			}
		}
        */

        //Debug.LogError("J: " + tile.placmentOrder.ToString());

        if (tile != null)
		{
            List<AC> aCs;
            AttributeDatabase.TryGetListing("all_nodes", out aCs);
            foreach (AC ac in aCs)
            {
				if (!tile.data.ContainsKey(ac.longName) && ac.longName == "Node Order") tile.data.Add(ac.longName, tile.PositionInNodeMap(this).ToString());
				else if (!tile.data.ContainsKey(ac.longName))
				{

                    tile.data.Add(ac.longName, JToken.FromObject(tile.name));
                    tile.data[ac.longName] = JToken.FromObject(tile.name);

                   
                }
            }
        }
        //Debug.LogError("J: " + JToken.FromObject(tile.data["nodPos"]).ToString());
        //

        UpdateAtrributeList();
    }

    public void UpdateAtrributeList()
	{
        //Debug.LogError(0);
        List<object> paths2 = new List<object>();
        for (int i = 0; i < this.fuckYou.Count; i++) { 
            //Debug.LogError(i + " : A");
            paths2.Add(i.ToString()); } 
        //Debug.LogError(1);

        AttributeDatabase.allAttributes["nodPos"].possibleValues = paths2.ToArray();
        //AttributeDatabase.allAttributes["nSP_O"].possibleValues = paths2.ToArray();


        foreach (var entry in fuckYou)
        {
            entry.data["Node Order"] = entry.PositionInNodeMap(this).ToString();
        }

        //Debug.LogError(2);
    }


    protected override void InitializeTiles()
    {
        base.InitializeTiles();
        foreach (Tile tile in this.tiles)
        {
            DataTile dataTile = tile as DataTile;
            List<AC> aCs;
            AttributeDatabase.TryGetListing("all_nodes", out aCs);

        }
    }

    public DataTile[] nodes = new DataTile[0];
	public List<DataTile> fuckYou = new List<DataTile>();



    public override TileDatabase InitializeDatabase()
	{
        this.tileDatabase = new TileDatabase();
		this.tileDatabase.Entries = new Dictionary<string, string>
		{
			{
				"Nodes",
				Manager.paletteDividerGuid
			},
			{
				"Center",
				"Center"
			},
			{
				"North",
				"North"
			},
			{
				"NorthEast",
				"NorthEast"
			},
			{
				"East",
				"East"
			},
			{
				"SouthEast",
				"SouthEast"
			},
			{
				"South",
				"South"
			},
			{
				"SouthWest",
				"SouthWest"
			},
			{
				"West",
				"West"
			},
			{
				"NorthWest",
				"NorthWest"
			},
		};
		this.tileDatabase.SubEntries = new Dictionary<string, Dictionary<string, string>>();
		this.tileDatabase.spriteDirectory = "sprites/node";
		return this.tileDatabase;
	}
}
