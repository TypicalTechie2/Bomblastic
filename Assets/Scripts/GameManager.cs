using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button pauseButton;
    public Image inGameMenuImage;
    public Image BombButtonBackgroundImage;
    public bool hasGameBegun;
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
    public Joystick joystick;

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
        inGameMenuImage.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        playerJoystick.gameObject.SetActive(true);
        BombButtonBackgroundImage.gameObject.SetActive(true);
        audioManagerScript.ButtonOnClickAudio();
        audioManagerScript.ResumeMusic();
        Time.timeScale = 1.0f;
        inGameMenuImage.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
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
        audioManagerScript.RestartButtonClip();

        StartCoroutine(RestartGameDelay());
    }

    private IEnumerator RestartGameDelay()
    {
        yield return new WaitForSeconds(0.75f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator UIVisibleDelayAtStart()
    {
        toggleImage.gameObject.SetActive(false);
        keyImages.SetActive(false);
        playerControllerScript.joystick.gameObject.SetActive(false);
        playerControllerScript.scoreText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        BombButtonBackgroundImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        toggleImage.gameObject.SetActive(false);
        keyImages.SetActive(true);
        playerControllerScript.joystick.gameObject.SetActive(true);
        playerControllerScript.scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        BombButtonBackgroundImage.gameObject.SetActive(true);
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
        joystick.gameObject.SetActive(false);
        keyImages.gameObject.SetActive(false);
    }
}
