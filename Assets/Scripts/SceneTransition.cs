using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnim;
    public float transitionTime = 1.5f;
    private int lastSceneIndex = 10;

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
            // Handle the case where the player is at the last scene
            // For example, you might load a victory screen or return to the main menu
            SceneManager.LoadScene("VictoryScreen"); // Replace with the actual name of your victory scene
        }
    }

    public IEnumerator StartScreenTransition()
    {
        transitionAnim.SetTrigger("start");

        yield return new WaitForSecondsRealtime(transitionTime);

        SceneManager.LoadScene(0);
    }
}
