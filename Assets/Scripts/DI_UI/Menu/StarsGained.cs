// // Devils Inc Studios
// // Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// //
// // TODO: Include a description of the file here.
// //

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StarsGained : MonoBehaviour {
	public StarAmountGainable easyStars, mediumStars, hardStars, insaneStars;
	public Image starOne, starTwo, starThree, starFour, starFive;
	public DifficultyTypes difficulty;



	// Use this for initialization
	void Start () {
		easyStars = StarAmountGainable.THREE;
		mediumStars = StarAmountGainable.FOUR;
		hardStars = StarAmountGainable.TWO;
		insaneStars = StarAmountGainable.ONE;
		starOne.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		difficulty = (DifficultyTypes)PlayerPrefs.GetInt("Difficulty");

		if(difficulty == DifficultyTypes.EASY){
			SetStarView(easyStars);
		}

		if(difficulty == DifficultyTypes.MEDIUM){
			SetStarView(mediumStars);
		}
		if(difficulty == DifficultyTypes.HARD){
			SetStarView(hardStars);
		}

		if(difficulty == DifficultyTypes.INSANE){
			SetStarView(insaneStars);
		}

	}

	void SetStarView(StarAmountGainable starAmount){
		if(starAmount == StarAmountGainable.ONE){
			starOne.enabled = true;
			starTwo.enabled = false;
			starThree.enabled = false;
			starFour.enabled = false;
			starFive.enabled = false;
		}else if(starAmount == StarAmountGainable.TWO){
			starOne.enabled = true;
			starTwo.enabled = true;
			starThree.enabled = false;
			starFour.enabled = false;
			starFive.enabled = false;
		}else if(starAmount == StarAmountGainable.THREE){
			starOne.enabled = true;
			starTwo.enabled = true;
			starThree.enabled = true;
			starFour.enabled = false;
			starFive.enabled = false;
		}else if(starAmount == StarAmountGainable.FOUR){
			starOne.enabled = true;
			starTwo.enabled = true;
			starThree.enabled = true;
			starFour.enabled = true;
			starFive.enabled = false;
		}else if( starAmount == StarAmountGainable.FIVE){
			starOne.enabled = true;
			starTwo.enabled = true;
			starThree.enabled = true;
			starFour.enabled = true;
			starFive.enabled = true;
		}else{
			starOne.enabled = false;
			starTwo.enabled = false;
			starThree.enabled = false;
			starFour.enabled = false;
			starFive.enabled = false;
		}

	}
}
