using UnityEngine;
using System.Collections;
namespace UME{
	/*
	 * Switch prefab on off
	*/
    [AddComponentMenu("UME/Control/KeySwitch")]
    public class KeySwitch: MonoBehaviour
	{
		public KeyCode Key;
		public GameObject targetObject;
		public bool onOff = true;
		// Use this for initialization
		void Start ()
		{
			if (targetObject != null){
				targetObject.SetActive(!onOff);
			}
		}
		void FixedUpdate () { // Will not work if FixedUpdate, leave as is!
			// Key Detection and Management

			if (Input.GetKey (Key)) {
				if (targetObject != null) {
					targetObject.SetActive (onOff);
				}
			} else if(targetObject != null){
				targetObject.SetActive(!onOff);
			}

		}

	}
}
