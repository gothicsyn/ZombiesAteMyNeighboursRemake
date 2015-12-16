// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Collections;

[AddComponentMenu("Game State/Dialog/Dialog Controller")]
public class DialogController : MonoBehaviour
{
	[Header("GUI Element Setup")]
	public Image portrait;
	public RawImage portraitRender;
	public Text textBox;
	public Canvas dialogCanvas;
	public Text namePlate;
	public Text namePlateRender;
	public Text dialogNextInfo;
	
	[HideInInspector]
	public TypeWritter typeWritter;

	[Header("These are set by events and do not need to be modified")]
	public DialogScript currentScript;
	public DialogConsumer currentConsumer;

	[HideInInspector]
	public bool isReadyToAdvance = false;

	public void OnEnable()
	{
		typeWritter = GetComponent<TypeWritter>();
		DI_Events.EventCenter.addListener("OnDialogStart", handleDialogStart);
		DI_Events.EventCenter.addListener("OnDialogEnd", handleDialogEnd);
		DI_Events.EventCenter<DialogConsumer>.addListener("OnDialogUpdate", handleDialogUpdate);
		// TODO figure out how to change the [SPACE] to the registered key, Should only be an issue with PC builds though.
	}

	public void OnDisable()
	{
		DI_Events.EventCenter.removeListener("OnDialogStart", handleDialogStart);
		DI_Events.EventCenter.removeListener("OnDialogEnd", handleDialogEnd);
		DI_Events.EventCenter<DialogConsumer>.removeListener("OnDialogUpdate", handleDialogUpdate);
	}

	public void Update()
	{
		if (!isReadyToAdvance) {
			if (Input.GetButtonDown("Dialog Next")) {
				isReadyToAdvance = true;
			}
		}
	}

	public void handleDialogStart()
	{
		// TODO write something up so this snipet doesn't get lost its really useful.
		// Debug.Log(System.Reflection.MethodBase.GetCurrentMethod().Name);
		if (currentConsumer != null) {
			showDialog();
			StartCoroutine("playScript");
		}
	}

	public void handleDialogEnd()
	{
		typeWritter.stopTyping();
		hideDialog();
	}

	public void handleDialogUpdate(DialogConsumer consumer)
	{
		currentConsumer = consumer;
		currentScript = consumer.script;
	}

	public void changeSpeaker(string speakersName, Sprite newSpeakerPortrait)
	{
		#if DEBUG
		Debug.Log("Change Speaker - Sprite: " + speakersName);
		#endif
		namePlate.text = speakersName;
		portrait.sprite = newSpeakerPortrait;
		portrait.gameObject.SetActive(true);
		portraitRender.gameObject.SetActive(false);
	}
	public void changeSpeaker(string speakersName, RenderTexture newSpeakerPortrait)
	{
		#if DEBUG
		Debug.Log("Change Speaker - Render: " + speakersName);
		#endif
		namePlateRender.text = speakersName;
		portraitRender.texture = newSpeakerPortrait;
		portrait.gameObject.SetActive(false);
		portraitRender.gameObject.SetActive(true);
	}

	public void updateDialogText(string message, bool useTypeWritter)
	{
		if (useTypeWritter) {
			typeWritter.typeMessage(message);
		}
		else {
			textBox.text = message;
		}
	}

	public void showDialog()
	{
		MenuController.instance.disableAllMenus();
		dialogCanvas.gameObject.SetActive(true);
		MenuController.instance.hud.gameObject.SetActive(false);
		GameStateController.instance.enterCinemaNoSkip();
	}

	public void hideDialog()
	{
		MenuController.instance.hud.gameObject.SetActive(true);
		dialogCanvas.gameObject.SetActive(false);
		GameStateController.instance.exitCinemaNoSkip();
	}

	public IEnumerator playScript()
	{
		// Don't do anything if we are in a paused state.
		while (GameStateController.instance.gameState == GameStates.IN_MENU || GameStateController.instance.gameState == GameStates.PAUSED) {
			yield return new WaitForSeconds(0.1f);
		}

		for (int currentLine = 0; currentLine < currentScript.lines.Count; currentLine++) {		
			DialogLine line = currentScript.lines[currentLine];
			if (!line.Equals(null)) {
				if (line.actor == null) {
					Debug.LogError("Actor is null.");
				}
				else {
					#if DEBUG
						Debug.Log(line.actor);
					#endif
					currentConsumer.readyActor(line.actor.actorName);
					if (line.actor.actorRender == null) {
						changeSpeaker(line.actor.actorName, line.actor.actorPortrait);
					}
					else {
						changeSpeaker(line.actor.actorName, line.actor.actorRender);
					}
				}
				if (line.hasAnimation) {
					GameObject avatar = currentConsumer.getActorAvatar(line.actor.actorName);
					if (avatar != null) {
						avatar.GetComponent<Animator>().Play(line.animationName);
						#if DEBUG
							Debug.Log("Attempting to play animation: " + line.animationName);
						#endif
					}
				}

				if (line.firesEvent) {
					if (line.eventName != null) {
						#if DEBUG
							Debug.Log("Playing Event: " + line.eventName + " with a delay of " + line.eventDelay);
						#endif
						yield return new WaitForSeconds(line.eventDelay);
						DI_Events.EventCenter.invoke(line.eventName);
					}
				}

				if (line.hasVoiceOver) {
					DI_Events.EventCenter<AudioClip, float>.invoke("OnVoiceOver", line.voiceOverClip, line.voiceOverVolume);
				}

				#if DEBUG
					Debug.Log(line.line);
				#endif
				typeWritter.typeMessage(line.line);

				// Wait for the player to press the next button [Default space]
				while (!isReadyToAdvance) {
					yield return new WaitForSeconds(0.1f);
				}
				isReadyToAdvance = false;
				typeWritter.stopTyping();
			}
		}

		currentConsumer.endScript();
		hideDialog();
	}
}
