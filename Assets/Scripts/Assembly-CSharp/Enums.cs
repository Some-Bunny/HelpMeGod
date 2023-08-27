using System;


public static class Enums
{
	 
	public static T GetEnumValue<T>(string val) where T : Enum
	{
		return (T)((object)Enum.Parse(typeof(T), val.ToUpper()));
	}
    public static T GetEnumValueBase<T>(string val) where T : Enum
    {
        return (T)((object)Enum.Parse(typeof(T), val));
    }
    [Serializable]
	public enum SerializedPathWrapMode
	{
		PINGPONG,
		LOOP,
		ONCE
	}


	[Serializable]
	public enum RoomEventTriggerCondition
	{
		ON_ENTER,
		ON_EXIT,
		ON_ENEMIES_CLEARED,
		ON_ENTER_WITH_ENEMIES,
		ON_ONE_QUARTER_ENEMY_HP_DEPLETED,
		ON_HALF_ENEMY_HP_DEPLETED,
		ON_THREE_QUARTERS_ENEMY_HP_DEPLETED,
		ON_NINETY_PERCENT_ENEMY_HP_DEPLETED,
		TIMER,
		SHRINE_WAVE_A,
		SHRINE_WAVE_B,
		SHRINE_WAVE_C,
		NPC_TRIGGER_A,
		NPC_TRIGGER_B,
		NPC_TRIGGER_C,
		ENEMY_BEHAVIOR,
		SEQUENTIAL_WAVE_TRIGGER,
	}


	public enum DungeonMusicState
	{
		FLOOR_INTRO,
		ACTIVE_SIDE_A,
		ACTIVE_SIDE_B,
		ACTIVE_SIDE_C,
		ACTIVE_SIDE_D,
		CALM,
		SHOP,
		SECRET,
		ARCADE,
		FOYER_ELEVATOR,
		FOYER_SORCERESS,
	}

	
	public enum RoomCategory
	{
		
		NORMAL,
		
		SPECIAL,
		
		REWARD,
		
		BOSS,
		
		CONNECTOR,
		
		HUB,
		
		SECRET,
		
		ENTRANCE,
		
		EXIT
	}

	
	public enum RoomNormalSubCategory
	{
		
		COMBAT,
		
		TRAP
	}

	
	public enum RoomBossSubCategory
	{
		
		FLOOR_BOSS,
		
		MINI_BOSS
	}

	
	public enum RoomSpecialSubCategory
	{
		
		UNSPECIFIED_SPECIAL,
		
		STANDARD_SHOP,
		
		WEIRD_SHOP,
		
		MAIN_STORY,
		
		NPC_STORY,
		
		CATACOMBS_BRIDGE_ROOM,
	}

	
	public enum ValidTilesets
	{
		
		GUNGEON,
		
		CASTLEGEON,
		
		SEWERGEON,
		
		CATHEDRALGEON,
		
		MINEGEON,
		
		CATACOMBGEON,
		
		FORGEGEON,
		
		HELLGEON,
		
		SPACEGEON,
		
		PHOBOSGEON512,
		
		WESTGEON,
		
		OFFICEGEON,
		
		BELLYGEON,
		
		JUNGLEGEON,
		
		FINALGEON,
		
		RATGEON
	}

	
	public enum CategoryType
	{
		
		Normal,
		
		Special,
		
		Boss,
		
		Secret
	}

    public enum SuperSpecialRooms
    {
		None,

        Misc_Reward,

        Winchester,

        Fireplace,

		Sewer_Entrance,

		Crest_Room,

		Abbey_Entrance,
		
		Abbey_Extra_Secret,

		Hollow_Sell_Creep,

		Bullet_Hell_Secret,

		Blockner_Miniboss,

		Shadow_Magician,

        Glitched_Boss,

        Gatling_Gull,

        Bullet_King,

        Trigger_Twins,

        Blobulord,

        Gorgun,

        Beholster,

        Ammoconda,

        Old_King,

        Cannonbalrog,

        Mine_Flayer,

        Treadnaught,

        High_Priest,

        Kill_Pillars,

        Wallmonger,

        Door_Lord,

    }
}
