// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;

public class ThrusterEditor : MonoBehaviour
{
	private ShootEmUpController controller;
	public Text maxSpeed;
	public Text mainThruster;
	public Text turningThruster;
	public Slider maxSpeedSlider;
	public Slider mainThrusterSlider;
	public Slider turningThrusterSlider;


	public void OnEnable()
	{
		controller = GameStateController.instance.playerState.gameObject.GetComponent<ShootEmUpController>();
		maxSpeedSlider.value = controller.maxSpeed;
		mainThrusterSlider.value = controller.movementSpeed;
		turningThrusterSlider.value = controller.turningSpeed;
		mainThruster.text = "Main Force: " + controller.movementSpeed;
		turningThruster.text = "Turning Force: " + controller.turningSpeed;
		maxSpeed.text = "Max Speed: " + controller.maxSpeed;

	}

	public void updateMainThruster(float updatedValue)
	{
		controller.movementSpeed = updatedValue;
		mainThruster.text = "Main Force: " + controller.movementSpeed;
	}

	public void updateTurningThruster(float updatedValue)
	{
		controller.turningSpeed = updatedValue;
		turningThruster.text = "Turning Force: " + controller.turningSpeed;
	}

	public void updateMaxSpeed(float updatedValue)
	{
		controller.maxSpeed = updatedValue;
		maxSpeed.text = "Max Speed: " + controller.maxSpeed;
	}

}