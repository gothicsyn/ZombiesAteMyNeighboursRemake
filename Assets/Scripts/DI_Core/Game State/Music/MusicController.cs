// Devils Inc Studios
// Copyright DEVILS INC. STUDIOS LIMITED 2014, 2015
//
// TODO: Include a description of the file here.
//

#region Includes
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
#endregion

public enum Fade
{
	IN,
	OUT
}

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("Game State/Music Controller")]
public class MusicController : MonoBehaviour
{
	#region Events Used
	/*
	 * Listening:
	 * OnPlayEffect
	 * OnEnterCombat
	 * OnExitCombat
	 */
	#endregion

	#region Public Variables
	[Header("Out of Combat Background Tracks")]
	// The list of background music clips
	public List<AudioClip> bgm;
	[Header("In Combat Background Tracks")]
	// The list of in combat background music clips
	public List<AudioClip> bgmCombat;
	[Header("Audio Sources")]
	// The first audiosource for the background music
	public AudioSource bgmOne;
	// The second audiosource for the background music
	public AudioSource bgmTwo;
	// The third audiosource for the sound effects
	public AudioSource audioEffect;
	public AudioSource voiceOverPlayer;

	[Header("Fade Settings")]
	[Tooltip("Time in seconds fade should take")]
	public float fadeTime = 0.1f;
	[Header("Volume Settings")]
	// The volume to play the background music at
	public float bgmVolume = 0.25f;
	public float bgmVolumeOne = 0.25f;
	public float bgmVolumeTwo = 0.20f;

	// Is the player in combat
	[HideInInspector]
	public bool inCombat = false;
	#endregion

	#region Public Methods
	public void OnEnable()
	{
		bgmOne = GetComponents<AudioSource>()[0];
		bgmTwo = GetComponents<AudioSource>()[1];
		audioEffect = GetComponents<AudioSource>()[2];
		voiceOverPlayer = GetComponents<AudioSource>()[3];
		bgmOne.clip = bgm[UnityEngine.Random.Range(0, bgm.Count - 1)];
		bgmTwo.clip = bgmCombat[UnityEngine.Random.Range(0, bgmCombat.Count - 1)];

		DI_Events.EventCenter<AudioClip, float>.addListener("OnPlayEffect", playClip);
		DI_Events.EventCenter<AudioClip, float, Vector3>.addListener("OnPlayEffectAtPoint", playClip);
		DI_Events.EventCenter<AudioClip, float>.addListener("OnVoiceOver", playVoiceOver);
		DI_Events.EventCenter.addListener("OnEnterCombat", handleCombatStart);
		DI_Events.EventCenter.addListener("OnExitCombat", handleCombatEnd);

		bgmOne.Play();
		bgmTwo.Play();
		bgmTwo.volume = 0.0f;
	}
	
	public void OnDisable()
	{
		DI_Events.EventCenter<AudioClip, float>.removeListener("OnPlayEffect", playClip);
		DI_Events.EventCenter<AudioClip, float, Vector3>.removeListener("OnPlayEffectAtPoint", playClip);
		DI_Events.EventCenter<AudioClip, float>.removeListener("OnVoiceOver", playVoiceOver);
		DI_Events.EventCenter.removeListener("OnEnterCombat", handleCombatStart);
		DI_Events.EventCenter.removeListener("OnExitCombat", handleCombatEnd);
	}

	/// <summary>
	/// Handles the combat start.
	/// </summary>
	//TODO Fade in/out music
	public void handleCombatStart()
	{
		if (!inCombat) {
			inCombat = true;
			StartCoroutine(FadeAudio(bgmOne, Fade.OUT));
			StartCoroutine(FadeAudio(bgmTwo, Fade.IN));
		}
	}

	/// <summary>
	/// Handles the combat end.
	/// </summary>
	//TODO Fade in/out music
	public void handleCombatEnd()
	{
		if (inCombat) {
			StartCoroutine(FadeAudio(bgmOne, Fade.IN));
			StartCoroutine(FadeAudio(bgmTwo, Fade.OUT));
			inCombat = false;
		}
	}

	public void playClip(AudioClip clip, float volume, Vector3 position)
	{
		AudioSource.PlayClipAtPoint(clip, position, volume);
	}

	/// <summary>
	/// Plaies the clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	/// <param name="volume">Volume.</param>
	public void playClip(AudioClip clip, float volume)
	{
		audioEffect.clip = clip;
		audioEffect.volume = volume;
		audioEffect.Play();
	}

	/// <summary>
	/// Plaies the clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	/// <param name="volume">Volume.</param>
	public void playVoiceOver(AudioClip clip, float volume)
	{
		voiceOverPlayer.clip = clip;
		voiceOverPlayer.volume = volume;
		voiceOverPlayer.Play();
	}

	/// <summary>
	/// Plaies the clip.
	/// </summary>
	/// <param name="clip">Clip.</param>
	public void playClip(AudioClip clip)
	{
		playClip(clip, 1.0f);
	}

	IEnumerator FadeAudio(AudioSource source, Fade fadeType)
	{
		float start = fadeType == Fade.IN ? 0.0f : bgmVolume;
		float end = fadeType == Fade.IN ? bgmVolume : 0.0f;
		float fadeTimer = 0.0f;
		float fadeStep = 1.0f/fadeTime;

		while (fadeTimer <= 1.0f) {
			fadeTimer += fadeStep * Time.deltaTime;
			source.volume = Mathf.Lerp(start, end, fadeTimer);
			yield return new WaitForSeconds(fadeStep * Time.deltaTime);
		}

		//We will reach this code once they have faded successfully
		source.volume = fadeType == Fade.IN ? bgmVolume : 0.0f;
	}
	#endregion
}