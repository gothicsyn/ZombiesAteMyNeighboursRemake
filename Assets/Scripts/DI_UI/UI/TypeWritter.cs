// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014
//
// TODO: Include a description of the file here.
//

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

[AddComponentMenu("UI/Effects/Type Writter")]
public class TypeWritter : MonoBehaviour
{
	public float typingSpeed = 0.05f;
	public AudioClip typingSound;
	public float typingVolume = 0.1f;
	public Text textBox;

	public void stopTyping()
	{
		StopCoroutine("typeLetter");
	}

	public void typeMessage(string message)
	{
		textBox.text = "";
		message = message.Replace("\\n", Environment.NewLine);
		int position = 1;
		foreach (char letter in message.ToCharArray()) {
			TypeWritterArgs args = default(TypeWritterArgs);
			args.message = letter + "";
			args.delay = position * typingSpeed;
			StartCoroutine("typeLetter", args);
			++position;
		}
	}

	public IEnumerator typeLetter(TypeWritterArgs args)
	{
		yield return new WaitForSeconds(args.delay);
		textBox.text = textBox.text + args.message;
		DI_Events.EventCenter<AudioClip, float>.invoke("OnPlayEffect", typingSound, typingVolume);
	}
}