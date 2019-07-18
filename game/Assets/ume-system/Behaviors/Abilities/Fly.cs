using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UME {
	public class Fly : BaseAbility {

		public float flyForce;
		public float maxSpeed;
		// Use this for initialization
		private Rigidbody2D m_Rigidbody2D;
		public override void Initialize(){
			m_Rigidbody2D = GetComponent<Rigidbody2D> ();
		}
		// Update is called once per frame
		public override void Activate () {
			if (m_Rigidbody2D) {
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, Mathf.Max (m_Rigidbody2D.velocity.y, 0f));
				m_Rigidbody2D.AddForce (new Vector2 (0f, flyForce));
				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y,0,maxSpeed));
			}

		}
	}
}
