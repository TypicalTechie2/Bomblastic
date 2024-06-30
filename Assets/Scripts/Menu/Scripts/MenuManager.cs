using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    private AudioManager audioManagerScript;
    public GameObject characterA;
    public GameObject characterB;
    public Image titleImage;
    public Image backgroundImage;
    public GameObject menuScreen;
    public Button playButton;
    public Button menuButton;
    public Button exitButton;
    public Button[] buttons;
    public GameObject testObj;
    private static bool isFirstLoad = true;
    public bool hasClickedOnStart = false;

    private void Awake()
    {
        audioManagerScript = FindObjectOfType<AudioManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isFirstLoad)
        {
            StartCoroutine(DisableButtonsForSeconds());
            isFirstLoad = false;
        }
        else
        {
            isFirstLoad = true;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DisableButtonsForSeconds()
    {
        // Disable all buttons
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(false);
        }

        // Wait for specified seconds
        yield return new WaitForSeconds(3);

        // Enable all buttons again
        foreach (Button button in buttons)
        {
            button.gameObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        hasClickedOnStart = true;
        audioManagerScript.PlayButtonAudio();


        characterA.SetActive(false);
        characterB.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void EnterMenuScreen()
    {
        audioManagerScript.ButtonOnClickAudio();
        titleImage.gameObject.SetActive(false);
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void ExitMenuScreen()
    {
        audioManagerScript.CloseButtonAudio();
        menuScreen.SetActive(false);
        titleImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

        Application.Quit();

#endif
    }
}
