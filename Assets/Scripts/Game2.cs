using System;
using UnityEngine;

namespace CPS {

	public sealed class Game2 : Level {

		public override void Clear(bool active) {
			base.Clear(active);
			m_currentDiagramSprite = 0;
			Repaint();
			m_correctAnswer.SetActive(false);
			Sticker.ClearUndoStack();
		}

		public override void Confirm() {
			bool correct = false;
			for (int i = 0; i != m_possibleCorrectAnswerGroups.Length; ++i) {
				if (m_possibleCorrectAnswerGroups[i].validateActiveStickers) {
					correct = true;
					break;
				}
			}
			OnConfirm(correct);
			if (!correct) {
				m_correctAnswer.SetActive(true);
			}
		}

		public void Undo() {
			Sticker.Undo();
		}

		public void Left() {
			if (m_currentDiagramSprite != 0) {
				--m_currentDiagramSprite;
			} else {
				m_currentDiagramSprite = m_diagramImages.Length - 1;
			}
			Repaint();
		}

		public void Right() {
			if (m_currentDiagramSprite != m_diagramImages.Length - 1) {
				++m_currentDiagramSprite;
			} else {
				m_currentDiagramSprite = 0;
			}
			Repaint();
		}

		void Repaint() {
			for (int i = 0; i != m_diagramImages.Length; ++i) {
				m_diagramImages[i].SetActive(i == m_currentDiagramSprite);
			}
		}
		[SerializeField] GameObject[] m_diagramImages = null;
		int m_currentDiagramSprite = 0;

		[Serializable]
		class PossibleCorrectAnswerGroup {

			[Serializable]
			public class AnswerClip {
				public Sticker sticker = null;
				public Sprite sprite = null;
			}
			public AnswerClip[] clips = null;

			public bool validateActiveStickers {
				get {
					for (int i = 0; i != Sticker.numberOfActiveStickers; ++i) {
						if (!ValidateStickerImage(Sticker.GetStickerAt(i))) {
							return false;
						}
					}
					return true;
				}
			}
			bool ValidateStickerImage(Sticker sticker) {
				Sprite sprite = sticker.image.sprite;
				for (int i = 0; i != clips.Length; ++i) {
					if (clips[i].sticker == sticker) {
						return sprite == clips[i].sprite;
					}
				}
				return !sprite;
			}
		}
		[SerializeField] PossibleCorrectAnswerGroup[] m_possibleCorrectAnswerGroups = null;

		[SerializeField] GameObject m_correctAnswer = null;
	}
}