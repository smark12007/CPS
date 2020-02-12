using System.Collections;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.Events;
#endif

namespace CPS {

	[DisallowMultipleComponent]
	public abstract class Level : BaseBehaviour {

		public virtual void Clear(bool active) {
			if (waitForResult != null) {
				StopCoroutine(waitForResult);
			}
			m_o.SetActive(false);
			m_x.SetActive(false);
			m_inputMask.SetActive(false);
			gameObject.SetActive(active);
		}

		public abstract void Confirm();

		public void OnConfirm(bool correct) {
			if (waitForResult != null) {
				StopCoroutine(waitForResult);
			}
			StartCoroutine(waitForResult = WaitForResult(correct));
		}
		IEnumerator WaitForResult(bool correct) {
			m_inputMask.SetActive(true);
			if (correct) {
				m_o.SetActive(true);
				m_x.SetActive(false);
			} else {
				m_x.SetActive(false);
				m_x.SetActive(true);
			}
			yield return new WaitForSeconds(2f);
			if (correct) {
				if (onCorrectAnswer != null) {
					onCorrectAnswer.Invoke();
				}
			} else {
				if (onWrongAnswer != null) {
					onWrongAnswer.Invoke();
				}
			}
			waitForResult = null;
		}
		IEnumerator waitForResult = null;

		[SerializeField] GameObject m_o = null;
		[SerializeField] GameObject m_x = null;
		[SerializeField] GameObject m_inputMask = null;

		public UnityEvent onCorrectAnswer = new UnityEvent();
		public UnityEvent onWrongAnswer = new UnityEvent();

#if UNITY_EDITOR
		protected virtual void Reset() {
			UnityEventTools.AddBoolPersistentListener(onWrongAnswer, Clear, true);
		}
#endif
	}
}