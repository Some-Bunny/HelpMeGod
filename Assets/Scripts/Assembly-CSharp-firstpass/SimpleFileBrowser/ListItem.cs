using System;
using UnityEngine;

namespace SimpleFileBrowser
{
	
	[RequireComponent(typeof(RectTransform))]
	public class ListItem : MonoBehaviour
	{
		

		 
		public object Tag { get; set; }

		

		 
		public int Position { get; set; }

		
		internal void SetAdapter(IListViewAdapter listView)
		{
			this.adapter = listView;
		}

		
		public void OnClick()
		{
			bool flag = this.adapter.OnItemClicked != null;
			if (flag)
			{
				this.adapter.OnItemClicked(this);
			}
		}

		
		private IListViewAdapter adapter;
	}
}
