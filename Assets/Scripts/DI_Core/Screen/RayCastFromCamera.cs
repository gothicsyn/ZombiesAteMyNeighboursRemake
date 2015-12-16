// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Screen/Raycast From Camera")]
public class RayCastFromCamera : MonoBehaviour
{
	public LayerMask mask;

	public void FixedUpdate()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		Debug.DrawRay(ray.origin, ray.direction, Color.red, 0.1f, true);
		if (Physics.Raycast(ray, out hit, 3.0f, mask)) {
			hit.collider.SendMessage("OnRayHit");
		}
	}
}