// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using System;

[Serializable]
public struct DialogLine
{
	[MultilineAttribute]
	public string line;
	[Header("Animation Settings")]
	public bool hasAnimation;
	public string animationName;
	[Header("Voiceover Settings Settings")]
	public bool hasVoiceOver;
	public AudioClip voiceOverClip;
	public float voiceOverVolume;
	[Header("Event Settings")]
	public bool firesEvent;
	public string eventName;
	public float eventDelay;
	[Header("Actor Settings")]
	public DialogActor actor;
}