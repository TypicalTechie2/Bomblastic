using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider;

    private void Start()
    {
        // Set the slider's value to the saved volume
        float savedVolume = VolumeManager.instance.GetVolume();
        volumeSlider.value = savedVolume;

        // Add listener to the slider
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    private void OnVolumeSliderChanged(float volume)
    {
        // Update the volume in the VolumeManager
        VolumeManager.instance.SetVolume(volume);
    }
}
