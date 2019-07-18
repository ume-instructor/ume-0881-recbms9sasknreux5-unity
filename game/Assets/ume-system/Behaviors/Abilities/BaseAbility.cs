using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UME {
	
	public class BaseAbility : MonoBehaviour {
	
		public KeyCode key;
		[HideInInspector]
		public float duration;

		void Start () {
			Initialize ();
		}
		
		// Update is called once per frame
		void FixedUpdate () {
			if (Input.GetKey(key) && duration > 0) {
				Activate();
				duration--;
			}
		}
		public virtual void Initialize(){}
		public virtual void Activate(){}
	}

}