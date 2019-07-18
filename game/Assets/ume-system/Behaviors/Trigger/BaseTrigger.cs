using System;
using UnityEngine;

namespace UME
{
	
	public class BaseTrigger : MonoBehaviour
	{
		public string activate = "All";

		protected bool m_inTrigger = false;
		protected UIControl uiControl; 

		void Start()
		{

			uiControl = FindObjectOfType<UIControl> ();
			if (uiControl == null) {
					GameObject obj = Instantiate (Resources.Load("UIControl")) as GameObject;
					obj.name = "UIControl";
				uiControl = obj.GetComponent<UIControl> ();
			}
			Initialize ();
		}
		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.tag.ToLower () == "ignore") { return; }

			if(other.tag.ToLower() == activate.ToLower() || activate.ToLower() == "all" && !m_inTrigger){ 
				m_inTrigger = true;
				Activate (other); 
			}

		}
		private void OnTriggerExit2D(Collider2D other)
		{
			if (other.tag.ToLower () == "ignore") { return; }
			m_inTrigger = false;

		}

		//derived classes must implement these
		public virtual void Initialize (){}
		public virtual void Activate(Collider2D other){}
	}
}
