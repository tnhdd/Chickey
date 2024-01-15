using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] Sound[] backgroundMusic, sfxSounds;
    [SerializeField] AudioSource musicSource, sfxSource;
    [SerializeField] AudioMixer musicMixer;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Sound sound = Array.Find(backgroundMusic, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            musicSource.clip = sound.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfxSounds, x => x.name == name);

        if (sound == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);

        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
