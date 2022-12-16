using System;

namespace SimpleFileBrowser
{
	
	public interface IListViewAdapter
	{
		
		
		
		OnItemClickedHandler OnItemClicked { get; set; }

		
		
		int Count { get; }

		
		
		float ItemHeight { get; }

		
		ListItem CreateItem();

		
		void SetItemContent(ListItem item);
	}
}
