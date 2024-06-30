using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float bombBulletSpeed = 10f;
    public float bombFlyForce = 25f;
    private float horizontalInput;
    private float verticalInput;
    public float moveSpeed = 10f;
    public float rotateSpeed = 500f;
    public float rotateDuration = 3f;
    public int keyCount = 0;
    public int currentScore;
    public AudioSource playerAudio;
    public AudioClip bombSpawnClip;
    public AudioClip keyCollectClip;
    public AudioClip coinCollectClip;
    public AudioClip playerDeathClip;
    public AudioClip portalOpenClip;
    public AudioClip portalEntryClip;
    private Vector3 moveDirection;
    public Animator playerAnimator;
    public GameObject bombPrefab;
    public GameObject explosionParticlePrefab;
    public GameObject bombBulletPrefab;
    public GameObject portal;
    public bool bombPlanted;
    public bool keyObtained;
    public bool isGameActive;
    public bool enteredPortal = false;
    public PlayerCamera playerCameraScript;
    public Rigidbody playerRb;
    public NavMeshAgent playerNavMesh;
    public ParticleSystem[] dustTrail;
    public SceneTransition sceneTransitionScript;
    public Coin coinScript;
    public TMP_Text scoreText;
    public GameManager gameManagerScript;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        isGameActive = true;
        bombPlanted = false;
        keyObtained = false;
        keyCount = 0;
        currentScore = 0;
        scoreText.text = "Score: " + currentScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && !bombPlanted && !enteredPortal && !playerCameraScript.isMovingCamera && isGameActive)
        {
            PlantTheBomb();
            playerAudio.PlayOneShot(bombSpawnClip, 0.5f);
        }
    }

    private void MovePlayer()
    {
        if (!playerCameraScript.isMovingCamera && isGameActive && !enteredPortal)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Determine the movement direction
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Check if there's any input (if moveDirection is not zero vector)
            if (moveDirection != Vector3.zero)
            {

                moveDirection = moveDirection.normalized;

                // Rotate player to face the movement direction (optional)
                transform.rotation = Quaternion.LookRotation(moveDirection);

                // Move the player in the desired direction
                transform.position += moveDirection * moveSpeed * Time.deltaTime;


                // Trigger walking animation (if you have an Animator component attached)
                if (playerAnimator != null)
                {
                    playerAnimator.SetBool("isWalking", true);
                    dustTrail[0].Play();
                    dustTrail[1].Play();
                }
            }
            else
            {
                // No input, stop walking animation
                if (playerAnimator != null)
                {
                    playerAnimator.SetBool("isWalking", false);
                    dustTrail[0].Stop();
                    dustTrail[1].Stop();
                }
            }
        }

        else
        {
            // No input, stop walking animation
            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isWalking", false);
                dustTrail[0].Stop();
                dustTrail[1].Stop();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Portal"))
        {
            enteredPortal = true;
            playerAudio.PlayOneShot(portalEntryClip, 1f);
            Debug.Log("Entered Portal");
            StartCoroutine(RotateOnPortalEnter());
            StartCoroutine(sceneTransitionScript.EndScreenTransition());
            playerCameraScript.MoveCameraToPosition(transform.position + new Vector3(0, 5, 0));
        }

        if (other.gameObject.CompareTag("Key"))
        {
            keyObtained = true;
            playerAudio.PlayOneShot(keyCollectClip, 1f);
            Destroy(other.gameObject, 1f);
            keyCount += 1;
            Debug.Log("Key Obtained: " + keyCount);
            StartCoroutine(ActivatePortal());
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            isGameActive = false;
            playerAudio.PlayOneShot(playerDeathClip, 1f);
            playerNavMesh.enabled = false;
            Debug.Log("Collded with Enemy");
            StartCoroutine(ReactToEnemyCollide());
        }

        if (other.gameObject.CompareTag("Coin"))
        {
            playerAudio.PlayOneShot(coinCollectClip, 1f);
            currentScore += coinScript.score;
            scoreText.text = "Score: " + currentScore.ToString();
            Debug.Log("Current Score: " + currentScore);
            Destroy(other.gameObject, 1f);
        }
    }
    private void PlantTheBomb()
    {
        bombPlanted = true;
        GameObject spawnedBomb = Instantiate(bombPrefab, transform.position, transform.rotation);

        Rigidbody spawnedBombRb = spawnedBomb.GetComponent<Rigidbody>();

        spawnedBombRb.AddForce(Vector3.up * bombFlyForce, ForceMode.Impulse);
    }

    private IEnumerator ActivatePortal()
    {
        if (keyCount == 3)
        {
            playerAudio.PlayOneShot(portalOpenClip, 1f);
            // Trigger camera movement
            if (playerCameraScript != null)
            {
                playerCameraScript.MoveCameraToPosition(new Vector3(0f, 30f, 15f));
            }

            yield return new WaitForSeconds(2f);

            portal.SetActive(true);
        }
    }

    private IEnumerator ReactToEnemyCollide()
    {
        // Add a force to push back the player
        float jumpForce = 300f;
        playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        float elapsedTime = 0f;

        while (elapsedTime < rotateDuration)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait until the next frame
        }

        // Ensure exact rotation to 0 degrees after the loop
        transform.rotation = Quaternion.Euler(0, 180, 0);

        yield return new WaitForSeconds(1f);

        sceneTransitionScript.transitionAnim.SetTrigger("end");

        yield return new WaitForSeconds(1.75f);

        gameManagerScript.RestartMenu();
    }

    private void FixedUpdate()
    {
        // Clamp the y position of the player to not exceed 350
        Vector3 position = playerRb.position;
        if (position.y > 365f)
        {
            position.y = 365f;
            playerRb.position = position;
        }
    }

    private IEnumerator RotateOnPortalEnter()
    {
        float jumpDistance = 5f;
        Vector3 originalPosition = transform.position;
        Vector3 newPosition = originalPosition + transform.forward * jumpDistance;

        float elapsedTime = 2f;

        while (enteredPortal)
        {
            // Linearly interpolate position to create a smooth jump effect
            transform.position = Vector3.Lerp(originalPosition, newPosition, elapsedTime / 5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player is exactly at the new position at the end
        transform.position = newPosition;

    }
}
