// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Misc/Projectile Blocker")]
public class ProjectileBlocker : MonoBehaviour
{
	public AudioClip zapSound;
	public float zapVolume = 0.25f;
	public bool allowPlayerAmmo = false;
	public bool allowTowerAmmo = false;

	public void OnTriggerEnter(Collider other)
	{
		if (!allowPlayerAmmo) {
			if (other.tag == "Player Ammo") {
				DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", zapSound, zapVolume);
				other.gameObject.SetActive(false);
			}
		}
		if (!allowTowerAmmo) {
			if (other.tag == "Tower Ammo") {
				DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", zapSound, zapVolume);
				other.gameObject.SetActive(false);
			}
		}
	}

	public void Update()
	{
		transform.parent.GetComponent<Rigidbody>().WakeUp();
	}
}