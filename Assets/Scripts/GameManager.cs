using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button pauseButton;
    public Image inGameMenuImage;
    public bool hasGameBegun;
    private AudioManager audioManagerScript;
    public Image restartMenuImage;
    public PlayerController playerControllerScript;

    private void Awake()
    {
        audioManagerScript = FindObjectOfType<AudioManager>();
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
        audioManagerScript.PauseMusic();
        Time.timeScale = 0.0f;
        pauseButton.gameObject.SetActive(false);
        inGameMenuImage.gameObject.SetActive(true);
    }

    public void ResumeGame()
    {
        audioManagerScript.CloseButtonAudio();
        audioManagerScript.ResumeMusic();
        Time.timeScale = 1.0f;
        inGameMenuImage.gameObject.SetActive(false);
        pauseButton.gameObject.SetActive(true);
    }

    public void ReturnToMenu()
    {
        audioManagerScript.CloseButtonAudio();
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    public void RestartGame()
    {
        playerControllerScript.currentScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator PauseButtonShowDelay()
    {
        pauseButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(2);

        pauseButton.gameObject.SetActive(true);
    }

    public void RestartMenu()
    {
        restartMenuImage.gameObject.SetActive(true);
    }
}
