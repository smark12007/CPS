using UnityEngine;
using UnityEngine.UI;

namespace CPS {

	public sealed class Game1 : Level {

		public override void Clear(bool active) {
			base.Clear(active);
			ChooseAnswer(-1);
		}

		public override void Confirm() {
			OnConfirm(m_selectedAnswer == m_correctAnswer);
		}

		public void ChooseAnswer(int index) {
			m_selectedAnswer = index;
			for (int i = 0; i != m_answers.Length; ++i) {
				m_answers[i].SetSelected(i == m_selectedAnswer);
			}
		}
		[SerializeField] Choice[] m_answers = null;

		[SerializeField] int m_correctAnswer = -1;
		int m_selectedAnswer = -1;
	}
}