using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace UME
{
    [AddComponentMenu("UME/Triggers/UITrigger")]
    [Serializable]
	public class UITrigger : BaseTrigger {

		//use custom inspector for this
		public UITriggerType type;

		public string value;
        public float duration=0f;

		// Use this for initialization
		public override void Activate (Collider2D other)
		{
			
			if (uiControl)
				uiControl.UpdateState (type, value, duration, other.gameObject);
					
		}


	}
}