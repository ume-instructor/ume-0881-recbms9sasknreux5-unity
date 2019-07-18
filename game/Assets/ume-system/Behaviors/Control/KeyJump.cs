using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UME{
	[AddComponentMenu("UME/Control/KeyJump")]
	public class KeyJump : MonoBehaviour {

		[SerializeField] public LayerMask GroundLayer; 
		public KeyCode jumpKey = KeyCode.Space;

		[SerializeField] public float m_JumpForce = 50f;  
		[SerializeField][Range(0.1f,0.5f)]public float JumpDuration = 0.1f;
		[SerializeField]public bool doubleJump = false;


		private bool m_Grounded = true; 
		private Transform m_GroundCheck;    
		private float k_GroundedRadius = .25f;


		private bool canJump = true;
		private bool jumping = true;
		private int jumpMax = 1;
		private int jumpCount;


		private float jump_duration;
		private float accumJumpForce;
		private Animator m_Anim;           
		private Rigidbody2D m_Rigidbody2D;


		// Use this for initialization
		void Start () {
			
			m_GroundCheck = transform.Find("Feet");
			//build GroundCheck from collider
			if (!m_GroundCheck) {
				m_GroundCheck = GetComponent<Collider2D>().transform;
				k_GroundedRadius += GetComponent<Collider2D>().bounds.extents.y;
			}
			m_Anim = GetComponent<Animator>();
			m_Rigidbody2D = GetComponent<Rigidbody2D>();
			jump_duration = JumpDuration;
			accumJumpForce = 0f;
			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
			}

		
		}

		


		void FixedUpdate(){
			GroundCheck ();
			//doublejump
			if (Input.GetKeyDown (jumpKey)) {
				//force button reset
				canJump = true;
				if (doubleJump && !m_Grounded && jumping && jumpCount < jumpMax) {
					jumpCount++;
					jump_duration = 0f;
					// if falling (- y velocity) set vertical velocity to 0
					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Max(m_Rigidbody2D.velocity.y,0f));
					// add the rest of the accumforce
					m_Rigidbody2D.AddForce (new Vector2 (0f, (float)accumJumpForce*4.0f));
				}
			}
			//jump
			if (Input.GetKey (jumpKey) && canJump) {
				m_Grounded = false;
				gameObject.transform.parent = null;
				jump_duration -= Time.fixedDeltaTime;
				//Debug.Log(string.Format("scale: {0}", scale));
				if (jump_duration > 0f) {
					jumping = true;
					float scale = Mathf.Clamp01(jump_duration / JumpDuration);
					float force = m_JumpForce * scale;
					accumJumpForce += force;
					m_Rigidbody2D.AddForce (new Vector2 (0f, (float)force));
				} else {
					//force button reset
					canJump = false;
				}

			}
			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
				m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
			}

		
		}


		//use ground check
		private void GroundCheck(){
			if (m_GroundCheck != null) {
				Collider2D[] colliders = Physics2D.OverlapCircleAll (m_GroundCheck.position, k_GroundedRadius, GroundLayer);
				for (int i = 0; i < colliders.Length; i++) {
					if (colliders [i].gameObject != gameObject) {
						m_Grounded = true;
						jump_duration = JumpDuration;
						jumpCount = 0;
						jumping = false;
						accumJumpForce = 0f;

					}

				}
			}
		}

	}
}