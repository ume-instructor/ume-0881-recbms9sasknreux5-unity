using UnityEngine;
using System.Collections;
using System;
namespace UME
{
	[AddComponentMenu("UME/Control/KeyRotate")]
	[Serializable]
	public class KeyRotate : MonoBehaviour {
		public KeyCode Key;
		public float torque = 0;
		public float maxVelocity = 10f; 

		private Rigidbody2D m_rigidbody;
		private Animator m_Anim;          

		void Start () {
			m_rigidbody = gameObject.GetComponent<Rigidbody2D> ();
			m_Anim = GetComponent<Animator>();			
			if (m_Anim) {
				m_Anim.SetBool ("Ground", true);
			}
		}


		void FixedUpdate () { 
			/*
			if (m_Anim) {
				
				m_Anim.SetFloat ("Rotate", (float)Math.Abs(m_rigidbody.angularVelocity));
			}
			*/
			// Key Detection and Management
			if (Input.GetKey (Key)) {
				if (m_rigidbody && m_rigidbody.angularVelocity < maxVelocity) 
					m_rigidbody.AddTorque (torque);
		}
	}

	}
} 