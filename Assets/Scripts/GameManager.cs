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
    private AudioManager audioManagerScript;
    public Image restartMenuImage;
    public GameObject keyImages;
    public PlayerController playerControllerScript;
    public ScoreManager scoreManagerScript;

    private void Awake()
    {
        audioManagerScript = AudioManager.instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PauseButtonShowDelay());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseGame()
    {
        audioManagerScript.ButtonOnClickAudio();
        Time.timeScale = 0.0f;
        pauseButton.gameObject.SetActive(false);
        inGameMenuImage.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        audioManagerScript.ButtonOnClickAudio();
        audioManagerScript.ResumeMusic();
        Time.timeScale = 1.0f;
        inGameMenuImage.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void ReturnToMenu()
    {
        audioManagerScript.ButtonOnClickAudio();
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void RestartGame()
    {
        scoreManagerScript.ResetScore();
        audioManagerScript.PlayButtonAudio();
        playerControllerScript.currentScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator PauseButtonShowDelay()
    {
        keyImages.SetActive(false);
        playerControllerScript.joystick.gameObject.SetActive(false);
        playerControllerScript.scoreText.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(false);
        BombButtonBackgroundImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.5f);

        keyImages.SetActive(true);
        playerControllerScript.joystick.gameObject.SetActive(true);
        playerControllerScript.scoreText.gameObject.SetActive(true);
        pauseButton.gameObject.SetActive(true);
        BombButtonBackgroundImage.gameObject.SetActive(true);
    }

    public void RestartMenu()
    {
        restartMenuImage.gameObject.SetActive(true);
    }
}
