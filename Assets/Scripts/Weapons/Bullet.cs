using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public Rigidbody 	projectile;			// Will require a rigidbody prefab to act as a prejectile.  
	public float 		speed;				// Sets the objects speed travelling away from the spawn
	public AudioClip	SFX;				// Add a desired sound effect
	new AudioSource 		audio;

	// Use this for initialization
	void Start () {
		audio =  GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	// Keyboard Input
		if(Input.GetKeyUp("space")) {
			Rigidbody instantiatedProjectile = Instantiate(projectile,			// Projectile 
			                                               transform.position, 	// Position in the world
			                                               transform.rotation) 	// Rotation in the world
															as Rigidbody;		// Converts back to rigidbody object

		// Make the oject move through the world
			instantiatedProjectile.velocity = transform.TransformDirection(new Vector3(0, 0, speed));
			audio.PlayOneShot(SFX, 0.7F);

		// Turn off collisions with root object
			Physics.IgnoreCollision(instantiatedProjectile.GetComponent<Collider>(), transform.root.GetComponent<Collider>());
		}
	}
}
