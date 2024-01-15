using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] Slider musicSlider, sfxSlider;
    [SerializeField] AudioMixer musicMixer;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            MusicVolume();
            SFXVolume();
        }
    }

    public void MusicVolume()
    {
        float volume = musicSlider.value;
        musicMixer.SetFloat("music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("musicVolume", volume);
        // AudioManager.instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        float volume = sfxSlider.value;
        musicMixer.SetFloat("sfx", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        MusicVolume();
        SFXVolume();
    }
}
