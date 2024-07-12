using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TurnerObject : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip turnerButtonClip;
    private Animator animator;
    public TurnerButton turnerButtonScript;
    public GameObject turnerButton;
    public Image turnerButtonImage;
    public PlayerController playerControllerScript;
    private Coroutine audioCoroutine;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerControllerScript.currentHintCount == playerControllerScript.totalHintCount)
        {
            if (audioCoroutine == null)
            {
                audioCoroutine = StartCoroutine(PlayAudioRepeatedly());
            }


            turnerButton.SetActive(true);
            turnerButtonImage.gameObject.SetActive(true);
        }

        if (turnerButtonScript.hasPushedButton)
        {
            StopRepeatingAudio();
            turnerButtonImage.gameObject.SetActive(false);
            animator.SetBool("isRotating", false);
            //gameObject.layer = 10;
        }

        else
        {
            animator.SetBool("isRotating", true);
        }
    }

    private IEnumerator PlayAudioRepeatedly()
    {
        while (!turnerButtonScript.hasPushedButton)
        {
            audioSource.PlayOneShot(turnerButtonClip, 1f);
            yield return new WaitForSeconds(turnerButtonClip.length);
        }
    }

    private void StopRepeatingAudio()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }
    }
}
