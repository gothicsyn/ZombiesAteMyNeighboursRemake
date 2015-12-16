/*
*
* 	Devils Inc Studios
* 	// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015, 2015
*	
*	Disables the mesh renderer for editor only objects once the game starts.
*	This allows them to be seen while in editor mode, but not while in game.
*
*/

using UnityEngine;
using System.Collections;

namespace DI_Game {
	[AddComponentMenu("Editor/Editor Only Object")]
	public class EditorOnlyObject : MonoBehaviour {
		public void Start() {
			// Get the mesh renderer and disable it.
			this.gameObject.GetComponent<MeshRenderer>().enabled = false;
		}
	}
}
