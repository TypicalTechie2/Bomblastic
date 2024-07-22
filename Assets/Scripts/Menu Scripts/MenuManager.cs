using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public AudioSource menuAudio;
    public AudioClip playButtonClip;
    public AudioClip exitButtonClip;
    public AudioClip ControlsButtonClip;
    public GameObject characterA;
    public GameObject characterB;
    public Image titleImage;
    public Image backgroundImage;
    public Image howToPlayImage;
    public GameObject menuScreen;
    public Button playButton;
    public Button menuButton;
    public Button exitButton;
    public Button[] buttons;
    public Button howToPlayButton;
    private static bool isFirstLoad = true;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        if (isFirstLoad)
        {
            StartCoroutine(DisableButtonsForSeconds());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DisableButtonsForSeconds()
    {
        isFirstLoad = false;

        // Disable all buttons
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        // Wait for specified seconds
        yield return new WaitForSeconds(2f);

        // Enable all buttons again
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        StartCoroutine(startButtonDelay());
    }

    private IEnumerator startButtonDelay()
    {
        menuAudio.PlayOneShot(playButtonClip, 1f);

        yield return new WaitForSeconds(0.75f);

        characterA.SetActive(false);
        characterB.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void EnterMenuScreen()
    {
        menuAudio.PlayOneShot(ControlsButtonClip, 1f);
        titleImage.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void ExitMenuScreen()
    {
        menuAudio.PlayOneShot(ControlsButtonClip, 1f);
        menuScreen.SetActive(false);
        titleImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        howToPlayButton.gameObject.SetActive(true);
    }

    public void HowToPlayMenu()
    {
        menuAudio.PlayOneShot(ControlsButtonClip, 1f);
        titleImage.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        howToPlayButton.gameObject.SetActive(false);
        howToPlayImage.gameObject.SetActive(true);
    }

    public void CloseHowToPlayMenu()
    {
        menuAudio.PlayOneShot(ControlsButtonClip, 1f);
        howToPlayImage.gameObject.SetActive(false);
        titleImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        howToPlayButton.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameDelay());
    }

    public IEnumerator ExitGameDelay()
    {
        menuAudio.PlayOneShot(exitButtonClip, 1f);

        yield return new WaitForSeconds(0.75f);

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

        Application.Quit();

#endif
    }
}
