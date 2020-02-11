using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CPS {

	[DisallowMultipleComponent]
	public sealed class ApplicationManager : BaseBehaviour {

		public void Home() {
			Clear();
			m_homePage.SetActive(true);
		}
		[SerializeField] GameObject m_homePage = null;

		public void StageSelection() {
			Clear();
			m_selectionPage.SetActive(true);
		}
		[SerializeField] GameObject m_selectionPage = null;

		public void Stage(int stage) {
			Clear();
			m_stagePanel.SetActive(true);
			m_currentStage = stage;
			if (m_stages[m_currentStage].tutorialPageCount != 0) {
				m_stages[m_currentStage].ShowTutorial(0);
			} else {
				m_stages[m_currentStage].ShowLevel(0);
			}
		}

		public void Tutorial(int page) {
			Tutorial(m_currentStage, page);
		}
		public void Tutorial(int stage, int page) {
			Clear();
			m_stagePanel.SetActive(true);
			m_stages[m_currentStage = stage].ShowTutorial(page);
		}

		public void Level(int level) {
			Level(m_currentStage, level);
		}
		public void Level(int stage, int level) {
			Clear();
			m_stagePanel.SetActive(true);
			m_stages[m_currentStage = stage].ShowLevel(level);
		}
		[SerializeField] GameObject m_stagePanel = null;
		[SerializeField] Stage[] m_stages = null;
		int m_currentStage = -1;

		public void Success() {
			Clear();
			m_successPage.SetActive(true);
		}
		[SerializeField] GameObject m_successPage = null;

		void Clear() {
			m_homePage.SetActive(false);
			m_selectionPage.SetActive(false);
			m_stagePanel.SetActive(false);
			for (int i = 0; i != m_stages.Length; ++i) {
				m_stages[i].Clear();
			}
			m_successPage.SetActive(false);
		}

#if UNITY_EDITOR
		[UnityEditor.CustomEditor(typeof(ApplicationManager)), CanEditMultipleObjects]
		class CustomEditor : Editor {

			public override void OnInspectorGUI() {
				base.OnInspectorGUI();
				EditorGUILayout.Space();
				if (targets.Length != 1) {
					return;
				}
				ApplicationManager target = this.target as ApplicationManager;
				EditorGUILayout.Space();
				if (GUILayout.Button("Home")) {
					target.Home();
				}
				EditorGUILayout.Space();
				if (GUILayout.Button("Stage Selection")) {
					target.StageSelection();
				}
				EditorGUILayout.Space();
				for (int i = 0; i != target.m_stages.Length; ++i) {
					GUILayout.BeginHorizontal();
					GUILayout.Space(25f);
					GUILayout.BeginVertical();
					for (int j = 0; j != target.m_stages[i].tutorialPageCount; ++j) {
						GUILayout.BeginHorizontal();
						GUILayout.Space(25f);
						GUILayout.EndHorizontal();
						if (GUILayout.Button("Tutorial " + (i + 1) + "-" + (j + 1))) {
							target.Tutorial(i, j);
						}
					}
					for (int j = 0; j != target.m_stages[i].levelCount; ++j) {
						GUILayout.BeginHorizontal();
						GUILayout.Space(25f);
						GUILayout.EndHorizontal();
						if (GUILayout.Button("Level " + (i + 1) + "-" + (j + 1))) {
							target.Level(i, j);
						}
					}
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
					EditorGUILayout.Space();
				}
				if (GUILayout.Button("Success")) {
					target.Success();
				}
				EditorGUILayout.Space();
				EditorGUI.BeginDisabledGroup(target.m_homePage.activeSelf || target.m_selectionPage.activeSelf || target.m_successPage.activeSelf);
				if (GUILayout.Button("Goto Current Level")) {
					Stage stage = target.m_stages[target.m_currentStage];
					bool finding = true;
//					if (finding) {
						for (int i = 0; i != stage.tutorialPageCount; ++i) {
							GameObject tutorial = stage.GetTutorialAt(i);
							if (tutorial.activeSelf) {
								Selection.activeObject = tutorial;
								finding = false;
								break;
							}
						}
//					}
					if (finding) {
						for (int i = 0; i != stage.levelCount; ++i) {
							GameObject level = stage.GetLevelAt(i).gameObject;
							if (level.activeSelf) {
								Selection.activeObject = level;
//								finding = false;
								break;
							}
						}
					}
				}
			}
		}
#endif
	}
}