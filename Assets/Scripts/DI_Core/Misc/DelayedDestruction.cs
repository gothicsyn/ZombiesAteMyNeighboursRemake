// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections;

[AddComponentMenu("Misc/Delayed Destruction")]
public class DelayedDestruction : MonoBehaviour
{
	[Header("Settings")]
	public float lifeTime = 3.0f;

	public void OnEnable()
	{
//		Debug.Log("Starting Timer" + System.DateTime.UtcNow.ToString());
		StartCoroutine(expire());
	}

	public IEnumerator expire()
	{
		yield return new WaitForSeconds(lifeTime);
//		Debug.Log("Timer Expired" + System.DateTime.UtcNow.ToString());
		this.gameObject.SetActive(false);
	}
}