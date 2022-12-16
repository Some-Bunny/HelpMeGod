using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleFileBrowser
{
	
	[RequireComponent(typeof(ScrollRect))]
	public class RecycledListView : MonoBehaviour
	{
		
		private void Start()
		{
			this.viewportHeight = this.viewportTransform.rect.height;
			base.GetComponent<ScrollRect>().onValueChanged.AddListener(delegate(Vector2 pos)
			{
				this.UpdateItemsInTheList(false);
			});
		}

		
		public void SetAdapter(IListViewAdapter adapter)
		{
			this.adapter = adapter;
			this.itemHeight = adapter.ItemHeight;
			this._1OverItemHeight = 1f / this.itemHeight;
		}

		
		public void UpdateList()
		{
			float newHeight = Mathf.Max(1f, (float)this.adapter.Count * this.itemHeight);
			this.contentTransform.sizeDelta = new Vector2(0f, newHeight);
			this.viewportHeight = this.viewportTransform.rect.height;
			this.UpdateItemsInTheList(true);
		}

		
		public void OnViewportDimensionsChanged()
		{
			this.viewportHeight = this.viewportTransform.rect.height;
			this.UpdateItemsInTheList(false);
		}

		
		private void UpdateItemsInTheList(bool updateAllVisibleItems = false)
		{
			bool flag = this.adapter.Count > 0;
			if (flag)
			{
				float contentPos = this.contentTransform.anchoredPosition.y - 1f;
				int newTopIndex = (int)(contentPos * this._1OverItemHeight);
				int newBottomIndex = (int)((contentPos + this.viewportHeight + 2f) * this._1OverItemHeight);
				bool flag2 = newTopIndex < 0;
				if (flag2)
				{
					newTopIndex = 0;
				}
				bool flag3 = newBottomIndex > this.adapter.Count - 1;
				if (flag3)
				{
					newBottomIndex = this.adapter.Count - 1;
				}
				bool flag4 = this.currentTopIndex == -1;
				if (flag4)
				{
					updateAllVisibleItems = true;
					this.currentTopIndex = newTopIndex;
					this.currentBottomIndex = newBottomIndex;
					this.CreateItemsBetweenIndices(newTopIndex, newBottomIndex);
				}
				else
				{
					bool flag5 = newBottomIndex < this.currentTopIndex || newTopIndex > this.currentBottomIndex;
					if (flag5)
					{
						updateAllVisibleItems = true;
						this.DestroyItemsBetweenIndices(this.currentTopIndex, this.currentBottomIndex);
						this.CreateItemsBetweenIndices(newTopIndex, newBottomIndex);
					}
					else
					{
						bool flag6 = newTopIndex > this.currentTopIndex;
						if (flag6)
						{
							this.DestroyItemsBetweenIndices(this.currentTopIndex, newTopIndex - 1);
						}
						bool flag7 = newBottomIndex < this.currentBottomIndex;
						if (flag7)
						{
							this.DestroyItemsBetweenIndices(newBottomIndex + 1, this.currentBottomIndex);
						}
						bool flag8 = newTopIndex < this.currentTopIndex;
						if (flag8)
						{
							this.CreateItemsBetweenIndices(newTopIndex, this.currentTopIndex - 1);
							bool flag9 = !updateAllVisibleItems;
							if (flag9)
							{
								this.UpdateItemContentsBetweenIndices(newTopIndex, this.currentTopIndex - 1);
							}
						}
						bool flag10 = newBottomIndex > this.currentBottomIndex;
						if (flag10)
						{
							this.CreateItemsBetweenIndices(this.currentBottomIndex + 1, newBottomIndex);
							bool flag11 = !updateAllVisibleItems;
							if (flag11)
							{
								this.UpdateItemContentsBetweenIndices(this.currentBottomIndex + 1, newBottomIndex);
							}
						}
					}
					this.currentTopIndex = newTopIndex;
					this.currentBottomIndex = newBottomIndex;
				}
				bool flag12 = updateAllVisibleItems;
				if (flag12)
				{
					this.UpdateItemContentsBetweenIndices(this.currentTopIndex, this.currentBottomIndex);
				}
			}
			else
			{
				bool flag13 = this.currentTopIndex != -1;
				if (flag13)
				{
					this.DestroyItemsBetweenIndices(this.currentTopIndex, this.currentBottomIndex);
					this.currentTopIndex = -1;
				}
			}
		}

		
		private void CreateItemsBetweenIndices(int topIndex, int bottomIndex)
		{
			for (int i = topIndex; i <= bottomIndex; i++)
			{
				this.CreateItemAtIndex(i);
			}
		}

		
		private void CreateItemAtIndex(int index)
		{
			bool flag = this.pooledItems.Count > 0;
			ListItem item;
			if (flag)
			{
				item = this.pooledItems.Pop();
				item.gameObject.SetActive(true);
			}
			else
			{
				item = this.adapter.CreateItem();
				item.transform.SetParent(this.contentTransform, false);
				item.SetAdapter(this.adapter);
			}
			((RectTransform)item.transform).anchoredPosition = new Vector2(1f, (float)(-(float)index) * this.itemHeight);
			this.items[index] = item;
		}

		
		private void DestroyItemsBetweenIndices(int topIndex, int bottomIndex)
		{
			for (int i = topIndex; i <= bottomIndex; i++)
			{
				ListItem item = this.items[i];
				item.gameObject.SetActive(false);
				this.pooledItems.Push(item);
			}
		}

		
		private void UpdateItemContentsBetweenIndices(int topIndex, int bottomIndex)
		{
			for (int i = topIndex; i <= bottomIndex; i++)
			{
				ListItem item = this.items[i];
				item.Position = i;
				this.adapter.SetItemContent(item);
			}
		}

		
		public RectTransform viewportTransform;

		
		public RectTransform contentTransform;

		
		private float itemHeight;

		
		private float _1OverItemHeight;

		
		private float viewportHeight;

		
		private readonly Dictionary<int, ListItem> items = new Dictionary<int, ListItem>();

		
		private readonly Stack<ListItem> pooledItems = new Stack<ListItem>();

		
		private IListViewAdapter adapter = null;

		
		private int currentTopIndex = -1;

		
		private int currentBottomIndex = -1;
	}
}
