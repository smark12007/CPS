using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CPS {

	[DisallowMultipleComponent]
	[RequireComponent(typeof(Button))]
	public sealed class Choice : BaseBehaviour {

		public Button button {
			get {
				if (!m_button) {
					m_button = GetComponent<Button>();
				}
				return m_button;
			}
		}
		Button m_button = null;

		public void SetSelected(bool selected) {
			if (selected) {
				button.image.sprite = m_selectedImage;
			} else {
				button.image.sprite = m_normalImage;		
			}
			if (onSelectionChanged != null) {
				onSelectionChanged.Invoke(selected);
			}
		}
		[SerializeField] Sprite m_normalImage = null;
		[SerializeField] Sprite m_selectedImage = null;

		[Serializable] public sealed class OnSelectionChanged : UnityEvent<bool> {}
		public OnSelectionChanged onSelectionChanged = new OnSelectionChanged();

#if UNITY_EDITOR
		void Reset() {
			m_normalImage = button.image.sprite;
			m_selectedImage = button.spriteState.highlightedSprite;
		}
#endif
	}
}