using System;
using System.IO;
using SimpleFileBrowser;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FileButton : DropdownContentButton
{
	
	public static string GetInitialPath()
	{
		string path = null;
		if (!string.IsNullOrEmpty(Manager.FilePath))
		{
			string dir = Path.GetDirectoryName(Manager.FilePath);
			bool flag2 = Directory.Exists(dir);
			if (flag2)
			{
				path = dir;
			}
		}
		return path;
	}

	
	public override void OnClick()
	{
		base.OnClick();
		bool flag = SceneManager.GetActiveScene().name != "MainMenu";
		if (flag)
		{
			base.transform.parent.gameObject.SetActive(false);
		}
		switch (this.action)
		{
		case FileButton.FileAction.New:
			FileButton.New();
			break;
		case FileButton.FileAction.Open:
			FileButton.Open();
			break;
		case FileButton.FileAction.Save:
			FileButton.Save();
			break;
		case FileButton.FileAction.SaveAs:
			FileButton.SaveAs();
			break;
		case FileButton.FileAction.Exit:
			FileButton.Exit();
			break;
		case FileButton.FileAction.Rodent:
			FileButton.NewRodentRoom();
			break;
		case FileButton.FileAction.Update:
			FileButton.UpdateFolder();
			break;
		}
	}

	
	public static void NewRodentRoom()
	{
		Manager.FilePath = null;
		Manager.r0dent = true;
		Manager.roomSize = new Vector2Int(36, 27);
		Manager.Reload();
	}


	public static void New()
	{
		Manager.FilePath = null;
		NewRoomMenu.Instance.gameObject.SetActive(true);
	}


	public static void Open()
	{
		FileBrowser.OnSuccess OnSuccess = delegate(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				Manager.OpeningFile = true;
				Manager.FilePath = path;
				ImportExport.ImportGateKeeper(path);
			}
		};
		FileBrowser.ShowLoadDialog(OnSuccess, null, false, FileButton.GetInitialPath(), "Open", "Select");
	}

	public static void UpdateFolder()
	{
		FileBrowser.OnSuccess OnSuccess = delegate (string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				ImportExport.ConvertToNewRoomFormat(path);
			}
		};
		FileBrowser.ShowLoadDialog(OnSuccess, null, true, FileButton.GetInitialPath(), "Open", "Select");
	}

	public static void Save()
	{
		bool flag = string.IsNullOrEmpty(Manager.FilePath);
		if (flag)
		{
			FileButton.SaveAs();
		}
		else
		{
			ImportExport.Export(Manager.FilePath);
		}
	}

	
	public static void SaveAs()
	{
		FileBrowser.OnSuccess OnSuccess = delegate(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				bool flag2 = !path.Contains(".");
				if (flag2)
				{
					//path += ".room";
					path += ".newroom";
				}
				Manager.FilePath = path;
				FileButton.Save();
			}
		};
		FileBrowser.ShowSaveDialog(OnSuccess, null, false, FileButton.GetInitialPath(), "Save As...", "Save");
	}

	
	public static void Exit()
	{
		Application.Quit(0);
	}

	
	public FileButton.FileAction action;

	
	public enum FileAction
	{
		
		New,
		
		Open,
		
		Save,
		
		SaveAs,
		
		Exit,

		Rodent,

		Update
	}
}
