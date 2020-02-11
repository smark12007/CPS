using UnityEngine;

namespace CPS {

	[DisallowMultipleComponent]
	public abstract class Level : BaseBehaviour {

		public virtual void Clear() {
			GetResult(true)?.SetActive(false);
			GetResult(false)?.SetActive(false);
			gameObject.SetActive(false);
		}

		public abstract void Confirm();

		public GameObject GetResult(bool correct) {
			return correct? m_o : m_x;
		}
		[SerializeField] GameObject m_o = null;
		[SerializeField] GameObject m_x = null;
	}
}