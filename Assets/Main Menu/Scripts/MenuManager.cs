using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public MenuCharacter characterA;
    public MenuCharacter characterB;

    public AudioSource menuAudio;
    public AudioClip playButtonClip;

    // Start is called before the first frame update
    void Start()
    {
        CharactersBoundary();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CharactersBoundary()
    {
        characterA.minXBoundary = -8f;
        characterB.minXBoundary = -6.5f;

        characterA.maxXBoundary = 6.5f;
        characterB.maxXBoundary = 8f;
    }

    public void StartGame()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
        menuAudio.PlayOneShot(playButtonClip, 1f);

        yield return new WaitForSeconds(0.4f);

        SceneManager.LoadScene(1);
    }
}
