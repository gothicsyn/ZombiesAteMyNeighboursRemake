// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
[AddComponentMenu("Misc/Teleport Player")]
public class TeleportPlayer : MonoBehaviour
{
	public Transform playerRespawn;
	public AudioClip warpSound;
	public float warpVolume = 0.25f;

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			other.transform.position = playerRespawn.position;
			DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", warpSound, warpVolume);
		}
	}
}