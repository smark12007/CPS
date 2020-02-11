using UnityEngine;
using UnityEngine.UI;

namespace CPS {

	public sealed class Game1 : Level {

		public override void Clear() {
			base.Clear();
			ChooseAnswer(-1);
		}

		public override void Confirm() {
			GetResult(m_selectedAnswer == m_correctAnswer).SetActive(true);
		}

		public void ChooseAnswer(int index) {
			m_selectedAnswer = index;
			for (int i = 0; i != m_answers.Length; ++i) {
				SetButtonSelected(m_answers[i], i == m_selectedAnswer);
			}
		}
		void SetButtonSelected(Button button, bool selected) {
			button.image.sprite = selected? m_selectedButtonImage : m_normalButtonImage;
		}
		[SerializeField] Button[] m_answers = null;

		[SerializeField] int m_correctAnswer = -1;
		int m_selectedAnswer = -1;

		[SerializeField] Sprite m_normalButtonImage = null;
		[SerializeField] Sprite m_selectedButtonImage = null;
	}
}