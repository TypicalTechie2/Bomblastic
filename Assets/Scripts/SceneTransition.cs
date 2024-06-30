using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator transitionAnim;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator EndScreenTransition()
    {
        yield return new WaitForSecondsRealtime(2.5f);

        transitionAnim.SetTrigger("end");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(2);
    }

    public IEnumerator StartScreenTransition()
    {
        transitionAnim.SetTrigger("start");

        yield return new WaitForSecondsRealtime(1.5f);

        SceneManager.LoadScene(0);
    }
}
