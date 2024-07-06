using System;
using System.Collections;
using System.IO;
using AnotherFileBrowser.Windows;
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
		Manager.roomSize = new Vector2Int(34, 24);
		Manager.Reload();
	}


	public static void New()
	{
		Manager.FilePath = null;
		NewRoomMenu.Instance.gameObject.SetActive(true);
	}


	public static void Open()
	{
		var bp = new BrowserProperties();
		bp.filter = "Room files (*.room, *.newroom) | *.room; *.newroom";
		bp.filterIndex = 0;
		bp.title = "Import";
		new FileBrowser().OpenFileBrowser(bp, path =>
		{
			if (!string.IsNullOrEmpty(path))
			{
				Manager.OpeningFile = true;
				Manager.FilePath = path;
				ImportExport.ImportGateKeeper(path);
			}
		});
	}

	public static void UpdateFolder()
	{

		var bp = new FolderBrowserProperties();
		bp.title = "Select folder to upgrade rooms from";
		bp.rootFolder = Environment.SpecialFolder.MyDocuments;
		new FileBrowser().OpenFolderBrowser(bp, path =>
		{
			if (!string.IsNullOrEmpty(path))
			{
				ImportExport.ConvertToNewRoomFormat(path);
			}
		});
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
		var bp = new BrowserProperties();
		bp.filter = "Room files (*.newroom) | *.newroom";
		bp.filterIndex = 0;
		bp.title = "Save As...";
		new FileBrowser().OpenSaveFileBrowser(bp, path =>
		{
            if (!string.IsNullOrEmpty(path))
			{
				if (!path.Contains("."))
				{
					path += ".newroom";
				}
				Manager.FilePath = path;
				FileButton.Save();
			}
		});


		/*
		FileBrowser.OnSuccess OnSuccess = delegate(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				if (!path.Contains("."))
				{
					//path += ".room";
					path += ".newroom";
				}
				Manager.FilePath = path;
				FileButton.Save();
			}
		};
		FileBrowser.ShowSaveDialog(OnSuccess, null, false, FileButton.GetInitialPath(), "Save As...", "Save");*/
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
