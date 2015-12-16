// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("Dialogs/Dialog Consumer")]
public class DialogConsumer : MonoBehaviour
{
	[Header("Dialog Setup")]
	public string dialogName;

	[Header("Actor Setup")]
	public List<DialogActorGameObjects> actors;
	public DialogScript script;

	[Header("Trigger Setup")]
	public DialogTriggerTypes triggerType;
	public float dialogTimer = 0.0f;
	public string triggerTagFilter;

	public List<GameObject> cleanUpAvatars;

	public void OnEnable()
	{
		cleanUpAvatars = new List<GameObject>();
		if (triggerType == DialogTriggerTypes.TRIGGER_ON_START) {
			startScript();
		}
		if (triggerType == DialogTriggerTypes.TRIGGER_ON_EVENT) {
			DI_Events.EventCenter<string, DialogEventTypes>.addListener("OnDialogEvent", handleDialogEvent);
		}
		if (triggerType == DialogTriggerTypes.TRIGGER_ON_TIMER) {
			StartCoroutine("delayedStart");
		}
	}

	
	public void OnDisable()
	{
		if (triggerType == DialogTriggerTypes.TRIGGER_ON_EVENT) {
			DI_Events.EventCenter<string, DialogEventTypes>.removeListener("OnDialogEvent", handleDialogEvent);
		}
	}

	public void OnTriggerEnter(Collider other) {
		if (triggerType == DialogTriggerTypes.TRIGGER_ON_TRIGGER) {
			if (other.tag == triggerTagFilter) {
				startScript();
			}
		}
	}

	public void handleDialogEvent(string dialog, DialogEventTypes eventType) {
		if (dialog == dialogName) {
			switch (eventType) {
				case DialogEventTypes.EVENT_START_DIALOG:
					startScript();
					break;
				case DialogEventTypes.EVENT_END_DIALOG:
					endScript();
					break;
			}
		}
	}

	public void readyActor(string actorName)
	{
		#if DEBUG
		Debug.Log("Ready Actor");
		#endif
		foreach (DialogActorGameObjects actorObjects in actors.ToArray()) {
			if (actorObjects.actor.actorName == actorName) {
				if (!actorObjects.actorAvatar.activeSelf) {
					actorObjects.actorAvatar.SetActive(true);
					#if DEBUG
					Debug.Log("Adding actor to cleanup list - readyActor()");
					#endif
					cleanUpAvatars.Add(actorObjects.actorAvatar);
				}
			}
		}
	}

	public GameObject getActorAvatar(string actorName)
	{
		foreach (DialogActorGameObjects actorObjects in actors.ToArray()) {
			if (actorObjects.actor.actorName == actorName) {
				if (!actorObjects.actorAvatar.activeSelf) {
					actorObjects.actorAvatar.SetActive(true);
					#if DEBUG
					Debug.Log("Adding actor to cleanup list - getActorAvatar()");
					#endif
					cleanUpAvatars.Add(actorObjects.actorAvatar);
				}
				return actorObjects.actorAvatar;
			}
		}
		return null;
	}

	public IEnumerator delayedStart()
	{
		yield return new WaitForSeconds(dialogTimer);
		startScript();
	}

	public void startScript()
	{
		DI_Events.EventCenter<DialogConsumer>.invoke("OnDialogUpdate", this);
		DI_Events.EventCenter.invoke("OnDialogStart");
	}

	public void endScript()
	{
		DI_Events.EventCenter.invoke("OnDialogEnd");
		if (cleanUpAvatars.Count > 0) {
			foreach (GameObject avatar in cleanUpAvatars.ToArray()) {
				avatar.SetActive(false);
			}
		}
	}
}