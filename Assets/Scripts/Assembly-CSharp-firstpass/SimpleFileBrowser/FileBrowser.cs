using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleFileBrowser
{
	
	public class FileBrowser : MonoBehaviour, IListViewAdapter
	{
		

		 
		public static bool IsOpen { get; private set; }

		

		 
		public static bool Success { get; private set; }

		

		 
		public static string Result { get; private set; }

		

		 
		public static bool AskPermissions
		{
			get
			{
				return FileBrowser.m_askPermissions;
			}
			set
			{
				FileBrowser.m_askPermissions = value;
			}
		}

		

		 
		public static bool SingleClickMode
		{
			get
			{
				return FileBrowser.m_singleClickMode;
			}
			set
			{
				FileBrowser.m_singleClickMode = value;
			}
		}

		

		private static FileBrowser Instance
		{
			get
			{
				bool flag = FileBrowser.m_instance == null;
				if (flag)
				{
					FileBrowser.m_instance = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("SimpleFileBrowserCanvas")).GetComponent<FileBrowser>();
					UnityEngine.Object.DontDestroyOnLoad(FileBrowser.m_instance.gameObject);
					FileBrowser.m_instance.gameObject.SetActive(false);
				}
				return FileBrowser.m_instance;
			}
		}

		

		 
		private string CurrentPath
		{
			get
			{
				return this.m_currentPath;
			}
			set
			{
				bool flag = value != null;
				if (flag)
				{
					value = this.GetPathWithoutTrailingDirectorySeparator(value.Trim());
				}
				bool flag2 = value == null;
				if (!flag2)
				{
					bool flag3 = this.m_currentPath != value;
					if (flag3)
					{
						bool flag4 = !FileBrowserHelpers.DirectoryExists(value);
						if (flag4)
						{
							return;
						}
						this.m_currentPath = value;
						this.pathInputField.text = this.m_currentPath;
						bool flag5 = this.currentPathIndex == -1 || this.pathsFollowed[this.currentPathIndex] != this.m_currentPath;
						if (flag5)
						{
							this.currentPathIndex++;
							bool flag6 = this.currentPathIndex < this.pathsFollowed.Count;
							if (flag6)
							{
								this.pathsFollowed[this.currentPathIndex] = value;
								for (int i = this.pathsFollowed.Count - 1; i >= this.currentPathIndex + 1; i--)
								{
									this.pathsFollowed.RemoveAt(i);
								}
							}
							else
							{
								this.pathsFollowed.Add(this.m_currentPath);
							}
						}
						this.backButton.interactable = (this.currentPathIndex > 0);
						this.forwardButton.interactable = (this.currentPathIndex < this.pathsFollowed.Count - 1);
						this.upButton.interactable = (Directory.GetParent(this.m_currentPath) != null);
						this.m_searchString = string.Empty;
						this.searchInputField.text = this.m_searchString;
						this.filesScrollRect.verticalNormalizedPosition = 1f;
						this.filenameImage.color = Color.white;
						bool folderSelectMode = this.m_folderSelectMode;
						if (folderSelectMode)
						{
							this.filenameInputField.text = string.Empty;
						}
					}
					this.RefreshFiles(true);
				}
			}
		}

		

		 
		private string SearchString
		{
			get
			{
				return this.m_searchString;
			}
			set
			{
				bool flag = this.m_searchString != value;
				if (flag)
				{
					this.m_searchString = value;
					this.searchInputField.text = this.m_searchString;
					this.RefreshFiles(false);
				}
			}
		}

		

		public int SelectedFilePosition
		{
			get
			{
				return this.m_selectedFilePosition;
			}
		}

		

		 
		private FileBrowserItem SelectedFile
		{
			get
			{
				return this.m_selectedFile;
			}
			set
			{
				bool flag = value == null;
				if (flag)
				{
					bool flag2 = this.m_selectedFile != null;
					if (flag2)
					{
						this.m_selectedFile.Deselect();
					}
					this.m_selectedFilePosition = -1;
					this.m_selectedFile = null;
				}
				else
				{
					bool flag3 = this.m_selectedFilePosition != value.Position;
					if (flag3)
					{
						bool flag4 = this.m_selectedFile != null;
						if (flag4)
						{
							this.m_selectedFile.Deselect();
						}
						this.m_selectedFile = value;
						this.m_selectedFilePosition = value.Position;
						bool flag5 = this.m_folderSelectMode || !this.m_selectedFile.IsDirectory;
						if (flag5)
						{
							this.filenameInputField.text = this.m_selectedFile.Name;
						}
						this.m_selectedFile.Select();
					}
				}
			}
		}

		

		 
		private bool AcceptNonExistingFilename
		{
			get
			{
				return this.m_acceptNonExistingFilename;
			}
			set
			{
				this.m_acceptNonExistingFilename = value;
			}
		}

		

		 
		private bool FolderSelectMode
		{
			get
			{
				return this.m_folderSelectMode;
			}
			set
			{
				bool flag = this.m_folderSelectMode != value;
				if (flag)
				{
					this.m_folderSelectMode = value;
					bool folderSelectMode = this.m_folderSelectMode;
					if (folderSelectMode)
					{
						this.filtersDropdown.options[0].text = "Folders";
						this.filtersDropdown.value = 0;
						this.filtersDropdown.RefreshShownValue();
						this.filtersDropdown.interactable = false;
					}
					else
					{
						this.filtersDropdown.options[0].text = this.filters[0].ToString();
						this.filtersDropdown.interactable = true;
					}
					Text placeholder = this.filenameInputField.placeholder as Text;
					bool flag2 = placeholder != null;
					if (flag2)
					{
						placeholder.text = (this.m_folderSelectMode ? "" : "Filename");
					}
				}
			}
		}

		

		 
		private string Title
		{
			get
			{
				return this.titleText.text;
			}
			set
			{
				this.titleText.text = value;
			}
		}

		

		 
		private string SubmitButtonText
		{
			get
			{
				return this.submitButtonText.text;
			}
			set
			{
				this.submitButtonText.text = value;
			}
		}

		 
		private void Awake()
		{
			FileBrowser.m_instance = this;
			this.rectTransform = (RectTransform)base.transform;
			this.windowTR = (RectTransform)this.window.transform;
			this.ItemHeight = ((RectTransform)this.itemPrefab.transform).sizeDelta.y;
			this.nullPointerEventData = new PointerEventData(null);
			this.DEFAULT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			this.InitializeFiletypeIcons();
			this.filetypeIcons = null;
			FileBrowser.SetExcludedExtensions(this.excludeExtensions);
			this.excludeExtensions = null;
			this.backButton.interactable = false;
			this.forwardButton.interactable = false;
			this.upButton.interactable = false;
			InputField inputField = this.filenameInputField;
			inputField.onValidateInput = (InputField.OnValidateInput)Delegate.Combine(inputField.onValidateInput, new InputField.OnValidateInput(this.OnValidateFilenameInput));
			this.allFilesFilter = new FileBrowser.Filter("All Files (.*)");
			this.filters.Add(this.allFilesFilter);
			this.window.Initialize(this);
			this.listView.SetAdapter(this);
		}

		 
		private void OnRectTransformDimensionsChange()
		{
			this.canvasDimensionsChanged = true;
		}

		 
		private void LateUpdate()
		{
			bool flag = this.canvasDimensionsChanged;
			if (flag)
			{
				this.canvasDimensionsChanged = false;
				this.EnsureWindowIsWithinBounds();
			}
		}

		 
		private void OnApplicationFocus(bool focus)
		{
			if (focus)
			{
				this.RefreshFiles(true);
			}
		}

		

		 
		public OnItemClickedHandler OnItemClicked
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		

		public int Count
		{
			get
			{
				return this.validFileEntries.Count;
			}
		}

		

		 
		public float ItemHeight { get; private set; }

		 
		public ListItem CreateItem()
		{
			FileBrowserItem item = UnityEngine.Object.Instantiate<FileBrowserItem>(this.itemPrefab, this.filesContainer, false);
			item.SetFileBrowser(this);
			return item;
		}

		 
		public void SetItemContent(ListItem item)
		{
			FileBrowserItem file = (FileBrowserItem)item;
			FileSystemEntry fileInfo = this.validFileEntries[item.Position];
			bool isDirectory = fileInfo.IsDirectory;
			bool flag = isDirectory;
			Sprite icon;
			if (flag)
			{
				icon = this.folderIcon;
			}
			else
			{
				bool flag2 = !this.filetypeToIcon.TryGetValue(fileInfo.Extension.ToLowerInvariant(), out icon);
				if (flag2)
				{
					icon = this.defaultIcon;
				}
			}
			file.SetFile(icon, fileInfo.Name, isDirectory);
			file.SetHidden((fileInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden);
			bool flag3 = item.Position == this.m_selectedFilePosition;
			if (flag3)
			{
				this.m_selectedFile = file;
				file.Select();
			}
			else
			{
				file.Deselect();
			}
		}

		 
		private void InitializeFiletypeIcons()
		{
			this.filetypeToIcon = new Dictionary<string, Sprite>();
			for (int i = 0; i < this.filetypeIcons.Length; i++)
			{
				FileBrowser.FiletypeIcon thisIcon = this.filetypeIcons[i];
				this.filetypeToIcon[thisIcon.extension] = thisIcon.icon;
			}
		}

		 
		private void InitializeQuickLinks()
		{
			Vector2 anchoredPos = new Vector2(0f, -this.quickLinksContainer.sizeDelta.y);
			bool flag = this.generateQuickLinksForDrives;
			if (flag)
			{
				string[] drives = Directory.GetLogicalDrives();
				for (int i = 0; i < drives.Length; i++)
				{
					this.AddQuickLink(this.driveIcon, drives[i], drives[i], ref anchoredPos);
				}
			}
			for (int j = 0; j < this.quickLinks.Length; j++)
			{
				FileBrowser.QuickLink quickLink = this.quickLinks[j];
				string quickLinkPath = Environment.GetFolderPath(quickLink.target);
				this.AddQuickLink(quickLink.icon, quickLink.name, quickLinkPath, ref anchoredPos);
			}
			this.quickLinks = null;
			this.quickLinksContainer.sizeDelta = new Vector2(0f, -anchoredPos.y);
		}

		 
		public void OnBackButtonPressed()
		{
			bool flag = this.currentPathIndex > 0;
			if (flag)
			{
				List<string> list = this.pathsFollowed;
				int index = this.currentPathIndex - 1;
				this.currentPathIndex = index;
				this.CurrentPath = list[index];
			}
		}

		 
		public void OnForwardButtonPressed()
		{
			bool flag = this.currentPathIndex < this.pathsFollowed.Count - 1;
			if (flag)
			{
				List<string> list = this.pathsFollowed;
				int index = this.currentPathIndex + 1;
				this.currentPathIndex = index;
				this.CurrentPath = list[index];
			}
		}

		 
		public void OnUpButtonPressed()
		{
			DirectoryInfo parentPath = Directory.GetParent(this.m_currentPath);
			bool flag = parentPath != null;
			if (flag)
			{
				this.CurrentPath = parentPath.FullName;
			}
		}

		 
		public void OnSubmitButtonClicked()
		{
			string path = this.m_currentPath;
			string filenameInput = this.filenameInputField.text.Trim();
			bool flag = filenameInput.Length > 0;
			if (flag)
			{
				path = Path.Combine(path, filenameInput);
			}
			bool flag2 = File.Exists(path);
			if (flag2)
			{
				bool flag3 = !this.m_folderSelectMode;
				if (flag3)
				{
					this.OnOperationSuccessful(path);
				}
				else
				{
					this.filenameImage.color = this.wrongFilenameColor;
				}
			}
			else
			{
				bool flag4 = Directory.Exists(path);
				if (flag4)
				{
					bool folderSelectMode = this.m_folderSelectMode;
					if (folderSelectMode)
					{
						this.OnOperationSuccessful(path);
					}
					else
					{
						bool flag5 = this.m_currentPath == path;
						if (flag5)
						{
							this.filenameImage.color = this.wrongFilenameColor;
						}
						else
						{
							this.CurrentPath = path;
						}
					}
				}
				else
				{
					bool acceptNonExistingFilename = this.m_acceptNonExistingFilename;
					if (acceptNonExistingFilename)
					{
						bool flag6 = !this.m_folderSelectMode && this.filters[this.filtersDropdown.value].defaultExtension != null;
						if (flag6)
						{
							path = Path.ChangeExtension(path, this.filters[this.filtersDropdown.value].defaultExtension);
						}
						this.OnOperationSuccessful(path);
					}
					else
					{
						this.filenameImage.color = this.wrongFilenameColor;
					}
				}
			}
		}

		 
		public void OnCancelButtonClicked()
		{
			this.OnOperationCanceled(true);
		}

		 
		private void OnOperationSuccessful(string path)
		{
			FileBrowser.Success = true;
			FileBrowser.Result = path;
			this.Hide();
			FileBrowser.OnSuccess _onSuccess = this.onSuccess;
			this.onSuccess = null;
			this.onCancel = null;
			bool flag = _onSuccess != null;
			if (flag)
			{
				_onSuccess(path);
			}
		}

		 
		private void OnOperationCanceled(bool invokeCancelCallback)
		{
			FileBrowser.Success = false;
			FileBrowser.Result = null;
			this.Hide();
			FileBrowser.OnCancel _onCancel = this.onCancel;
			this.onSuccess = null;
			this.onCancel = null;
			bool flag = invokeCancelCallback && _onCancel != null;
			if (flag)
			{
				_onCancel();
			}
		}

		 
		public void OnPathChanged(string newPath)
		{
			this.CurrentPath = newPath;
		}

		 
		public void OnSearchStringChanged(string newSearchString)
		{
			this.SearchString = newSearchString;
		}

		 
		public void OnFilterChanged()
		{
			this.RefreshFiles(false);
		}

		 
		public void OnShowHiddenFilesToggleChanged()
		{
			this.RefreshFiles(false);
		}

		 
		public void OnQuickLinkSelected(FileBrowserQuickLink quickLink)
		{
			bool flag = quickLink != null;
			if (flag)
			{
				this.CurrentPath = quickLink.TargetPath;
			}
		}

		 
		public void OnItemSelected(FileBrowserItem item)
		{
			this.SelectedFile = item;
		}

		 
		public void OnItemOpened(FileBrowserItem item)
		{
			bool isDirectory = item.IsDirectory;
			if (isDirectory)
			{
				this.CurrentPath = Path.Combine(this.m_currentPath, item.Name);
			}
			else
			{
				this.OnSubmitButtonClicked();
			}
		}

		 
		public char OnValidateFilenameInput(string text, int charIndex, char addedChar)
		{
			bool flag = addedChar == '\n';
			char result;
			if (flag)
			{
				this.OnSubmitButtonClicked();
				result = '\0';
			}
			else
			{
				result = addedChar;
			}
			return result;
		}

		 
		public void Show(string initialPath)
		{
			bool askPermissions = FileBrowser.AskPermissions;
			if (askPermissions)
			{
				FileBrowser.RequestPermission();
			}
			bool flag = !FileBrowser.quickLinksInitialized;
			if (flag)
			{
				FileBrowser.quickLinksInitialized = true;
				this.InitializeQuickLinks();
			}
			this.SelectedFile = null;
			this.m_searchString = string.Empty;
			this.searchInputField.text = this.m_searchString;
			this.filesScrollRect.verticalNormalizedPosition = 1f;
			this.filenameInputField.text = string.Empty;
			this.filenameImage.color = Color.white;
			FileBrowser.IsOpen = true;
			FileBrowser.Success = false;
			FileBrowser.Result = null;
			base.gameObject.SetActive(true);
			this.CurrentPath = this.GetInitialPath(initialPath);
		}

		 
		public void Hide()
		{
			FileBrowser.IsOpen = false;
			this.currentPathIndex = -1;
			this.pathsFollowed.Clear();
			this.backButton.interactable = false;
			this.forwardButton.interactable = false;
			this.upButton.interactable = false;
			base.gameObject.SetActive(false);
		}

		 
		public void RefreshFiles(bool pathChanged)
		{
			if (pathChanged)
			{
				bool flag = !string.IsNullOrEmpty(this.m_currentPath);
				if (flag)
				{
					this.allFileEntries = FileBrowserHelpers.GetEntriesInDirectory(this.m_currentPath);
				}
				else
				{
					this.allFileEntries = null;
				}
			}
			this.SelectedFile = null;
			bool flag2 = !this.showHiddenFilesToggle.isOn;
			if (flag2)
			{
				this.ignoredFileAttributes |= FileAttributes.Hidden;
			}
			else
			{
				this.ignoredFileAttributes &= ~FileAttributes.Hidden;
			}
			string searchStringLowercase = this.m_searchString.ToLower();
			this.validFileEntries.Clear();
			bool flag3 = this.allFileEntries != null;
			if (flag3)
			{
				int i = 0;
				while (i < this.allFileEntries.Length)
				{
					try
					{
						FileSystemEntry item = this.allFileEntries[i];
						bool flag4 = !item.IsDirectory;
						if (flag4)
						{
							bool folderSelectMode = this.m_folderSelectMode;
							if (folderSelectMode)
							{
								goto IL_1B6;
							}
							bool flag5 = (item.Attributes & this.ignoredFileAttributes) > (FileAttributes)0;
							if (flag5)
							{
								goto IL_1B6;
							}
							string extension = item.Extension.ToLowerInvariant();
							bool flag6 = this.excludedExtensionsSet.Contains(extension);
							if (flag6)
							{
								goto IL_1B6;
							}
							HashSet<string> extensions = this.filters[this.filtersDropdown.value].extensions;
							bool flag7 = extensions != null && !extensions.Contains(extension);
							if (flag7)
							{
								goto IL_1B6;
							}
						}
						else
						{
							bool flag8 = (item.Attributes & this.ignoredFileAttributes) > (FileAttributes)0;
							if (flag8)
							{
								goto IL_1B6;
							}
						}
						bool flag9 = this.m_searchString.Length == 0 || item.Name.ToLower().Contains(searchStringLowercase);
						if (flag9)
						{
							this.validFileEntries.Add(item);
						}
					}
					catch (Exception e)
					{
						Debug.LogException(e);
					}
					IL_1B6:
					i++;
					continue;
					goto IL_1B6;
				}
			}
			this.listView.UpdateList();
			this.filesScrollRect.OnScroll(this.nullPointerEventData);
		}

		 
		private bool AddQuickLink(Sprite icon, string name, string path, ref Vector2 anchoredPos)
		{
			bool flag = string.IsNullOrEmpty(path);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !Directory.Exists(path);
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = this.addedQuickLinksSet.Contains(path);
					if (flag3)
					{
						result = false;
					}
					else
					{
						FileBrowserQuickLink quickLink = UnityEngine.Object.Instantiate<FileBrowserQuickLink>(this.quickLinkPrefab, this.quickLinksContainer, false);
						quickLink.SetFileBrowser(this);
						bool flag4 = icon != null;
						if (flag4)
						{
							quickLink.SetQuickLink(icon, name, path);
						}
						else
						{
							quickLink.SetQuickLink(this.folderIcon, name, path);
						}
						quickLink.TransformComponent.anchoredPosition = anchoredPos;
						anchoredPos.y -= this.ItemHeight;
						this.addedQuickLinksSet.Add(path);
						result = true;
					}
				}
			}
			return result;
		}

		 
		public void EnsureWindowIsWithinBounds()
		{
			Vector2 canvasSize = this.rectTransform.sizeDelta;
			Vector2 windowSize = this.windowTR.sizeDelta;
			bool flag = windowSize.x > canvasSize.x;
			if (flag)
			{
				windowSize.x = canvasSize.x;
			}
			bool flag2 = windowSize.y > canvasSize.y;
			if (flag2)
			{
				windowSize.y = canvasSize.y;
			}
			Vector2 windowPos = this.windowTR.anchoredPosition;
			Vector2 canvasHalfSize = canvasSize * 0.5f;
			Vector2 windowHalfSize = windowSize * 0.5f;
			Vector2 windowBottomLeft = windowPos - windowHalfSize + canvasHalfSize;
			Vector2 windowTopRight = windowPos + windowHalfSize + canvasHalfSize;
			bool flag3 = windowBottomLeft.x < 0f;
			if (flag3)
			{
				windowPos.x -= windowBottomLeft.x;
			}
			else
			{
				bool flag4 = windowTopRight.x > canvasSize.x;
				if (flag4)
				{
					windowPos.x -= windowTopRight.x - canvasSize.x;
				}
			}
			bool flag5 = windowBottomLeft.y < 0f;
			if (flag5)
			{
				windowPos.y -= windowBottomLeft.y;
			}
			else
			{
				bool flag6 = windowTopRight.y > canvasSize.y;
				if (flag6)
				{
					windowPos.y -= windowTopRight.y - canvasSize.y;
				}
			}
			this.windowTR.anchoredPosition = windowPos;
			this.windowTR.sizeDelta = windowSize;
		}

		 
		private string GetPathWithoutTrailingDirectorySeparator(string path)
		{
			bool flag = string.IsNullOrEmpty(path);
			string result;
			if (flag)
			{
				result = null;
			}
			else
			{
				try
				{
					bool flag2 = Path.GetDirectoryName(path) != null;
					if (flag2)
					{
						char lastChar = path[path.Length - 1];
						bool flag3 = lastChar == Path.DirectorySeparatorChar || lastChar == Path.AltDirectorySeparatorChar;
						if (flag3)
						{
							path = path.Substring(0, path.Length - 1);
						}
					}
				}
				catch
				{
					return null;
				}
				result = path;
			}
			return result;
		}

		 
		private int CalculateLengthOfDropdownText(string str)
		{
			int totalLength = 0;
			Font myFont = this.filterItemTemplate.font;
			CharacterInfo characterInfo = default(CharacterInfo);
			myFont.RequestCharactersInTexture(str, this.filterItemTemplate.fontSize, this.filterItemTemplate.fontStyle);
			for (int i = 0; i < str.Length; i++)
			{
				bool flag = !myFont.GetCharacterInfo(str[i], out characterInfo, this.filterItemTemplate.fontSize);
				if (flag)
				{
					totalLength += 5;
				}
				totalLength += characterInfo.advance;
			}
			return totalLength;
		}

		 
		private string GetInitialPath(string initialPath)
		{
			bool flag = string.IsNullOrEmpty(initialPath) || !Directory.Exists(initialPath);
			if (flag)
			{
				bool flag2 = this.CurrentPath.Length == 0;
				if (flag2)
				{
					initialPath = this.DEFAULT_PATH;
				}
				else
				{
					initialPath = this.CurrentPath;
				}
			}
			this.m_currentPath = string.Empty;
			return initialPath;
		}

		 
		public static bool ShowSaveDialog(FileBrowser.OnSuccess onSuccess, FileBrowser.OnCancel onCancel, bool folderMode = false, string initialPath = null, string title = "Save", string saveButtonText = "Save")
		{
			bool activeSelf = FileBrowser.Instance.gameObject.activeSelf;
			bool result;
			if (activeSelf)
			{
				Debug.LogError("Error: Multiple dialogs are not allowed!");
				result = false;
			}
			else
			{
				FileBrowser.Instance.onSuccess = onSuccess;
				FileBrowser.Instance.onCancel = onCancel;
				FileBrowser.Instance.FolderSelectMode = folderMode;
				FileBrowser.Instance.Title = title;
				FileBrowser.Instance.SubmitButtonText = saveButtonText;
				FileBrowser.Instance.AcceptNonExistingFilename = !folderMode;
				FileBrowser.Instance.Show(initialPath);
				result = true;
			}
			return result;
		}

		 
		public static bool ShowLoadDialog(FileBrowser.OnSuccess onSuccess, FileBrowser.OnCancel onCancel, bool folderMode = false, string initialPath = null, string title = "Load", string loadButtonText = "Select")
		{
			bool activeSelf = FileBrowser.Instance.gameObject.activeSelf;
			bool result;
			if (activeSelf)
			{
				Debug.LogError("Error: Multiple dialogs are not allowed!");
				result = false;
			}
			else
			{
				FileBrowser.Instance.onSuccess = onSuccess;
				FileBrowser.Instance.onCancel = onCancel;
				FileBrowser.Instance.FolderSelectMode = folderMode;
				FileBrowser.Instance.Title = title;
				FileBrowser.Instance.SubmitButtonText = loadButtonText;
				FileBrowser.Instance.AcceptNonExistingFilename = false;
				FileBrowser.Instance.Show(initialPath);
				result = true;
			}
			return result;
		}

		 
		public static void HideDialog(bool invokeCancelCallback = false)
		{
			FileBrowser.Instance.OnOperationCanceled(invokeCancelCallback);
		}

		 
		public static IEnumerator WaitForSaveDialog(bool folderMode = false, string initialPath = null, string title = "Save", string saveButtonText = "Save")
		{
			bool flag = !FileBrowser.ShowSaveDialog(null, null, folderMode, initialPath, title, saveButtonText);
			if (flag)
			{
				yield break;
			}
			while (FileBrowser.Instance.gameObject.activeSelf)
			{
				yield return null;
			}
			yield break;
		}

		 
		public static IEnumerator WaitForLoadDialog(bool folderMode = false, string initialPath = null, string title = "Load", string loadButtonText = "Select")
		{
			bool flag = !FileBrowser.ShowLoadDialog(null, null, folderMode, initialPath, title, loadButtonText);
			if (flag)
			{
				yield break;
			}
			while (FileBrowser.Instance.gameObject.activeSelf)
			{
				yield return null;
			}
			yield break;
		}

		 
		public static bool AddQuickLink(string name, string path, Sprite icon = null)
		{
			bool flag = !FileBrowser.quickLinksInitialized;
			if (flag)
			{
				FileBrowser.quickLinksInitialized = true;
				bool askPermissions = FileBrowser.AskPermissions;
				if (askPermissions)
				{
					FileBrowser.RequestPermission();
				}
				FileBrowser.Instance.InitializeQuickLinks();
			}
			Vector2 anchoredPos = new Vector2(0f, -FileBrowser.Instance.quickLinksContainer.sizeDelta.y);
			bool flag2 = FileBrowser.Instance.AddQuickLink(icon, name, path, ref anchoredPos);
			bool result;
			if (flag2)
			{
				FileBrowser.Instance.quickLinksContainer.sizeDelta = new Vector2(0f, -anchoredPos.y);
				result = true;
			}
			else
			{
				result = false;
			}
			return result;
		}

		 
		public static void SetExcludedExtensions(params string[] excludedExtensions)
		{
			FileBrowser.Instance.excludedExtensionsSet.Clear();
			bool flag = excludedExtensions != null;
			if (flag)
			{
				for (int i = 0; i < excludedExtensions.Length; i++)
				{
					FileBrowser.Instance.excludedExtensionsSet.Add(excludedExtensions[i].ToLowerInvariant());
				}
			}
		}

		 
		public static void SetFilters(bool showAllFilesFilter, IEnumerable<string> filters)
		{
			FileBrowser.SetFiltersPreProcessing(showAllFilesFilter);
			bool flag = filters != null;
			if (flag)
			{
				foreach (string filter in filters)
				{
					bool flag2 = filter != null && filter.Length > 0;
					if (flag2)
					{
						FileBrowser.Instance.filters.Add(new FileBrowser.Filter(null, filter));
					}
				}
			}
			FileBrowser.SetFiltersPostProcessing();
		}

		 
		public static void SetFilters(bool showAllFilesFilter, params string[] filters)
		{
			FileBrowser.SetFiltersPreProcessing(showAllFilesFilter);
			bool flag = filters != null;
			if (flag)
			{
				for (int i = 0; i < filters.Length; i++)
				{
					bool flag2 = filters[i] != null && filters[i].Length > 0;
					if (flag2)
					{
						FileBrowser.Instance.filters.Add(new FileBrowser.Filter(null, filters[i]));
					}
				}
			}
			FileBrowser.SetFiltersPostProcessing();
		}

		 
		public static void SetFilters(bool showAllFilesFilter, IEnumerable<FileBrowser.Filter> filters)
		{
			FileBrowser.SetFiltersPreProcessing(showAllFilesFilter);
			bool flag = filters != null;
			if (flag)
			{
				foreach (FileBrowser.Filter filter in filters)
				{
					bool flag2 = filter != null && filter.defaultExtension.Length > 0;
					if (flag2)
					{
						FileBrowser.Instance.filters.Add(filter);
					}
				}
			}
			FileBrowser.SetFiltersPostProcessing();
		}

		 
		public static void SetFilters(bool showAllFilesFilter, params FileBrowser.Filter[] filters)
		{
			FileBrowser.SetFiltersPreProcessing(showAllFilesFilter);
			bool flag = filters != null;
			if (flag)
			{
				for (int i = 0; i < filters.Length; i++)
				{
					bool flag2 = filters[i] != null && filters[i].defaultExtension.Length > 0;
					if (flag2)
					{
						FileBrowser.Instance.filters.Add(filters[i]);
					}
				}
			}
			FileBrowser.SetFiltersPostProcessing();
		}

		 
		private static void SetFiltersPreProcessing(bool showAllFilesFilter)
		{
			FileBrowser.Instance.showAllFilesFilter = showAllFilesFilter;
			FileBrowser.Instance.filters.Clear();
			if (showAllFilesFilter)
			{
				FileBrowser.Instance.filters.Add(FileBrowser.Instance.allFilesFilter);
			}
		}

		 
		private static void SetFiltersPostProcessing()
		{
			List<FileBrowser.Filter> filters = FileBrowser.Instance.filters;
			bool flag = filters.Count == 0;
			if (flag)
			{
				filters.Add(FileBrowser.Instance.allFilesFilter);
			}
			int maxFilterStrLength = 100;
			List<string> dropdownValues = new List<string>(filters.Count);
			for (int i = 0; i < filters.Count; i++)
			{
				string filterStr = filters[i].ToString();
				dropdownValues.Add(filterStr);
				maxFilterStrLength = Mathf.Max(maxFilterStrLength, FileBrowser.Instance.CalculateLengthOfDropdownText(filterStr));
			}
			Vector2 size = FileBrowser.Instance.filtersDropdownContainer.sizeDelta;
			size.x = (float)(maxFilterStrLength + 28);
			FileBrowser.Instance.filtersDropdownContainer.sizeDelta = size;
			FileBrowser.Instance.filtersDropdown.ClearOptions();
			FileBrowser.Instance.filtersDropdown.AddOptions(dropdownValues);
			FileBrowser.Instance.filtersDropdown.value = 0;
		}

		 
		public static bool SetDefaultFilter(string defaultFilter)
		{
			bool flag = defaultFilter == null;
			bool result;
			if (flag)
			{
				bool flag2 = FileBrowser.Instance.showAllFilesFilter;
				if (flag2)
				{
					FileBrowser.Instance.filtersDropdown.value = 0;
					FileBrowser.Instance.filtersDropdown.RefreshShownValue();
					result = true;
				}
				else
				{
					result = false;
				}
			}
			else
			{
				defaultFilter = defaultFilter.ToLowerInvariant();
				for (int i = 0; i < FileBrowser.Instance.filters.Count; i++)
				{
					HashSet<string> extensions = FileBrowser.Instance.filters[i].extensions;
					bool flag3 = extensions != null && extensions.Contains(defaultFilter);
					if (flag3)
					{
						FileBrowser.Instance.filtersDropdown.value = i;
						FileBrowser.Instance.filtersDropdown.RefreshShownValue();
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		 
		public static FileBrowser.Permission CheckPermission()
		{
			return FileBrowser.Permission.Granted;
		}

		 
		public static FileBrowser.Permission RequestPermission()
		{
			return FileBrowser.Permission.Granted;
		}

		
		private const string ALL_FILES_FILTER_TEXT = "All Files (.*)";

		
		private const string FOLDERS_FILTER_TEXT = "Folders";

		
		private string DEFAULT_PATH;

		
		private static bool m_askPermissions = true;

		
		private static bool m_singleClickMode = false;

		
		private static FileBrowser m_instance = null;

		
		[Header("References")]
		[SerializeField]
		private FileBrowserMovement window;

		
		private RectTransform windowTR;

		
		[SerializeField]
		private FileBrowserItem itemPrefab;

		
		[SerializeField]
		private FileBrowserQuickLink quickLinkPrefab;

		
		[SerializeField]
		private Text titleText;

		
		[SerializeField]
		private Button backButton;

		
		[SerializeField]
		private Button forwardButton;

		
		[SerializeField]
		private Button upButton;

		
		[SerializeField]
		private InputField pathInputField;

		
		[SerializeField]
		private InputField searchInputField;

		
		[SerializeField]
		private RectTransform quickLinksContainer;

		
		[SerializeField]
		private RectTransform filesContainer;

		
		[SerializeField]
		private ScrollRect filesScrollRect;

		
		[SerializeField]
		private RecycledListView listView;

		
		[SerializeField]
		private InputField filenameInputField;

		
		[SerializeField]
		private Image filenameImage;

		
		[SerializeField]
		private Dropdown filtersDropdown;

		
		[SerializeField]
		private RectTransform filtersDropdownContainer;

		
		[SerializeField]
		private Text filterItemTemplate;

		
		[SerializeField]
		private Toggle showHiddenFilesToggle;

		
		[SerializeField]
		private Text submitButtonText;

		
		[Header("Icons")]
		[SerializeField]
		private Sprite folderIcon;

		
		[SerializeField]
		private Sprite driveIcon;

		
		[SerializeField]
		private Sprite defaultIcon;

		
		[SerializeField]
		private FileBrowser.FiletypeIcon[] filetypeIcons;

		
		private Dictionary<string, Sprite> filetypeToIcon;

		
		[Header("Other")]
		public Color normalFileColor = Color.white;

		
		public Color hoveredFileColor = new Color32(225, 225, byte.MaxValue, byte.MaxValue);

		
		public Color selectedFileColor = new Color32(0, 175, byte.MaxValue, byte.MaxValue);

		
		public Color wrongFilenameColor = new Color32(byte.MaxValue, 100, 100, byte.MaxValue);

		
		public int minWidth = 380;

		
		public int minHeight = 300;

		
		[SerializeField]
		private string[] excludeExtensions;

		
		[SerializeField]
		private FileBrowser.QuickLink[] quickLinks;

		
		private static bool quickLinksInitialized;

		
		private readonly HashSet<string> excludedExtensionsSet = new HashSet<string>();

		
		private readonly HashSet<string> addedQuickLinksSet = new HashSet<string>();

		
		[SerializeField]
		private bool generateQuickLinksForDrives = true;

		
		private RectTransform rectTransform;

		
		private FileAttributes ignoredFileAttributes = FileAttributes.System;

		
		private FileSystemEntry[] allFileEntries;

		
		private readonly List<FileSystemEntry> validFileEntries = new List<FileSystemEntry>();

		
		private readonly List<FileBrowser.Filter> filters = new List<FileBrowser.Filter>();

		
		private FileBrowser.Filter allFilesFilter;

		
		private bool showAllFilesFilter = true;

		
		private int currentPathIndex = -1;

		
		private readonly List<string> pathsFollowed = new List<string>();

		
		private bool canvasDimensionsChanged;

		
		private PointerEventData nullPointerEventData;

		
		private string m_currentPath = string.Empty;

		
		private string m_searchString = string.Empty;

		
		private int m_selectedFilePosition = -1;

		
		private FileBrowserItem m_selectedFile;

		
		private bool m_acceptNonExistingFilename = false;

		
		private bool m_folderSelectMode = false;

		
		private FileBrowser.OnSuccess onSuccess;

		
		private FileBrowser.OnCancel onCancel;

		
		public enum Permission
		{
			
			Denied,
			
			Granted,
			
			ShouldAsk
		}

		
		[Serializable]
		private struct FiletypeIcon
		{
			
			public string extension;

			
			public Sprite icon;
		}

		
		[Serializable]
		private struct QuickLink
		{
			
			public Environment.SpecialFolder target;

			
			public string name;

			
			public Sprite icon;
		}

		
		public class Filter
		{
			
			internal Filter(string name)
			{
				this.name = name;
				this.extensions = null;
				this.defaultExtension = null;
			}

			
			public Filter(string name, string extension)
			{
				this.name = name;
				extension = extension.ToLowerInvariant();
				this.extensions = new HashSet<string>
				{
					extension
				};
				this.defaultExtension = extension;
			}

			
			public Filter(string name, params string[] extensions)
			{
				this.name = name;
				for (int i = 0; i < extensions.Length; i++)
				{
					extensions[i] = extensions[i].ToLowerInvariant();
				}
				this.extensions = new HashSet<string>(extensions);
				this.defaultExtension = extensions[0];
			}

			
			public override string ToString()
			{
				string result = "";
				bool flag = this.name != null;
				if (flag)
				{
					result += this.name;
				}
				bool flag2 = this.extensions != null;
				if (flag2)
				{
					bool flag3 = this.name != null;
					if (flag3)
					{
						result += " (";
					}
					int index = 0;
					foreach (string extension in this.extensions)
					{
						bool flag4 = index++ > 0;
						if (flag4)
						{
							result = result + ", " + extension;
						}
						else
						{
							result += extension;
						}
					}
					bool flag5 = this.name != null;
					if (flag5)
					{
						result += ")";
					}
				}
				return result;
			}

			
			public readonly string name;

			
			public readonly HashSet<string> extensions;

			
			public readonly string defaultExtension;
		}

		
		
		public delegate void OnSuccess(string path);

		
		
		public delegate void OnCancel();
	}
}
