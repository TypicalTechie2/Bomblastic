using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnerButton : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip switchOnClip;
    private Animator animator;
    public bool hasPushedButton = false;
    private PlayerCamera playerCameraScript;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCameraScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            if (!hasPushedButton)
            {
                hasPushedButton = true;
                audioSource.PlayOneShot(switchOnClip, 1f);
                animator.SetTrigger("hasPushedButton");
                playerCameraScript.MoveCameraToPosition(new Vector3(0, 25, -15));
            }

        }
    }
}
