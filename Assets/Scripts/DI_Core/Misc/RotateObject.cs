// 	Devils Inc Studios
// 	// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015, 2015
//
// 	TODO: Include a description of the file here.
//

using UnityEngine;

namespace DI_Game {
	[AddComponentMenu("Editor/Rotate Object")]
	public class RotateObject : UnityEngine.MonoBehaviour {
		public float rotationSpeed;
		private float newY;
		private float currentX;
		private float currentY;
		private float currentZ;
		public bool controlledByGameState = false;

		public void Start() {
			currentX = this.transform.localEulerAngles.x;
			currentZ = this.transform.localEulerAngles.z;
		}
		public void Update() {
//			if (controlledByGameState) {
//				if (GameStateController.instance.gameState != GameStates.PLAYING) {
//					return;
//				}
//			}
			currentY = this.transform.localEulerAngles.y;
			newY = MathLib.wrapAngle(currentY + rotationSpeed);
			this.transform.localEulerAngles = new Vector3(currentX,newY, currentZ);
		}
	}
}