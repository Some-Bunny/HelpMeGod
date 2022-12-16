using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SimpleFileBrowser
{
	
	public class FileBrowserMovement : MonoBehaviour
	{
		
		public void Initialize(FileBrowser fileBrowser)
		{
			this.fileBrowser = fileBrowser;
			this.canvasTR = fileBrowser.GetComponent<RectTransform>();
		}

		
		public void OnDragStarted(BaseEventData data)
		{
			PointerEventData pointer = (PointerEventData)data;
			this.canvasCam = pointer.pressEventCamera;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.window, pointer.pressPosition, this.canvasCam, out this.initialTouchPos);
		}

		
		public void OnDrag(BaseEventData data)
		{
			PointerEventData pointer = (PointerEventData)data;
			Vector2 touchPos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.window, pointer.position, this.canvasCam, out touchPos);
			this.window.anchoredPosition += touchPos - this.initialTouchPos;
		}

		
		public void OnEndDrag(BaseEventData data)
		{
			this.fileBrowser.EnsureWindowIsWithinBounds();
		}

		
		public void OnResizeStarted(BaseEventData data)
		{
			PointerEventData pointer = (PointerEventData)data;
			this.canvasCam = pointer.pressEventCamera;
			this.initialAnchoredPos = this.window.anchoredPosition;
			this.initialSizeDelta = this.window.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTR, pointer.pressPosition, this.canvasCam, out this.initialTouchPos);
		}

		
		public void OnResize(BaseEventData data)
		{
			PointerEventData pointer = (PointerEventData)data;
			Vector2 touchPos;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasTR, pointer.position, this.canvasCam, out touchPos);
			Vector2 delta = touchPos - this.initialTouchPos;
			Vector2 newSize = this.initialSizeDelta + new Vector2(delta.x, -delta.y);
			bool flag = newSize.x < (float)this.fileBrowser.minWidth;
			if (flag)
			{
				newSize.x = (float)this.fileBrowser.minWidth;
			}
			bool flag2 = newSize.y < (float)this.fileBrowser.minHeight;
			if (flag2)
			{
				newSize.y = (float)this.fileBrowser.minHeight;
			}
			newSize.x = (float)((int)newSize.x);
			newSize.y = (float)((int)newSize.y);
			delta = newSize - this.initialSizeDelta;
			this.window.anchoredPosition = this.initialAnchoredPos + new Vector2(delta.x * 0.5f, delta.y * -0.5f);
			this.window.sizeDelta = newSize;
			this.listView.OnViewportDimensionsChanged();
		}

		
		public void OnEndResize(BaseEventData data)
		{
			this.fileBrowser.EnsureWindowIsWithinBounds();
		}

		
		private FileBrowser fileBrowser;

		
		private RectTransform canvasTR;

		
		private Camera canvasCam;

		
		[SerializeField]
		private RectTransform window;

		
		[SerializeField]
		private RecycledListView listView;

		
		private Vector2 initialTouchPos = Vector2.zero;

		
		private Vector2 initialAnchoredPos;

		
		private Vector2 initialSizeDelta;
	}
}
