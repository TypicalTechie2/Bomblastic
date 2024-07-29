using System.Collections;
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
    private bool audioPlayed = false;

    // Start is called before the first frame update
    void Start()
    {
        // If this is the first time the game has been loaded, disable buttons for a few seconds
        if (isFirstLoad)
        {
            StartCoroutine(DisableButtonsForSeconds());
        }
    }

    // Coroutine to disable buttons for a few seconds on first load
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

    // Method to start the game when the play button is pressed
    public void StartGame()
    {
        StartCoroutine(startButtonDelay());
    }

    // Coroutine to handle delay and audio playback when starting the game
    private IEnumerator startButtonDelay()
    {
        if (!audioPlayed)
        {
            audioPlayed = true;
            menuAudio.PlayOneShot(playButtonClip, 1f);

            yield return new WaitForSeconds(0.75f);

            // Deactivate menu elements and load the game scene
            characterA.SetActive(false);
            characterB.SetActive(false);
            backgroundImage.gameObject.SetActive(false);
            SceneManager.LoadScene(1);

            yield return new WaitForSeconds(1f);

            audioPlayed = false;
        }

    }

    // Method to enter the menu screen
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

    // Method to exit the menu screen
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

    // Method to enter the how-to-play screen
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

    // Method to close the how-to-play screen
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

    // Method to exit the game
    public void ExitGame()
    {
        StartCoroutine(ExitGameDelay());
    }

    // Coroutine to handle delay and audio playback when exiting the game
    public IEnumerator ExitGameDelay()
    {
        menuAudio.PlayOneShot(exitButtonClip, 1f);

        yield return new WaitForSeconds(0.75f);

#if UNITY_EDITOR
        // Stop play mode in the Unity editor
        UnityEditor.EditorApplication.isPlaying = false;

#else
        // Quit the application
        Application.Quit();

#endif
    }
}
