using System;
using UnityEngine;

namespace UME
{
	[AddComponentMenu("UME/Move/Hunt")]
	[RequireComponent(typeof(Rigidbody2D))]
    public class Hunt : MonoBehaviour
    {
		[SerializeField] [Range(0,50)] private float m_MaxSpeed = 10f; 
		[SerializeField] [Range(0,100)] private float m_Attraction = 100f;
		[SerializeField] [Range(0.0f, 5.0f)] public float checkinDistance = 0.7f;
		public GameObject targetObject;
		private bool m_FacingRight = true;  // For determining which way the enemy is currently facing.
		private Animator m_Anim;            // Reference to the enemy's animator component.
		private Rigidbody2D m_Rigidbody2D;
		private Transform target;
		private bool noPlayer = true;

		private void Awake(){
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			if (targetObject == null) {
				targetObject=GameObject.FindWithTag("Player");
			}
			if (targetObject != null) {
				target = targetObject.transform;
				noPlayer = false;
			} 
		}
		// Update is called once per frame
        private void FixedUpdate()
		{	
			if (noPlayer) return;

			float distance = (target.position.x - transform.position.x);
			float active = 0;
			Vector3 move = Vector3.zero;
			m_Rigidbody2D.velocity = new Vector2(0.0f, 0.0f);
			if (Mathf.Abs(distance) <= m_Attraction && Mathf.Abs(distance) >= checkinDistance)
				active = Mathf.Sign (distance);
			if (active != 0) {
				m_Rigidbody2D.velocity = new Vector2(active*m_MaxSpeed, m_Rigidbody2D.velocity.y);
				if (m_Anim != null) {
					m_Anim.SetBool ("Ground", true);
					m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
					m_Anim.SetFloat ("Speed", Mathf.Abs (active * m_MaxSpeed * .03f));
				}
				if (active > 0 && !m_FacingRight)
					Flip ();
				else if (active < 0 && m_FacingRight)
					Flip ();
			
			}
		}
		private void Flip()
		{
			// Switch the way the player is labelled as facing.
			m_FacingRight = !m_FacingRight;
			
			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}
	}
}
