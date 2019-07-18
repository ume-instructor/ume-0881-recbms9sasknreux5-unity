using UnityEngine;
using System.Collections;
namespace UME
{
    [AddComponentMenu("UME/Utility/ObjectFactory")]
    public class ObjectFactory : MonoBehaviour {

		public GameObject spawnObject;
		[Range(0.01f, 5.0f)] public float lifeSpan = 5.0f;
		[Range(0.1f, 10f)] public float spawnDelay = 1.0f;
		[Range(0.0f, 10000.0f)] public float force =100;
		[Range(0.0f, 1f)] public float randomSpread = 0;
		public bool vertical = false;
		public bool flip = true;
		public AudioClip spawnSound;
		public float volumeMax = 1.0f;
		[HideInInspector]public AudioSource audioSrc;

	    private float spawnTime;

		//Initialization
		void Start(){
			spawnTime = spawnDelay;
			if (spawnSound != null)
				spawnSound.LoadAudioData ();
	    }

		void Update(){
			// timer
	        if (spawnTime <= 0)
	        {
	            spawnTime = spawnDelay;
				if (spawnObject != null) 
					assembleGameObject();
	        }
	        else
	        {
	            spawnTime -= Time.deltaTime;
	        }

	    }

		void assembleGameObject()
		{
				// create and init gameobject
				GameObject obj = Instantiate (spawnObject, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
				
			    // add lifespan to object
				if (obj.GetComponent<Life> () == null ) 
					obj.AddComponent<Life>();
				obj.GetComponent<Life> ().LifeSpan = lifeSpan;

				// attach audio to obj so it dies with object
				if (spawnSound != null) {
					if (obj.GetComponent<AudioSource> () == null)
						obj.AddComponent<AudioSource> ();
					// setup audio
					audioSrc = obj.GetComponent<AudioSource> ();
					audioSrc.playOnAwake = false;
					audioSrc.priority = 1;
					//set clip to spawnSound
					audioSrc.clip = spawnSound;
					audioSrc.PlayOneShot (spawnSound, volumeMax);
				}

				// apply object factory settings to object
				
				//randomSpread
				float degree = Random.Range (-90.0f, 90.0f) * randomSpread;
				
				// cardinal direction
				Vector2 direction = Vector2.right;
				if (vertical){
					direction = Vector2.up;
				}

				//rotated direction
				direction = Quaternion.Euler(0, 0, degree) * direction;
				
				//flip
				float flip_val = -1.0f;
				if (flip) {
					flip_val = 1.0f;
				}
				Vector3 objScale = obj.transform.localScale;
				objScale.x *= flip_val;
				obj.transform.localScale = objScale;
				
				//apply force to rigidbody
				if (obj.GetComponent<Rigidbody2D> () != null){
					obj.GetComponent<Rigidbody2D> ().AddRelativeForce (direction*flip_val*force);
					obj.transform.Rotate (direction);
				}
				
		}
	}
}