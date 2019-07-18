using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace UME
{
    [AddComponentMenu("UME/Triggers/TeleportTrigger")]
    public class TeleportTrigger : MonoBehaviour {
		
		public GameObject[] teleportLocation; 
		[SerializeField] [Range(0f,5f)] public float teleportDelay=5.0f;
		public string triggerTag = "Player";
		public bool x = true;
		public bool y = true;
		bool hideTarget = false;

		//private Queue<GameObject> teleObjects;
		private List<Timer> teleObjects = new List<Timer>();

		//System.Collections.Stack

		// Use this for initialization
		void Start(){
			if (hideTarget) {
				// hide target sprites
				foreach (GameObject obj in teleportLocation) {
					obj.GetComponent<SpriteRenderer> ().enabled = false;
				}
			}

		}

		void Update(){
			//list store items to be deleted
			List<Timer> teleported = new List<Timer> ();

			// search for completed timers
			foreach (Timer m_timer in teleObjects) {
				m_timer.Clock -= Time.deltaTime;
				if (m_timer.Clock <= 0){
					//teleport the object
					if (m_timer.Target != null) {
						m_timer.Target.SetActive (true);
						teleportGameObject (m_timer.Target);
						Debug.Log("Teleport Exit: " + m_timer.Target.name);
					}
					//store index for deferred removal outside of enumeration

					teleported.Add(m_timer);
				}

			}
			//clean up

			foreach (Timer m_timer in teleported) {
					teleObjects.Remove(m_timer);
			}

		}

		void OnTriggerEnter2D (Collider2D other)
		{
			if (other.tag.ToLower () == "untagged") {return;}
			if(other.tag.ToLower() == triggerTag.ToLower() || (triggerTag.ToLower() == "all" )){
				if (other.gameObject.transform.parent == null) {
					//deactivate object
					Timer m_timer = ScriptableObject.CreateInstance<Timer>();
					m_timer.Clock = teleportDelay;
					m_timer.Target = other.gameObject;
					m_timer.Target.SetActive (false);
					Debug.Log ("Teleport Enter: " + other.gameObject.name);
					//store in list
					teleObjects.Add (m_timer);
				}
			}
		}

		void teleportGameObject(GameObject obj){
			int spawnPointIndex = 0;
			if (teleportLocation.Length > 1) {
				spawnPointIndex = Random.Range (0, teleportLocation.Length);
			}
			if (obj != null) {
				Vector3 tpos = teleportLocation [spawnPointIndex].transform.position;
				Vector3 pos = obj.transform.position;
				if (x) {
					pos.x = tpos.x;
				}
				if (y) {
					pos.y = tpos.y;
				}
				obj.transform.position = pos;
			}
					
		}
		
	}
}