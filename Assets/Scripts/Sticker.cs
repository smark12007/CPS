using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CPS {

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Image))]
	public sealed class Sticker : BaseBehaviour {

		new public RectTransform transform {
			get {
				return base.transform as RectTransform;
			}
		}

		public void Stick(Drag drag) {
			m_undoStack.Push(new Dirty {
				sticker = this,
				image = image.sprite
			});
			image.enabled = true;
			image.sprite = drag.image.sprite;
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

		void OnEnable() {
			image.sprite = null;
			image.enabled = false;
			m_stickers.Add(this);
		}

		void OnDisable() {
			m_stickers.Remove(this);			
			image.enabled = true;
		}

		public static int numberOfActiveStickers {
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
		}
		class Dirty {
			public Sticker sticker;
			public Sprite image;
		}
		static Stack<Dirty> m_undoStack = new Stack<Dirty>();
	}
}