using System.Collections;
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

    // Method called when the script instance is being loaded
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Method called once per frame
    private void Update()
    {
        if (playerControllerScript.isGameActive)
        {
            // Check if the player has collected all hints
            if (playerControllerScript.currentHintCount == playerControllerScript.totalHintCount)
            {
                if (audioCoroutine == null)
                {
                    audioCoroutine = StartCoroutine(PlayAudioRepeatedly());
                }


                turnerButton.SetActive(true);
                turnerButtonImage.gameObject.SetActive(true);
            }

            // Check if the button has been pushed
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

        else
        {
            StopAllCoroutines();
            turnerButtonImage.gameObject.SetActive(false);
        }



    }

    // Coroutine to play audio repeatedly until the button is pushed
    private IEnumerator PlayAudioRepeatedly()
    {
        while (!turnerButtonScript.hasPushedButton)
        {
            audioSource.PlayOneShot(turnerButtonClip, 1f);
            yield return new WaitForSeconds(turnerButtonClip.length);
        }
    }

    // Method to stop the repeating audio coroutine
    private void StopRepeatingAudio()
    {
        if (audioCoroutine != null)
        {
            StopCoroutine(audioCoroutine);
            audioCoroutine = null;
        }
    }
}
