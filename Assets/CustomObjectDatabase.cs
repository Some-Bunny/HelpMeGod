using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class CustomObjectDatabase : MonoBehaviour
{

    public void Start()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, dataFileName);

        LoadDataFile();      
    }

    public void LoadDataFile()
    {    
        if (!File.Exists(dataFilePath))
        {
            File.Create(dataFilePath).Close();
            File.WriteAllText(dataFilePath, JsonUtility.ToJson(this, true));
        }


        JsonUtility.FromJsonOverwrite(File.ReadAllText(dataFilePath), Instance);
    }

    public void SaveDataFile()
    {
        if (!File.Exists(dataFilePath))
        {
            File.Create(dataFilePath).Close();          
        }
        File.WriteAllText(dataFilePath, JsonUtility.ToJson(this, true));
    }


    public void AddEntry(string name, string guid, Type type)
    {
        (type == Type.Enemy ? customEnemies : customPlaceables).Add(new CustomDataEntry(name, guid));
        SaveDataFile();
    }


    public CustomDataEntry GetEntryByName(string name, Type type)
    {
        var e = (type == Type.Enemy ? customEnemies : customPlaceables).Find(x => x.name == name);

        if (e != null && (type == Type.Enemy ? customEnemies : customPlaceables).Contains(e)) return e;

        return null;
    }

    public CustomDataEntry GetEntryByGUID(string guid, Type type)
    {
        var e = (type == Type.Enemy ? customEnemies : customPlaceables).Find(x => x.guid == guid);

        if (e != null && (type == Type.Enemy ? customEnemies : customPlaceables).Contains(e)) return e;

        return null;
    }

    public void RemoveEntryByName(string name, Type type)
    {
        (type == Type.Enemy ? customEnemies : customPlaceables).Remove(GetEntryByName(name, type));
        SaveDataFile();
    }

    public void RemoveEntryByGUID(string guid, Type type)
    {

        (type == Type.Enemy ? customEnemies : customPlaceables).Remove(GetEntryByGUID(guid, type));
        SaveDataFile();
    }


    public static string dataFileName = "CustomAssetDatabase.json";
    public static string dataFilePath;

    

    public List<CustomDataEntry> customEnemies = new List<CustomDataEntry>();
    public List<CustomDataEntry> customPlaceables = new List<CustomDataEntry>();


    private static CustomObjectDatabase _instance;

    public enum Type
    {
        Enemy,
        Placeable
    }
    
    public static CustomObjectDatabase Instance
    {
        get
        {
            if (!_instance) _instance = UnityEngine.Object.FindObjectOfType<CustomObjectDatabase>();

            return _instance;
        }
        set
        {
            _instance = value;
        }
    }


    [Serializable]
    public class CustomDataEntry
    {
        public CustomDataEntry(string name, string guid)
        {
            this.name = name;
            this.guid = guid;
        }

        public string guid;
        public string name;
    }
}