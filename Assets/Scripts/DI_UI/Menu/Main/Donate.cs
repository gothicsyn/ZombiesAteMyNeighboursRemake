// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;

namespace DI.Menus
{
	[AddComponentMenu("Menus/Main/Donate")]
	public class Donate : MonoBehaviour
	{
		public bool childsPlay = false;

		public void OnClick()
		{
			if (childsPlay) {
				Application.OpenURL("https://donate.childsplaycharity.org/");
			}
			else {
				Application.OpenURL("http://devilsincstudios.com/");
			}
		}
	}
}