// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Character/Physics Enabled Actor")]
[RequireComponent(typeof(CharacterController))]
public class PhysicsEnabledActor : MonoBehaviour
{
	public float pushPower = 3.0f;

	void OnControllerColliderHit(ControllerColliderHit hit) {
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body == null || body.isKinematic) {
			return;
		}

		Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
		body.velocity = pushDir * pushPower;
	}
}