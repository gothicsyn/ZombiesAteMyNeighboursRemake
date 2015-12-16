// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

using UnityEngine;
#if UNITY_EDITOR
	using UnityEditor;
#endif

namespace DI.Menus
{
	[AddComponentMenu("Menus/Main/Exit")]
	public class Exit : MonoBehaviour
	{
		public void OnEnable()
		{
			#if UNITY_STANDALONE
						gameObject.SetActive(true);
			#else
						gameObject.SetActive(false);
			#endif
		}

		public void OnClick()
		{
			#if UNITY_EDITOR
				EditorApplication.Exit(0);
			#else
				Application.Quit();
			#endif
		}
	}
}