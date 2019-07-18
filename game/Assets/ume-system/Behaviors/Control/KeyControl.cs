using System;
using UnityEngine;
namespace UME{
	[AddComponentMenu("UME/Control/KeyControl")]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
    public class KeyControl : MonoBehaviour
	{
		[SerializeField] public LayerMask GroundLayer; 
		public KeyCode jumpKey = KeyCode.Space;
		public KeyCode leftKey = KeyCode.LeftArrow;
		public KeyCode rightKey = KeyCode.RightArrow;
		public KeyCode flyKey = KeyCode.LeftShift;

        [SerializeField] public float m_MaxSpeed = 10f;   

		[SerializeField] public float m_JumpForce = 50f;  
		[SerializeField][Range(0.1f,0.5f)]public float JumpDuration = 0.25f;

		[SerializeField] public bool m_AirControl = true;
		[SerializeField]public bool doubleJump = false;
		[SerializeField]public bool alwaysFly = false;
	
		[SerializeField] public bool MoveForce = false;
		[SerializeField] public bool relativeForce = false;

		[SerializeField]public float Fuel=0;


		private bool m_Grounded; 
        private Transform m_GroundCheck;    
        private float k_GroundedRadius = .25f;

        
		private bool canJump = true;
		private bool jumping = true;
		private int jumpMax = 1;
		private int jumpCount;


		private float jump_duration;
		private double accumJumpForce;
		private Animator m_Anim;           
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  


		
	private void Start()
        {

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
        }


		//unstick to object if it is moving
		private void OnCollisionExit2D(Collision2D other){
			if (other.transform.position.y < gameObject.transform.position.y && other.gameObject.CompareTag ("MovingGround")) {
				gameObject.transform.parent = null;
			}

		}
		//stick to object if it is moving
		private void OnCollisionStay2D(Collision2D other){
			if(other.transform.position.y < gameObject.transform.position.y && other.gameObject.CompareTag("MovingGround"))
				gameObject.transform.parent = other.transform;

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
						if (m_Anim) {
							m_Anim.SetBool ("Ground", m_Grounded);
							m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
						}

					}

				}
			}
		}

		public void Update(){
			GroundCheck ();


		}

		public void FixedUpdate()
        {
			//Debug.Log (string.Format("{0}",m_Grounded));
			float horiz = 0.0f;
			if (Input.GetKey(leftKey))
				horiz = -1.0f;
			if (Input.GetKey(rightKey))
				horiz = 1.0f;

			if (m_Anim) {
				m_Anim.SetBool ("Ground", m_Grounded);
				m_Anim.SetFloat ("vSpeed", m_Rigidbody2D.velocity.y);
			}
            //move x
			if (m_Grounded || m_AirControl && horiz != 0f)
            {
				if (!MoveForce)
					m_Rigidbody2D.velocity = new Vector2 (horiz * m_MaxSpeed, m_Rigidbody2D.velocity.y);
				else {
					if (relativeForce)
						m_Rigidbody2D.AddRelativeForce (new Vector2 (horiz * m_MaxSpeed, 0f));
					if (!relativeForce)
						m_Rigidbody2D.AddForce (new Vector2 (horiz * m_MaxSpeed, 0f));

					m_Rigidbody2D.velocity = new Vector2 (Mathf.Clamp (m_Rigidbody2D.velocity.x, -m_MaxSpeed, m_MaxSpeed), m_Rigidbody2D.velocity.y);
				}
					if (Input.GetKey(rightKey) && !m_FacingRight)
					Flip();
				if (Input.GetKey(leftKey) && m_FacingRight)
                    Flip();
				if(m_Anim)
					m_Anim.SetFloat("Speed", Mathf.Abs(horiz));
            }
			//fly
			if (Input.GetKey(flyKey) && !m_Grounded && (Fuel > 0f || alwaysFly)) {
				//stop falling
				m_Rigidbody2D.velocity = new Vector2 (m_Rigidbody2D.velocity.x, Mathf.Max (m_Rigidbody2D.velocity.y, 0f));
				if(relativeForce)
					m_Rigidbody2D.AddRelativeForce (new Vector2 (0f, m_JumpForce));
				if(!relativeForce)
					m_Rigidbody2D.AddForce (new Vector2 (0f, m_JumpForce));

				m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y,0,m_MaxSpeed));
				Fuel --;
			}
			//doublejump
			if (Input.GetKeyDown (jumpKey) && !Input.GetKey (flyKey)) {
				//force button reset
				canJump = true;
				if (doubleJump && !m_Grounded && jumping && jumpCount < jumpMax) {
					jumpCount++;
					jump_duration = 0f;

					m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Max(m_Rigidbody2D.velocity.y,0f));
					if(relativeForce)
						m_Rigidbody2D.AddRelativeForce (new Vector2 (0f, (float)accumJumpForce));
					if(!relativeForce)
						m_Rigidbody2D.AddForce (new Vector2 (0f, (float)accumJumpForce));
				}
			}
			//jump
			if (Input.GetKey (jumpKey) && !Input.GetKey(flyKey) && canJump) {
				m_Grounded = false;
				gameObject.transform.parent = null;
				jump_duration -= Time.deltaTime;
				//Debug.Log(string.Format("scale: {0}", scale));
				if (jump_duration > 0f) {
					jumping = true;
					float scale = (jump_duration / JumpDuration);
					double force = m_JumpForce * scale;
					accumJumpForce += force;
					if(relativeForce)
						m_Rigidbody2D.AddRelativeForce (new Vector2 (0f, (float)force));
					if(!relativeForce)
						m_Rigidbody2D.AddForce (new Vector2 (0f, (float)force));
				} else {
					//force button reset
					canJump = false;
				}
			
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
			Component[] objectFactories;
			objectFactories = GetComponentsInChildren<ObjectFactory> (true);
			foreach (ObjectFactory of in objectFactories)
				of.flip = !of.flip;
        }
    }
}
