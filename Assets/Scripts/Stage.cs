using UnityEngine;

namespace CPS {

	[DisallowMultipleComponent]
	public sealed class Stage : BaseBehaviour {

		public GameObject banner {
			get {
				return m_banner;
			}
		}
		[SerializeField] GameObject m_banner = null;

		public int tutorialPageCount {
			get {
				return m_tutorialPages.Length;
			}
		}
		public GameObject GetTutorialAt(int index) {
			return m_tutorialPages[index];
		}
		public void ShowTutorial(int index) {
			Clear(true);
			m_tutorialPages[index].SetActive(true);
		}
		[SerializeField] GameObject[] m_tutorialPages = null;

		public int levelCount {
			get {
				return m_levels.Length;
			}
		}
		public void ShowLevel(int index) {
			Clear(true);
			m_levels[index].gameObject.SetActive(true);
		}
		public Level GetLevelAt(int index) {
			return m_levels[index];
		}
		[SerializeField] Level[] m_levels = null;

		public void Clear() {
			Clear(false);
		}
		void Clear(bool active) {
			gameObject.SetActive(active);
			m_banner.SetActive(active);
			for (int i = 0; i != m_tutorialPages.Length; ++i) {
				m_tutorialPages[i].gameObject.SetActive(false);
			}
			for (int i = 0; i != m_levels.Length; ++i) {
				m_levels[i].Clear(false);
			}
		}
	}
}