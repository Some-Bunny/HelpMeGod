using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AnotherFileBrowser.Windows;
using UnityEngine.Networking;
using System.IO;

public class ImportCustomEnemyScript : MonoBehaviour
{
    private void Start()
    {
        this.m_hideable = base.GetComponent<HideableObject>();
        ImportCustomEnemyScript.Instance = this;
        base.gameObject.SetActive(false);

    }

    public void OnCloseClicked()
    {
        this.m_hideable.Hide();
    }


    public void OnImportClicked()
    {
        outputName = inputName.text;

        string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        foreach (char c in invalid)
        {
            outputName = outputName.Replace(c.ToString(), "");
        }
        outputName = outputName.ToLower();

        if (isForPlaceables)
        {
            if (File.Exists(Path.Combine(Manager.PlaceableOutputPath, $"{outputName}.png")))
            {
                areYouSurePopup.Show();
            }
            else
            {
                SaveCustomPlaceable();
            }
        }
        else
        {
            if (File.Exists(Path.Combine(Manager.EnemyOutputPath, $"{outputName}.png")))
            {
                areYouSurePopup.Show();
            }
            else
            {
                SaveCustomEnemy();
            }
        }


        Reset();
    }


    public void Reset()
    {
        inputGuid.text = "";
        inputName.text = "";
        importedSpritePath = "";
        outputName = "";
        importedSprite.texture = null;
    }

    public void OverrideOldCustomEnemy()
    {
        RemoveCustomEnemy(outputName);
        SaveCustomEnemy();
    }

    public void OverrideOldCustomPlaceable()
    {
        RemoveCustomPlaceable(outputName);
        SaveCustomPlaceable();
    }

    public void SaveCustomEnemy()
    {
        var map = Manager.Instance.GetTilemap(TilemapHandler.MapType.Enemies);

        File.Copy(importedSpritePath, Path.Combine(Manager.EnemyOutputPath, $"{outputName}.png"));

        CustomObjectDatabase.Instance.AddEntry(outputName, inputGuid.text, CustomObjectDatabase.Type.Enemy);

        map.tileDatabase.Entries.Add($"customEnemyAsset-{outputName}", inputGuid.text);
        map.tiles.Add(map.SetupTile(map.tileDatabase, $"customEnemyAsset-{outputName}", inputGuid.text, Manager.Instance.emptyTile));

        if (PaletteDropdown.Instance.palettes[2].Populated)
        {
            PaletteDropdown.Instance.palettes[2].Depopulate();
            PaletteDropdown.Instance.palettes[2].Populate();
        }
    }

    public void SaveCustomPlaceable()
    {
        var map = Manager.Instance.GetTilemap(TilemapHandler.MapType.Placeables);

        File.Copy(importedSpritePath, Path.Combine(Manager.PlaceableOutputPath, $"{outputName}.png"));;

        CustomObjectDatabase.Instance.AddEntry(outputName, inputGuid.text, CustomObjectDatabase.Type.Placeable);

        map.tileDatabase.Entries.Add($"customPlaceableAsset-{outputName}", inputGuid.text);
        map.tiles.Add(map.SetupTile(map.tileDatabase, $"customPlaceableAsset-{outputName}", inputGuid.text, Manager.Instance.emptyTile));

        if (PaletteDropdown.Instance.palettes[3].Populated)
        {
            PaletteDropdown.Instance.palettes[3].Depopulate();
            PaletteDropdown.Instance.palettes[3].Populate();
        }
    }


    public void OnImportSpriteClicked()
    {
        OpenFileBrowser();
    }

    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
        bp.filterIndex = 0;
        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            //Load image from local path with UWR
            StartCoroutine(LoadImage(path));
        });
    }

    IEnumerator LoadImage(string path)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(path))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                var uwrTexture = DownloadHandlerTexture.GetContent(uwr);
                importedSprite.texture = uwrTexture;
                importedSpritePath = path;
            }
        }
    }

    public void DeleteSelectedCustomAsset()
    {

        if (InputHandler.Instance.selectedTileType.name.Contains("customEnemyAsset-"))
        {
            string nameToDelete = InputHandler.Instance.selectedTileType.name.Replace("customEnemyAsset-", "");

            RemoveCustomEnemy(nameToDelete, true);
        }
        else
        {
            string nameToDelete = InputHandler.Instance.selectedTileType.name.Replace("customPlaceableAsset-", "");

            RemoveCustomPlaceable(nameToDelete, true);
        }

        
    }

    public void RemoveCustomEnemy(string nameToRemove, bool reload = false)
    {
        var map = Manager.Instance.GetTilemap(TilemapHandler.MapType.Enemies);

        File.Delete(Path.Combine(Manager.EnemyOutputPath, $"{nameToRemove}.png"));
        CustomObjectDatabase.Instance.RemoveEntryByName(nameToRemove, CustomObjectDatabase.Type.Enemy);
        if (map.tileDatabase.Entries.ContainsKey($"customEnemyAsset-{nameToRemove}")) map.tileDatabase.Entries.Remove($"customEnemyAsset-{nameToRemove}");

        var t = map.tiles.Find(x => x.name == $"customEnemyAsset-{nameToRemove}");
        if (t) map.tiles.Remove(t);


        if (PaletteDropdown.Instance.palettes[2].Populated && reload)
        {
            PaletteDropdown.Instance.palettes[2].Depopulate();
            PaletteDropdown.Instance.palettes[2].Populate();
        }

        InputHandler.Instance.selectedTileType = null;

        var tiles = map.AllTiles();



        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (!tiles[x, y]) continue;
                if (tiles[x, y].name == $"customEnemyAsset-{nameToRemove}")
                {                  
                    map.map.SetTile(TilemapHandler.GameToLocalPosition(x, y), null);
                }
            }
        }


    }

    public void RemoveCustomPlaceable(string nameToRemove, bool reload = false)
    {
        var map = Manager.Instance.GetTilemap(TilemapHandler.MapType.Placeables);

        File.Delete(Path.Combine(Manager.PlaceableOutputPath, $"{nameToRemove}.png"));
        CustomObjectDatabase.Instance.RemoveEntryByName(nameToRemove, CustomObjectDatabase.Type.Placeable);
        if (map.tileDatabase.Entries.ContainsKey($"customPlaceableAsset-{nameToRemove}")) map.tileDatabase.Entries.Remove($"customPlaceableAsset-{nameToRemove}");

        var t = map.tiles.Find(x => x.name == $"customPlaceableAsset-{nameToRemove}");
        if (t) map.tiles.Remove(t);


        if (PaletteDropdown.Instance.palettes[3].Populated && reload)
        {
            PaletteDropdown.Instance.palettes[3].Depopulate();
            PaletteDropdown.Instance.palettes[3].Populate();
        }

        InputHandler.Instance.selectedTileType = null;

        var tiles = map.AllTiles();



        for (int x = 0; x < tiles.GetLength(0); x++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (!tiles[x, y]) continue;
                if (tiles[x, y].name == $"customPlaceableAsset-{nameToRemove}")
                {
                    map.map.SetTile(TilemapHandler.GameToLocalPosition(x, y), null);
                }
            }
        }


    }

    public string prefix;

    public bool isForPlaceables;

    public RawImage importedSprite;

    string importedSpritePath;

    string outputName;

    public InputField inputGuid;
    public InputField inputName;
        
    public static ImportCustomEnemyScript Instance;

    public HideableObject areYouSurePopup;

    private HideableObject m_hideable;
}


