using System;
using System.Collections.Generic;
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
				this.redos.Push(curState);
				oldState.map.BuildFromTileArray(oldState.tiles);
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
				this.undos.Push(curState);
				newState.map.BuildFromTileArray(newState.tiles);
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
