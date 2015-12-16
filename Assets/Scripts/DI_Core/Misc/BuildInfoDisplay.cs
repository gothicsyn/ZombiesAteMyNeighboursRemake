// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

public class BuildInfoDisplay : MonoBehaviour
{
	public Text displayText;

	public void OnEnable()
	{
		displayText.text = BuildInfo.buildDisplayName;
		DontDestroyOnLoad(displayText);
		DontDestroyOnLoad(this.gameObject);
	}
}