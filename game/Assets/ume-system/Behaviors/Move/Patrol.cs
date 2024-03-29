using UnityEngine;
using System;
using System.Collections;

namespace UME{
    [AddComponentMenu("UME/Move/Patrol")]
    public class Patrol : MonoBehaviour {
		public Transform[] waypoints;
		//[Range(0.0001f, 0.5f)] 
		public float speed = 0.0001f;
		[Range(0.0f, 10.0f)] public float checkinDistance = 1.0f;
		[Range(0.1f, 5.0f)] public float delay = 2.0f;

		private int m_target_idx = 0;
		private Transform m_target;
		private float m_target_attention = 0;
		private bool m_FacingRight = true;
		private bool m_hitTarget = false;
		private Rigidbody2D m_Rigidbody2D;
		private Animator m_Anim;
		public bool flip = false;

		private void Start()
		{
			// set initial waypoint traget
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			m_target_attention = delay;
			if (waypoints.Length > 0) {
				if (waypoints [m_target_idx] != null) {
					m_target = waypoints [m_target_idx];
				}
			}
		}
/*
		private void OnCollisionStay2D(Collision2D other){
			m_target_attention -= Time.deltaTime;
			if (m_target_attention <= 0) {
				updateTarget ();
				m_target_attention = delay;
			}
		}
*/
		private void OnCollisionEnter2D(Collision2D other){
			if (other.transform.position == m_target.position) {
				m_hitTarget = true;
			}
		}
		private void updateTarget(){
			if (waypoints.Length > 0) {
				//update target after delay time expires
				m_target_idx = (m_target_idx >= waypoints.Length - 1) ? 0 : m_target_idx + 1;
				m_target = waypoints [m_target_idx];
			}
			//reset delay time

		}

		// Update is called once per frame
		private void FixedUpdate()
		{
			if (m_target != null) {
				// check for arrival
				if ( Vector3.Distance (this.transform.position, m_target.position) <= checkinDistance || m_hitTarget) {
					//start counting delay time
					m_target_attention -= Time.deltaTime;
					if (m_Anim != null){
						m_Anim.SetFloat("Speed",0.0f);
					}
					if (m_Rigidbody2D != null) {
						m_Rigidbody2D.position = this.transform.position;
					}
					if (m_target_attention <= 0) {
						updateTarget ();
						m_target_attention = delay;
						m_hitTarget = false;
					}
				} else {
					Vector3 move;

					// update position
					if (m_Rigidbody2D != null){
						move = Vector3.MoveTowards (m_Rigidbody2D.position, m_target.position, speed);
						if (flip) checkFlip (move, m_Rigidbody2D.position);
						m_Rigidbody2D.MovePosition (move);
					}else{
						move = Vector3.MoveTowards (this.transform.position, m_target.position, speed);
						if (flip) checkFlip (move, this.transform.position);
						this.transform.position = move;
					}
					if (m_Anim != null){			
						try{
							m_Anim.SetBool("Ground", true);
							m_Anim.SetBool("Crouch", false);
							m_Anim.SetFloat("vSpeed", (float)m_Rigidbody2D.velocity.y);
							m_Anim.SetFloat("Speed", (speed+1.0f)*0.085f);
						}
						catch{
						}
					}



				}

			}
		}

		private void checkFlip(Vector2 move, Vector3 position){
			if (move.x > position.x && !m_FacingRight) {
				Flip ();
			}else if (move.x < position.x && m_FacingRight) {
				Flip ();
			}	

		}
		private void Flip (){
			m_FacingRight = !m_FacingRight;
			// Multiply the player's x local scale by -1.
			Vector3 theScale = transform.localScale;
			theScale.x *= -1;
			transform.localScale = theScale;
		}



	}
}
	

