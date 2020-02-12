using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace CPS {

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	[RequireComponent(typeof(EventTrigger))]
	public sealed class Drag : BaseBehaviour {

		new public RectTransform transform {
			get {
				return base.transform as RectTransform;
			}
		}

		public Image image {
			get {
				if (!m_image) {
					m_image = GetComponent<Image>();
				}
				return m_image;
			}
		}
		Image m_image = null;

//		public void OnPointerDown(BaseEventData eventData) {
//			PointerEventData pointerData = eventData as PointerEventData;
//		}

		public void OnDrag(BaseEventData eventData) {
			transform.position += (Vector3)(eventData as PointerEventData).delta;
		}

		public void OnPointerUp() {
			Camera targetCamera = GetComponentInParent<Canvas>().worldCamera;
			Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(targetCamera, transform.position);
			for (int i = 0; i != Sticker.numberOfActiveStickers; ++i) {
				Sticker sticker = Sticker.GetStickerAt(i);
				RectTransform stickerTransform = sticker.transform;
				Vector2 localPoint;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(stickerTransform, screenPoint, targetCamera, out localPoint) &&
					stickerTransform.rect.Contains(localPoint)) {
					sticker.Stick(this);
					break;
				}
			}
			transform.anchoredPosition = Vector2.zero;
		}

		[Obsolete]
		public void OnPointerUp(BaseEventData eventData) {
			Vector2 screenPoint = (eventData as PointerEventData).position;
			for (int i = 0; i != Sticker.numberOfActiveStickers; ++i) {
				Sticker sticker = Sticker.GetStickerAt(i);
				RectTransform stickerTransform = sticker.transform;
				Vector2 localPoint;
				if (RectTransformUtility.ScreenPointToLocalPointInRectangle(stickerTransform, screenPoint, GetComponentInParent<Canvas>().worldCamera, out localPoint) &&
					stickerTransform.rect.Contains(localPoint)) {
					sticker.Stick(this);
					break;
				}
			}
			transform.anchoredPosition = Vector2.zero;
		}

#if UNITY_EDITOR
		void Reset() {
			EventTrigger eventTrigger = GetComponent<EventTrigger>();
			eventTrigger.triggers = new List<EventTrigger.Entry>();
			EventTrigger.Entry entry;

			entry = new EventTrigger.Entry();
//			entry.eventID = EventTriggerType.PointerDown;
//			UnityEventTools.AddPersistentListener(entry.callback = new EventTrigger.TriggerEvent(), OnPointerDown);
//			eventTrigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			UnityEventTools.AddPersistentListener(entry.callback = new EventTrigger.TriggerEvent(), OnDrag);
			eventTrigger.triggers.Add(entry);

			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback = new EventTrigger.TriggerEvent();
			UnityEventTools.AddVoidPersistentListener(entry.callback = new EventTrigger.TriggerEvent(), OnPointerUp);
			eventTrigger.triggers.Add(entry);
		}
#endif
	}
}