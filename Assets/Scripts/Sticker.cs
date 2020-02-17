using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CPS {

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public sealed class Sticker : BaseBehaviour {

		public RectTransform areaDetermination {
			get {
				return m_areaDetermination;
			}
		}
		[SerializeField] RectTransform m_areaDetermination = null;

		public void Stick(Drag drag) {
			m_undoStack.Push(new Dirty {
				sticker = this,
				image = image.sprite
			});
			image.enabled = true;
			image.sprite = drag.image.sprite;
			for (int i = 0; i != m_connectedStickers.Length; ++i) {
				m_connectedStickers[i].image.enabled = true;
				m_connectedStickers[i].image.sprite = drag.image.sprite;
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

		public bool AddToList() {
			if (m_stickers.Contains(this)) {
				return false;
			}
			image.sprite = null;
			image.enabled = false;
			m_stickers.Add(this);
			return true;
		}

		public bool RemoveFromList() {
			if (m_stickers.Remove(this)) {
				image.enabled = true;
				return true;
			}
			return false;
		}

		public static int numberOfStickers {
			get {
				return m_stickers.Count;
			}
		}
		public static Sticker GetStickerAt(int i) {
			return m_stickers[i];
		}
		static List<Sticker> m_stickers = new List<Sticker>();

		public static void ClearUndoStack() {
			m_undoStack.Clear();
		}
		public static void Undo() {
			if (m_undoStack.Count == 0) {
				return;
			}
			Dirty lastAction = m_undoStack.Pop();
			if (!(lastAction.sticker.image.sprite = lastAction.image)) {
				lastAction.sticker.image.enabled = false;
			}
			for (int i = 0; i != lastAction.sticker.m_connectedStickers.Length; ++i) {
				if (!(lastAction.sticker.m_connectedStickers[i].image.sprite = lastAction.sticker.image.sprite)) {
					lastAction.sticker.m_connectedStickers[i].image.enabled = false;
				}
			}
		}
		class Dirty {
			public Sticker sticker;
			public Sprite image;
		}
		static Stack<Dirty> m_undoStack = new Stack<Dirty>();

		[SerializeField] Sticker[] m_connectedStickers = new Sticker[0];
	}
}