using UnityEngine;

public class TurnerButton : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip switchOnClip;
    private Animator animator;
    public bool hasPushedButton = false;
    private PlayerCamera playerCameraScript;

    // Method called when the script instance is being loaded
    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerCameraScript = GameObject.Find("Main Camera").GetComponent<PlayerCamera>();
    }

    // Method called when another collider enters the trigger collider
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
