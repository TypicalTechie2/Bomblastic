using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Image menuImage;
    public Button playButton;
    public Button menuButton;
    public Button exitButton;
    public GameObject keys;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        SceneManager.LoadScene(1);
    }

    public void MenuScreen()
    {
        menuImage.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        menuButton.gameObject.SetActive(false);
        keys.SetActive(false);
    }

    public void ExitMenuScreen()
    {
        menuImage.gameObject.SetActive(false);
        playButton.gameObject.SetActive(true);
        exitButton.gameObject.SetActive(true);
        menuButton.gameObject.SetActive(true);
        keys.SetActive(true);
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
