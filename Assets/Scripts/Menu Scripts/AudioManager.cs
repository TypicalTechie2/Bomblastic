using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private const string VolumePrefKey = "volumePref";
    public AudioSource audioSource;
    public Slider volumeSlider;
    public AudioClip buttonClickClip;
    public AudioClip restartButtonClip;

    private void Awake()
    {
        LoadVolume();
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        SaveVolume(volume);
    }

    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }

    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey(VolumePrefKey))
        {
            float savedVolume = PlayerPrefs.GetFloat(VolumePrefKey);
            audioSource.volume = savedVolume;
            volumeSlider.value = savedVolume;
        }

        else
        {
            audioSource.volume = 1.0f; // Set to full volume by default
            volumeSlider.value = 1.0f; // Set slider to full volume by default
        }
    }

    public void PauseMusic()
    {
        audioSource.Pause();
    }

    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    public void RestartButtonClip()
    {
        audioSource.PlayOneShot(restartButtonClip, 1f);
    }

    public void ButtonOnClickAudio()
    {
        audioSource.PlayOneShot(buttonClickClip, 1f);
    }
}
