using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleFileBrowser
{
	
	public class FileBrowserQuickLink : FileBrowserItem, IPointerClickHandler, IEventSystemHandler
	{
		

		public string TargetPath
		{
			get
			{
				return this.m_targetPath;
			}
		}

		
		public void SetQuickLink(Sprite icon, string name, string targetPath)
		{
			base.SetFile(icon, name, true);
			this.m_targetPath = targetPath;
		}

		
		public new void OnPointerClick(PointerEventData eventData)
		{
			this.fileBrowser.OnQuickLinkSelected(this);
		}

		
		private string m_targetPath;
	}
}
