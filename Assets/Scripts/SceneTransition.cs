using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public SpawnManager spawnManagerScript;
    public Image victoryImage;
    public Animator transitionAnim;
    public float transitionTime = 1.5f;
    private int lastSceneIndex = 8;

    // Coroutine to handle the end screen transition, loading the next scene or displaying the victory image
    public IEnumerator EndScreenTransition()
    {
        yield return new WaitForSecondsRealtime(2.25f);

        transitionAnim.SetTrigger("end");

        yield return new WaitForSeconds(transitionTime);

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextSceneIndex < lastSceneIndex)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            spawnManagerScript.audioSource.PlayOneShot(spawnManagerScript.victoryClip, 1f);

            yield return new WaitForSecondsRealtime(1.5f);

            Time.timeScale = 0;
            victoryImage.gameObject.SetActive(true);
        }
    }

    // Coroutine to handle the start screen transition, resetting to the main menu
    public IEnumerator StartScreenTransition()
    {
        transitionAnim.SetTrigger("start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(0);
    }
}
