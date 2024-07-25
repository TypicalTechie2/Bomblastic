using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public Button pauseButton;
    public Image inGameMenuImage;
    public Image BombButtonBackgroundImage;
    public AudioManager audioManagerScript;
    public Image restartMenuImage;
    public GameObject keyImages;
    public PlayerController playerControllerScript;
    public Joystick playerJoystick;
    public Image toggleImage;
    public GameObject musicVolumeObject;
    public Button resumeButton;
    public Button homeButton;
    public Button musicButton;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UIVisibleDelayAtStart());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGame()
    {
        playerJoystick.gameObject.SetActive(false);
        BombButtonBackgroundImage.gameObject.SetActive(false);
        audioManagerScript.ButtonOnClickAudio();
        Time.timeScale = 0.0f;
        pauseButton.gameObject.SetActive(false);
        keyImages.gameObject.SetActive(false);
        inGameMenuImage.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        playerJoystick.gameObject.SetActive(true);
        keyImages.gameObject.SetActive(true);
        BombButtonBackgroundImage.gameObject.SetActive(true);
        audioManagerScript.ButtonOnClickAudio();
        audioManagerScript.ResumeMusic();
        Time.timeScale = 1.0f;
        inGameMenuImage.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);

#if UNITY_EDITOR
        // Check if the active build target is Android
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            playerJoystick.gameObject.SetActive(true);
            BombButtonBackgroundImage.gameObject.SetActive(true);
            toggleImage.gameObject.SetActive(true);
        }
        else
        {
            playerJoystick.gameObject.SetActive(false);
            BombButtonBackgroundImage.gameObject.SetActive(false);
            toggleImage.gameObject.SetActive(false);
        }
#else
        // Enable joystick, toggleImage, and BombButton Background Image only on Android builds
        if (Application.platform == RuntimePlatform.Android)
        {
            playerJoystick.gameObject.SetActive(true);
            BombButtonBackgroundImage.gameObject.SetActive(true);
            toggleImage.gameObject.SetActive(true);
        }
        else
        {
            playerJoystick.gameObject.SetActive(false);
            BombButtonBackgroundImage.gameObject.SetActive(false);
            toggleImage.gameObject.SetActive(false);
        }
#endif
    }

    public void ReturnToMenu()
    {
        ScoreManager.instance.ResetScore();
        audioManagerScript.ButtonOnClickAudio();
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;
    }

    public void RestartGame()
    {
        ScoreManager.instance.ResetScore();
        StartCoroutine(audioManagerScript.RestartButtonClip());
        StartCoroutine(RestartGameDelay());

    }

    private IEnumerator RestartGameDelay()
    {
        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator UIVisibleDelayAtStart()
    {
        toggleImage.gameObject.SetActive(false);
        keyImages.SetActive(false);
        playerControllerScript.joystick.gameObject.SetActive(false);
        playerControllerScript.scoreText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        BombButtonBackgroundImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        pauseButton.gameObject.SetActive(true);
        keyImages.SetActive(true);
        playerControllerScript.scoreText.gameObject.SetActive(true);

#if UNITY_EDITOR
        // Check if the active build target is Android
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            toggleImage.gameObject.SetActive(true);
            keyImages.SetActive(true);
            playerControllerScript.joystick.gameObject.SetActive(true);
            BombButtonBackgroundImage.gameObject.SetActive(true);
        }
#else
        // Enable joystick, toggleImage, and BombButton Background Image only on Android builds
        if (Application.platform == RuntimePlatform.Android)
        {
            toggleImage.gameObject.SetActive(true);
            keyImages.SetActive(true);
            playerControllerScript.joystick.gameObject.SetActive(true);
            BombButtonBackgroundImage.gameObject.SetActive(true);
        }
#endif
    }

    public void OpenVolumeMenu()
    {
        audioManagerScript.ButtonOnClickAudio();
        resumeButton.gameObject.SetActive(false);
        homeButton.gameObject.SetActive(false);
        musicButton.gameObject.SetActive(false);
        musicVolumeObject.SetActive(true);
    }

    public void CloseVolumeMenu()
    {
        audioManagerScript.ButtonOnClickAudio();
        musicVolumeObject.SetActive(false);
        resumeButton.gameObject.SetActive(true);
        homeButton.gameObject.SetActive(true);
        musicButton.gameObject.SetActive(true);
    }
    public void ShowRestartMenu()
    {
        restartMenuImage.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        toggleImage.gameObject.SetActive(false);
        BombButtonBackgroundImage.gameObject.SetActive(false);
        playerJoystick.gameObject.SetActive(false);
        keyImages.gameObject.SetActive(false);
    }
}
