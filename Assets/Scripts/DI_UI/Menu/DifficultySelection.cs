using UnityEngine;
using System.Collections;

public class DifficultySelection : MonoBehaviour {

	public void EasySelection(){
		PlayerPrefs.SetInt("Difficulty", (int)DifficultyTypes.EASY);
	}

	public void MediumSelection(){
		PlayerPrefs.SetInt("Difficulty", (int)DifficultyTypes.MEDIUM);
	}

	public void HardSelection(){
		PlayerPrefs.SetInt("Difficulty", (int)DifficultyTypes.HARD);
	}

	public void InsaneSelection(){
		PlayerPrefs.SetInt("Difficulty", (int)DifficultyTypes.INSANE);
	}
}
