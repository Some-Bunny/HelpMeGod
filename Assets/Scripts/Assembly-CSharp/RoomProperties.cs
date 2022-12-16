using System;
using System.Collections.Generic;


public class RoomProperties
{
	 
	public RoomProperties()
	{
		foreach (object value in Enum.GetValues(typeof(Enums.ValidTilesets)))
		{
			this.validTilesets.Add(value.ToString(), false);
		}
	}

	public void CollectRoomProperties(ref ImportExport.NewRoomData data)
	{
		data.category = this.category.ToString();
		data.normalSubCategory = this.normalSubCategory.ToString();
		data.bossSubCategory = this.bossSubCategory.ToString();
		data.specialSubCategory = this.specialSubCategory.ToString();
		data.weight = this.weight;
		data.shuffleReinforcementPositions = this.shuffleReinforcementPositions;
		data.darkRoom = this.darkRoom;
		data.doFloorDecoration = this.doFloorDecoration;
		data.doWallDecoration = this.doWallDecoration;
		data.doLighting = this.doLighting;
		List<string> floors = new List<string>();
		foreach (KeyValuePair<string, bool> floor in this.validTilesets)
		{
			bool value = floor.Value;
			if (value)
			{
				floors.Add(floor.Key);
			}
		}
		data.floors = floors.ToArray();
	}


	public void CollectRoomProperties(ref ImportExport.RoomData data)
	{
		data.category = this.category.ToString();
		data.normalSubCategory = this.normalSubCategory.ToString();
		data.bossSubCategory = this.bossSubCategory.ToString();
		data.specialSubCategory = this.specialSubCategory.ToString();
		data.weight = this.weight;
		data.shuffleReinforcementPositions = this.shuffleReinforcementPositions;
		data.darkRoom = this.darkRoom;
		data.doFloorDecoration = this.doFloorDecoration;
		data.doWallDecoration = this.doWallDecoration;
		data.doLighting = this.doLighting;
		List<string> floors = new List<string>();
		foreach (KeyValuePair<string, bool> floor in this.validTilesets)
		{
			bool value = floor.Value;
			if (value)
			{
				floors.Add(floor.Key);
			}
		}
		data.floors = floors.ToArray();
	}

	public void ImportRoomProperties(ImportExport.NewRoomData data)
	{
		bool flag = !string.IsNullOrEmpty(data.category);
		if (flag)
		{
			this.category = Enums.GetEnumValue<Enums.RoomCategory>(data.category);
		}
		bool flag2 = !string.IsNullOrEmpty(data.normalSubCategory);
		if (flag2)
		{
			this.normalSubCategory = Enums.GetEnumValue<Enums.RoomNormalSubCategory>(data.normalSubCategory);
		}
		bool flag3 = !string.IsNullOrEmpty(data.specialSubCategory);
		if (flag3)
		{
			this.specialSubCategory = Enums.GetEnumValue<Enums.RoomSpecialSubCategory>(data.specialSubCategory);
		}
		bool flag4 = !string.IsNullOrEmpty(data.bossSubCategory);
		if (flag4)
		{
			this.bossSubCategory = Enums.GetEnumValue<Enums.RoomBossSubCategory>(data.bossSubCategory);
		}
		bool flag5 = data.floors != null;
		if (flag5)
		{
			foreach (string floor in data.floors)
			{
				bool flag6 = this.validTilesets.ContainsKey(floor);
				if (flag6)
				{
					this.validTilesets[floor] = true;
				}
			}
		}
		this.weight = data.weight;
		this.shuffleReinforcementPositions = data.shuffleReinforcementPositions;
		this.darkRoom = data.darkRoom;
		this.doFloorDecoration = data.doFloorDecoration;
		this.doWallDecoration = data.doWallDecoration;
		this.doLighting = data.doLighting;
	}

	public void ImportRoomProperties(ImportExport.RoomData data)
	{
		bool flag = !string.IsNullOrEmpty(data.category);
		if (flag)
		{
			this.category = Enums.GetEnumValue<Enums.RoomCategory>(data.category);
		}
		bool flag2 = !string.IsNullOrEmpty(data.normalSubCategory);
		if (flag2)
		{
			this.normalSubCategory = Enums.GetEnumValue<Enums.RoomNormalSubCategory>(data.normalSubCategory);
		}
		bool flag3 = !string.IsNullOrEmpty(data.specialSubCategory);
		if (flag3)
		{
			this.specialSubCategory = Enums.GetEnumValue<Enums.RoomSpecialSubCategory>(data.specialSubCategory);
		}
		bool flag4 = !string.IsNullOrEmpty(data.bossSubCategory);
		if (flag4)
		{
			this.bossSubCategory = Enums.GetEnumValue<Enums.RoomBossSubCategory>(data.bossSubCategory);
		}
		bool flag5 = data.floors != null;
		if (flag5)
		{
			foreach (string floor in data.floors)
			{
				bool flag6 = this.validTilesets.ContainsKey(floor);
				if (flag6)
				{
					this.validTilesets[floor] = true;
				}
			}
		}
		this.weight = data.weight;
		this.shuffleReinforcementPositions = data.shuffleReinforcementPositions;
		this.darkRoom = data.darkRoom;
		this.doFloorDecoration = data.doFloorDecoration;
		this.doWallDecoration = data.doWallDecoration;
		this.doLighting = data.doLighting;
	}

	
	public Enums.RoomCategory category;

	
	public Enums.RoomNormalSubCategory normalSubCategory;

	
	public Enums.RoomBossSubCategory bossSubCategory;

	
	public Enums.RoomSpecialSubCategory specialSubCategory;

	
	public bool shuffleReinforcementPositions = false;

	
	public bool darkRoom = false;

	
	public bool doFloorDecoration = true;

	
	public bool doWallDecoration = true;

	
	public bool doLighting = true;

	
	public float weight = 1f;

	
	public Dictionary<string, bool> validTilesets = new Dictionary<string, bool>();
}
