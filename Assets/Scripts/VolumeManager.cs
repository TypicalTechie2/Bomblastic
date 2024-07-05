using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public static VolumeManager instance;
    public string backgroundMusicTag = "BackgroundMusic"; // Tag used to identify background music AudioSources

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Ensure this object persists across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load the saved volume setting
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        SetVolume(savedVolume);
    }

    public void SetVolume(float volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        UpdateAllAudioSources(volume);
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("Volume", 1.0f);
    }

    private void UpdateAllAudioSources(float volume)
    {
        // Find all AudioSources tagged as background music and update their volume
        GameObject[] backgroundMusicObjects = GameObject.FindGameObjectsWithTag(backgroundMusicTag);
        foreach (var obj in backgroundMusicObjects)
        {
            AudioSource audioSource = obj.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }
    }
}
