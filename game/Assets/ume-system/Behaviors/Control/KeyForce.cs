using UnityEngine;
using System.Collections;
using System;
namespace UME
{
    [AddComponentMenu("UME/Control/KeyForce")]
	[Serializable]
	public class KeyForce : MonoBehaviour {
		public KeyCode Key;
		public Vector2 force = Vector2.zero;
		public float maxVelocity = 10f ;
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
			Vector2 applyForce = Vector2.zero;

			if (m_Anim) {
				m_Anim.SetFloat ("Speed", (float)Math.Abs(m_rigidbody.velocity.x));

			}
			// Key Detection and Management
			if (Input.GetKey (Key)) {
				if (m_rigidbody){
					if (Math.Sign (force.x) > 0f && m_rigidbody.velocity.x < maxVelocity) 
						applyForce.x = force.x;
					if (Math.Sign (force.x) < 0f && m_rigidbody.velocity.x > Math.Sign(force.x)*maxVelocity) 
						applyForce.x = force.x;
					if (Math.Sign (force.y) > 0f && m_rigidbody.velocity.y < maxVelocity)
						applyForce.y = force.y;
					if (Math.Sign (force.y) < 0f && m_rigidbody.velocity.y > Math.Sign(force.y)*maxVelocity) 
						applyForce.y = force.y;
					
					m_rigidbody.AddForce (applyForce);

				}

			}


		}
	}
} 