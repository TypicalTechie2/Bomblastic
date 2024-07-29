using System.Collections;
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
    private bool audioPlayed = false;

    private void Awake()
    {
        LoadVolume();
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    // Sets the volume level and saves it to PlayerPrefs
    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
        SaveVolume(volume);
    }

    // Saves the volume level to PlayerPrefs
    public void SaveVolume(float volume)
    {
        PlayerPrefs.SetFloat(VolumePrefKey, volume);
    }

    // Loads the volume level from PlayerPrefs and applies it
    public void LoadVolume()
    {
        if (PlayerPrefs.HasKey(VolumePrefKey))
        {
            // Retrieve and apply saved volume
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

    // Pauses the audio playback
    public void PauseMusic()
    {
        audioSource.Pause();
    }

    // Resumes the audio playback
    public void ResumeMusic()
    {
        audioSource.UnPause();
    }

    // Plays the restart button sound and handles the cooldown to prevent overlapping
    public IEnumerator RestartButtonClip()
    {
        if (!audioPlayed)
        {
            audioPlayed = true;
            audioSource.PlayOneShot(restartButtonClip, 1f);
            yield return new WaitForSeconds(0.5f);
            audioPlayed = false;
        }
    }

    // Plays the button click sound
    public void ButtonOnClickAudio()
    {
        audioSource.PlayOneShot(buttonClickClip, 1f);
    }
}
