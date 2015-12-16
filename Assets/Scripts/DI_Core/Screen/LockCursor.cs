// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

[AddComponentMenu("Screen/Lock Cursor")]
public class LockCursor : MonoBehaviour
{
	public bool isCursorLocked = true;

	public void OnEnable()
	{
		if (isCursorLocked) {
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		else {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}