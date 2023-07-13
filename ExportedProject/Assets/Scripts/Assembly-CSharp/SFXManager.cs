using UnityEngine;

public class SFXManager : MonoBehaviour
{
	public static SFXManager Instance;

	private AudioSource[] sfx;

	private AudioSource music;

	private float mMusicVolume;

	private float mEffectsVolume;

	private string mCurrentMusic = "none";

	private void Awake()
	{
		Instance = this;
		sfx = GetComponentsInChildren<AudioSource>();
	}

	private void Start()
	{
		mMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
		mEffectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1f);
		if ((bool)EnvironmentController.Instance)
		{
			if (EnvironmentController.currentRoom == Room.Bedroom)
			{
				PlayMusic("BedroomTheme");
			}
			else if (EnvironmentController.currentRoom == Room.Lair)
			{
				PlayMusic("LairTheme");
			}
			else
			{
				PlayMusic("ThemeMusic");
			}
		}
		else
		{
			PlayMusic("TitleMusic");
		}
	}

	public void SetVolumes(float m, float e)
	{
		mMusicVolume = m;
		mEffectsVolume = e;
		if ((bool)music)
		{
			music.volume = m;
		}
	}

	public void PlaySound(string s)
	{
		AudioSource[] array = sfx;
		foreach (AudioSource audioSource in array)
		{
			if (audioSource.clip.name == s)
			{
				audioSource.PlayOneShot(audioSource.clip, mEffectsVolume);
				return;
			}
		}
		Debug.LogError("Could not find sound effect " + s + " in sfx manager.");
	}

	public void StopSound(string s)
	{
		AudioSource[] array = sfx;
		foreach (AudioSource audioSource in array)
		{
			if (audioSource.clip.name == s)
			{
				audioSource.Stop();
				return;
			}
		}
		Debug.LogError("Could not find sound effect " + s + " in sfx manager.");
	}

	public void RestartMusic()
	{
		if ((bool)music && !music.isPlaying)
		{
			music.volume = mMusicVolume;
			music.Play();
		}
	}

	public void PlayMusic(string s)
	{
		if (!(mCurrentMusic != s))
		{
			return;
		}
		mCurrentMusic = s;
		if ((bool)music)
		{
			music.Stop();
		}
		AudioSource[] array = sfx;
		foreach (AudioSource audioSource in array)
		{
			if (audioSource.clip.name == s)
			{
				music = audioSource;
				audioSource.volume = mMusicVolume;
				audioSource.Play();
				return;
			}
		}
		Debug.LogError("Could not find sound effect " + s + " in sfx manager.");
	}

	public void StopMusic()
	{
		if ((bool)music)
		{
			music.Stop();
		}
	}
}
