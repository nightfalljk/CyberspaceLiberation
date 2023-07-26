using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms;

public class AudioManager : Singleton<AudioManager>
{

	[SerializeField] private AudioCustomSettings audioCustomSettings;
	[SerializeField] private AudioMixer otherSourcesAudioMixer;
	[SerializeField] private float min_dB = -80;
	[SerializeField] private float max_dB = 0;
	[SerializeField] private AnimationCurve dBToLinNormalized;
	[SerializeField] private float fade = 1;
	public Sound[] sounds;

	private List<AudioSource> OtherSoundSources;
	private List<string> queue = new List<string>();
	private bool musicFade;

	public override void OnAwake()
	{
		if (Instance == null)
			Instance = this;
		else if(Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		if (sounds == null)
		{
			Debug.LogWarning("No sounds available!");
			return;
		}
		foreach (var s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			s.source.time = s.startOffset;
			//s.source.playOnAwake = s.playOnStart;//Doesn't work?
		}
		UpdateVolumes();

//		OtherSoundSources = FindObjectsOfType<AudioSource>().ToList();
//		foreach (AudioSource otherSoundSource in OtherSoundSources)
//		{
//			otherSoundSource.volume = gameSettings.SoundsValue;
//		}
	}

	private void Start()
	{
		if (sounds == null)
		{
			Debug.LogWarning("No sounds available!");
			return;
		}
//		foreach (var s in sounds)
//		{
//			if (s.playOnStart)
//			{
//				Play(s);
//			}
//		}
	}

	private void Update()
	{
		UpdateVolumes();
		CheckQueue();
	}

	private void CheckQueue()
	{
		if (queue.Count != 0)
		{
			foreach (var s in sounds)
			{
				if (s.isMusic && s.source.isPlaying)
				{
					if (s.source.clip.length - s.source.time <= fade)
					{
						Stop(s,true);
						Play(queue[0],smooth:true);
						queue.Clear();
					}
				}
			}
		}
	}

	public void UpdateVolumes()
	{
		foreach (var s in sounds)
		{
			if (s.isMusic)
				s.source.volume = s.volume * audioCustomSettings.MusicValue * s.fadeVolume;
			else
				s.source.volume = s.volume * audioCustomSettings.SoundsValue * s.fadeVolume;
		}

		otherSourcesAudioMixer.SetFloat("AudioVolume", Mathf.Lerp(min_dB, max_dB, DBToLin(audioCustomSettings.SoundsValue)));

		if (!audioCustomSettings.Sounds)
		{
			otherSourcesAudioMixer.SetFloat("AudioVolume", min_dB);
		}
		if (!audioCustomSettings.Music)
		{
			foreach (var s in sounds)
			{
				if (s.isMusic)
				{
					s.source.volume = 0;
				}
			}
		}
		//OtherSoundSources = FindObjectsOfType<AudioSource>().ToList();//This overrides previous settings!!!
//		foreach (AudioSource otherSoundSource in OtherSoundSources)
//		{
//			otherSoundSource.volume = audioCustomSettings.SoundsValue;
//		}
	}

	public float DBToLin(float dbNormalized)
	{
		return dBToLinNormalized.Evaluate(dbNormalized);
	}
	
//	public void PlayNow(string name)
//	{
//		Play(name, 0);
//	}

	//delay or smooth, not both currently possible
	public void Play(string name, float delay = 0, bool smooth = false)
	{
		if (sounds == null)
		{
			Debug.LogWarning("No sounds active! Check if AudioManager is in stared Scene and it's gameObject is active!");
			return;
		}
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if (s.isMusic)
		{
			if (!audioCustomSettings.Music)
				return;
			if (AnySongPlaying())
			{
				Debug.Log("Another Song is already playing");
				return;
			}
		}
		else
		{
			if (!audioCustomSettings.Sounds)
				return;
		}

		if (s.source.isPlaying && s.singleton)
		{
			return;
		}
		
		if (delay > 0)
		{
			s.source.PlayDelayed(delay);
		}
		else
		{
			//play sound
			Play(s, smooth);
			//s.source.Play();
		}

		//revert volumechange
		//s.source.volume = vTmp;
	}

	private bool AnySongPlaying()
	{
		foreach (var s in this.sounds)
		{
			if (s.isMusic && s.source.isPlaying && !musicFade)
				return true;
		}

		return false;
	}

	public void PlayRandom(string[] names)
	{
		if (names.Length == 0)
		{
			Debug.LogWarning("No sounds to choose random");
		}
		string chosen = names[UnityEngine.Random.Range(0, names.Length)];
		Play(chosen);
	}

	public void Stop(string name, bool smooth = false)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		Stop(s, smooth);
	}

	public void Stop(Sound s, bool smooth = false)
	{
		if (!smooth)
		{
			s.source.Stop();
		}
		else
		{
			Fade(s, 1, 0, fade,true);
		}
	}
	public void Play(Sound s, bool smooth = false)
	{
		if (!smooth)
		{
			s.source.Play();
		}
		else
		{
			s.source.Play();
			Fade(s, 0, 1, fade);
		}
	}

	public void StopAllSounds(bool music, bool sounds)
	{
		StopAllSounds(music, sounds, null);
	}
	
	public void StopAllSounds(bool music, bool sounds, List<AudioHelper> exclusions)
	{
		foreach (var s in this.sounds)
		{
			if (exclusions != null)
			{
				AudioHelper audioHelper;
				audioHelper = exclusions.Find(x => x.name == s.name);
				if (audioHelper != null)
				{
					QueueMusic(audioHelper.nextToPlayName);
					continue;
				}
			}
			
			if (music && s.isMusic && s.source.isPlaying)
			{
				Stop(s, true);
			}
			if (sounds && !s.isMusic)
			{
				Stop(s);
			}
		}
	}

	private void QueueMusic(string audioHelperNextToPlayName)
	{
		queue.Add(audioHelperNextToPlayName);
	}

	private void Fade(Sound s, float from, float to, float time = 1, bool stopAfter = false)
	{
		StartCoroutine(CoFade(s, from, to, time, stopAfter));
	}

	private IEnumerator CoFade(Sound s, float from, float to, float time = 1, bool stopAfter = false)
	{
		if (s.isMusic)
			musicFade = true;
		
		float t = from;
		bool fadeUp = (from < to);
		while(fadeUp ? s.fadeVolume < to : s.fadeVolume > to)
		{
			float timeSinceLastFrame = Time.deltaTime;
			//float direction = from < to ? 1 : -1;
			float value;
			if (fadeUp)
			{
				t += (timeSinceLastFrame / time);// *direction;
				value = Mathf.Lerp(from, to, t);
			}
			else
			{
				t -= (timeSinceLastFrame / time);// *direction;
				value = Mathf.Lerp(to, from, t);
			}
			s.fadeVolume = value;
			yield return null;
		}

		if (s.isMusic)
			musicFade = false;
		
		if (stopAfter)
		{
			s.source.Stop();
			
		}
	}
}

[Serializable]
public class AudioHelper
{
	public string name;
	public string nextToPlayName;
}