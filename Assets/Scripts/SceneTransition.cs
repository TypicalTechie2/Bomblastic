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
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartCoroutine(EndScreenTransition());
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            StartCoroutine(StartScreenTransition());
        }
    }

    public IEnumerator EndScreenTransition()
    {
        yield return new WaitForSeconds(2.5f);

        transitionAnim.SetTrigger("end");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(1);
    }

    public IEnumerator StartScreenTransition()
    {
        transitionAnim.SetTrigger("start");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(0);
    }
}
