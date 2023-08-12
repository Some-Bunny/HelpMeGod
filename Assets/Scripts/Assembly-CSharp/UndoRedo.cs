using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class UndoRedo
{
	
	public void RegisterState(TilemapHandler map, string eventName)
	{
		UndoRedo.State state = new UndoRedo.State
		{
			map = map,
			tiles = map.AllTiles(),
			eventName = eventName
		};
		this.undos.Push(state);
		this.redos.Clear();
	}

	
	public void Undo()
	{
		bool flag = this.undos.Count == 0;
		if (!flag)
		{
			UndoRedo.State oldState = this.undos.Pop();
			bool flag2 = oldState.map == null;
			if (!flag2)
			{

                UndoRedo.State curState = new UndoRedo.State
				{
					map = oldState.map,
					tiles = oldState.map.AllTiles(),
					eventName = oldState.eventName
				};

                if (curState.map is NodeMap pp)
				{
                    /*
                    foreach (var t in oldState.tiles)
                    {
                        if (t as DataTile == null)
                        {
                            Debug.Log("NULL");
                        }
                        else
                        {
                            //Debug.LogError((t as DataTile).worldIntPosition + " : " + (t as DataTile).PositionInNodeMap(pp));
                        }
                    }
                    Debug.LogError("======Current");
                    */
                    List<int> c = new List<int>();
                    Dictionary<int, DataTile> c2 = new Dictionary<int, DataTile>();


                    foreach (var t in oldState.tiles)
                    {
                        if (t as DataTile == null)
                        {

                        }
                        else
                        {
                            c.Add((t as DataTile).PositionInNodeMap(pp));
                            c2.Add((t as DataTile).PositionInNodeMap(pp), (t as DataTile));
                        }
                    }
                    pp.fuckYou.Clear();

                    c.Sort();

                    this.redos.Push(curState);
                    curState.map.map.ClearAllTiles();
                    for (int count = 0; count < c.Count; count++)
                    {
                        int help = c[count];
                        DataTile tiler;
                        c2.TryGetValue(help, out tiler);

                        curState.map.map.SetTile(tiler.worldIntPosition, tiler);
                        (curState.map as NodeMap).AddNewNodeTile(tiler, tiler.worldIntPosition);
                    }
                }
                else
				{
                    this.redos.Push(curState);
                    curState.map.BuildFromTileArray(oldState.tiles);
                }
            }
		}
	}

	
	public void Redo()
	{
		bool flag = this.redos.Count == 0;
		if (!flag)
		{

			UndoRedo.State newState = this.redos.Pop();
			bool flag2 = newState.map == null;
			if (!flag2)
			{

                UndoRedo.State curState = new UndoRedo.State
				{
					map = newState.map,
					tiles = newState.map.AllTiles(),
					eventName = newState.eventName
				};


                if (newState.map is NodeMap pp)
                {
                    List<int> c = new List<int>();
                    Dictionary<int, DataTile> c2 = new Dictionary<int, DataTile>();


                    foreach (var t in newState.tiles)
                    {
                        if (t as DataTile == null)
                        {
                        }
                        else
                        {
                            c.Add((t as DataTile).PositionInNodeMap(pp));
                            c2.Add((t as DataTile).PositionInNodeMap(pp), (t as DataTile));
                        }
                    }
                    pp.fuckYou.Clear();

                    c.Sort();

                    this.redos.Push(curState);
                    curState.map.map.ClearAllTiles();
                    for (int count = 0; count < c.Count; count++)
                    {
                        int help = c[count];
                        DataTile tiler;
                        c2.TryGetValue(help, out tiler);

                        curState.map.map.SetTile(tiler.worldIntPosition, tiler);
                        (curState.map as NodeMap).AddNewNodeTile(tiler, tiler.worldIntPosition);
                    }

                    /*
                    oldState.map.map.SetTile(pp.fuckYou.Last().worldIntPosition, null);
                    oldState.map.BuildFromTileArray(oldState.tiles);
                    pp.UpdateAtrributeList();
                    this.redos.Push(curState);
                    */
                }
                else
                {
                    this.redos.Push(curState);
                    curState.map.BuildFromTileArray(newState.tiles);
                }
                /*
                if (newState.map is NodeMap pp)
                {
                    newState.map.map.SetTile(pp.fuckYou.Last().worldIntPosition, null);
                    newState.map.BuildFromTileArray(newState.tiles);
                    pp.UpdateAtrributeList();
                    this.redos.Push(curState);
                }
                else
                {
                    this.redos.Push(curState);
                    newState.map.BuildFromTileArray(newState.tiles);
                }
                */

                /*
                var map = curState.map;
                if (map is NodeMap pain)
                {
                    pain.UpdateAtrributeList();
					
                }
				*/

                /*
                if (curState.map is NodeMap pain)
                {
                    TileBase[] tiles = curState.map.map.GetTilesBlock(pain.map.cellBounds);
                    curState.map.map.ClearAllTiles();
                    curState.map.ResizeBounds();
                    curState.map.map.SetTilesBlock(pain.map.cellBounds, tiles);

                    pain.fuckYou.Clear();
                    Dictionary<int, DataTile> v = new Dictionary<int, DataTile>();
                    List<int> P = new List<int>();
                    foreach (var tile in tiles)
                    {
                        if (tile is DataTile data)
                        {
                            if (data.isNode == true)
                            {
                                if (v.ContainsKey(data.placmentOrder)) { while (v.ContainsKey(data.placmentOrder)) { data.placmentOrder += 1; } }
                                v.Add(data.placmentOrder, data);
                                P.Add(data.placmentOrder);
                            }
                        }
                    }
                    P.Sort();

                    foreach (var pnis in P)
                    {
                        DataTile h;
                        v.TryGetValue(pnis, out h);
                        h.placmentOrder = pnis;
                        pain.fuckYou.Add(h);
                    }
                    pain.UpdateAtrributeList();

                   
                    foreach (var enrty in v)
                    {
                        pain.fuckYou.Add(enrty.Value);
                    }
                    *

                }
                */
            }
		}
	}

	
	public Stack<UndoRedo.State> undos = new Stack<UndoRedo.State>();

	
	public Stack<UndoRedo.State> redos = new Stack<UndoRedo.State>();

	
	public struct State
	{
		
		public TilemapHandler map;

		
		public Tile[,] tiles;

		
		public string eventName;
	}
}
