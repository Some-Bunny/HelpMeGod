using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleFileBrowser
{
	
	public class FileBrowserItem : ListItem, IPointerClickHandler, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler
	{
		

		public RectTransform TransformComponent
		{
			get
			{
				bool flag = this.m_transform == null;
				if (flag)
				{
					this.m_transform = (RectTransform)base.transform;
				}
				return this.m_transform;
			}
		}

		

		public string Name
		{
			get
			{
				return this.nameText.text;
			}
		}

		

		 
		public bool IsDirectory { get; private set; }

		
		public void SetFileBrowser(FileBrowser fileBrowser)
		{
			this.fileBrowser = fileBrowser;
		}

		
		public void SetFile(Sprite icon, string name, bool isDirectory)
		{
			this.icon.sprite = icon;
			this.nameText.text = name;
			this.IsDirectory = isDirectory;
		}

		
		public void OnPointerClick(PointerEventData eventData)
		{
			bool singleClickMode = FileBrowser.SingleClickMode;
			if (singleClickMode)
			{
				this.fileBrowser.OnItemSelected(this);
				this.fileBrowser.OnItemOpened(this);
			}
			else
			{
				bool flag = Time.realtimeSinceStartup - this.prevTouchTime < 0.5f;
				if (flag)
				{
					bool flag2 = this.fileBrowser.SelectedFilePosition == base.Position;
					if (flag2)
					{
						this.fileBrowser.OnItemOpened(this);
					}
					this.prevTouchTime = float.NegativeInfinity;
				}
				else
				{
					this.fileBrowser.OnItemSelected(this);
					this.prevTouchTime = Time.realtimeSinceStartup;
				}
			}
		}

		
		public void OnPointerEnter(PointerEventData eventData)
		{
			bool flag = this.fileBrowser.SelectedFilePosition != base.Position;
			if (flag)
			{
				this.background.color = this.fileBrowser.hoveredFileColor;
			}
		}

		
		public void OnPointerExit(PointerEventData eventData)
		{
			bool flag = this.fileBrowser.SelectedFilePosition != base.Position;
			if (flag)
			{
				this.background.color = this.fileBrowser.normalFileColor;
			}
		}

		
		public void Select()
		{
			this.background.color = this.fileBrowser.selectedFileColor;
		}

		
		public void Deselect()
		{
			this.background.color = this.fileBrowser.normalFileColor;
		}

		
		public void SetHidden(bool isHidden)
		{
			Color c = this.icon.color;
			c.a = (isHidden ? 0.5f : 1f);
			this.icon.color = c;
			c = this.nameText.color;
			c.a = (isHidden ? 0.55f : 1f);
			this.nameText.color = c;
		}

		
		private const float DOUBLE_CLICK_TIME = 0.5f;

		
		protected FileBrowser fileBrowser;

		
		[SerializeField]
		private Image background;

		
		[SerializeField]
		private Image icon;

		
		[SerializeField]
		private Text nameText;

		
		private float prevTouchTime = float.NegativeInfinity;

		
		private RectTransform m_transform;
	}
}
