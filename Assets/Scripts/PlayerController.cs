using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public int currentHintCount;
    public int totalHintCount = 4;
    public AudioSource playerAudio;
    public AudioClip playerHitClip;
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
    public Star starScript;
    public TMP_Text scoreText;
    public GameManager gameManagerScript;
    public Joystick joystick;
    public Button bombButton;
    public Image[] keysActivationImage;
    public SceneTransition sceneTransitionScript;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize game state
        isGameActive = true;
        bombPlanted = false;
        keyObtained = false;
        keyCount = 0;
        currentHintCount = 0;

        // Set initial score display
        scoreText.text = "Score: " + ScoreManager.instance.currentScore.ToString();

#if UNITY_EDITOR
        // Check if the active build target is Android
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
        {
            // Enable joystick, toggleImage, and BombButton Background Image
            joystick.gameObject.SetActive(true);
            gameManagerScript.toggleImage.gameObject.SetActive(true);
            gameManagerScript.BombButtonBackgroundImage.gameObject.SetActive(true);
        }
        else
        {
            // Disable joystick, toggleImage, and BombButton Background Image
            joystick.gameObject.SetActive(false);
            gameManagerScript.toggleImage.gameObject.SetActive(false);
            gameManagerScript.BombButtonBackgroundImage.gameObject.SetActive(false);
        }
#else
        // Enable joystick, toggleImage, and BombButton Background Image only on Android builds
        if (Application.platform == RuntimePlatform.Android)
        {
            joystick.gameObject.SetActive(true);
            gameManagerScript.toggleImage.gameObject.SetActive(true);
            gameManagerScript.BombButtonBackgroundImage.gameObject.SetActive(true);
        }
        else
        {
            joystick.gameObject.SetActive(false);
            gameManagerScript.toggleImage.gameObject.SetActive(false);
            gameManagerScript.BombButtonBackgroundImage.gameObject.SetActive(false);
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        // Handle player movement
        MovePlayer();

        // Handle bomb planting
        if (Input.GetKeyDown(KeyCode.Space) && !bombPlanted && !enteredPortal && !playerCameraScript.isMovingCamera && isGameActive)
        {
            PlantTheBomb();
            playerAudio.PlayOneShot(bombSpawnClip, 0.5f);
        }

        // Set player layer based on camera movement
        if (playerCameraScript.isMovingCamera)
        {
            gameObject.layer = 10;
        }

        else if (!playerCameraScript.isMovingCamera)
        {
            gameObject.layer = 9;
        }
    }

    private void FixedUpdate()
    {
        // Reset Rigidbody velocity if no input
        if (!playerCameraScript.isMovingCamera && isGameActive && !enteredPortal)
        {
            // Reset Rigidbody velocity and angular velocity if no input
            if (Mathf.Abs(joystick.Horizontal) < 0.1f && Mathf.Abs(joystick.Vertical) < 0.1f &&
                Mathf.Abs(Input.GetAxis("Horizontal")) < 0.1f && Mathf.Abs(Input.GetAxis("Vertical")) < 0.1f)
            {
                playerRb.velocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }
        }

        // Clamp the y position of the player to not exceed 350
        Vector3 position = playerRb.position;
        if (position.y > 365f)
        {
            position.y = 365f;
            playerRb.position = position;
        }
    }

    private void MovePlayer()
    {
        // Handle player movement and animations
        if (!playerCameraScript.isMovingCamera && isGameActive && !enteredPortal)
        {
            // Get joystick input
            float joystickHorizontal = joystick.Horizontal;
            float joystickVertical = joystick.Vertical;

            // Get keyboard input
            float keyboardHorizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
            float keyboardVertical = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

            // Combine inputs
            horizontalInput = joystickHorizontal + keyboardHorizontal;
            verticalInput = joystickVertical + keyboardVertical;

            // Apply dead zone to joystick input
            float deadZone = 0.1f; // Adjust this value as needed
            if (Mathf.Abs(horizontalInput) < deadZone) horizontalInput = 0f;
            if (Mathf.Abs(verticalInput) < deadZone) verticalInput = 0f;

            // Determine the movement direction
            moveDirection = new Vector3(horizontalInput, 0, verticalInput);

            // Normalize moveDirection only if there's any input (non-zero vector)
            if (moveDirection.magnitude > 1f)
            {
                moveDirection.Normalize();
            }

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

    // Method to handle button click
    public void OnBombButtonClicked()
    {
        if (!bombPlanted && !enteredPortal && !playerCameraScript.isMovingCamera && isGameActive)
        {
            PlantTheBomb();
            playerAudio.PlayOneShot(bombSpawnClip, 0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Handle player collision with portal or final reward
        if (other.gameObject.CompareTag("Portal") || other.gameObject.CompareTag("FinalReward"))
        {
            Debug.Log("Entered Portal");
            StartCoroutine(RotateOnPortalEnter());
            StartCoroutine(sceneTransitionScript.EndScreenTransition());
            playerCameraScript.MoveCameraToPosition(transform.position + new Vector3(0, 5, -0.5f));
            gameObject.layer = 10;
        }

        // Handle player collision with key
        if (other.gameObject.CompareTag("Key"))
        {
            keyObtained = true;
            playerAudio.PlayOneShot(keyCollectClip, 1f);
            Destroy(other.gameObject, 1f);
            keyCount += 1;
            Debug.Log("Key Obtained: " + keyCount);

            if (keyCount <= keysActivationImage.Length)
            {
                keysActivationImage[keyCount - 1].gameObject.SetActive(true);
            }
            StartCoroutine(ActivatePortal());
        }

        // Handle player collision with enemy, obstacle, or boss bullet
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Boss Bullet"))
        {
            isGameActive = false;
            playerAudio.PlayOneShot(playerHitClip, 1f);
            playerAudio.PlayOneShot(playerDeathClip, 0.3f);
            playerNavMesh.enabled = false;
            Debug.Log("Collded with: " + other.gameObject.name);

            StartCoroutine(ReactToEnemyCollide());
        }

        // Handle player collision with star
        if (other.gameObject.CompareTag("Star"))
        {
            playerAudio.PlayOneShot(coinCollectClip, 1f);
            ScoreManager.instance.currentScore += starScript.score;
            scoreText.text = "Score: " + ScoreManager.instance.currentScore.ToString();
            Debug.Log("Current Score: " + ScoreManager.instance.currentScore);
            Destroy(other.gameObject, 1f);
        }

        // Handle player collision with hint
        if (other.gameObject.CompareTag("Hint"))
        {
            currentHintCount += 1;
            Debug.Log("Current Hint Count: " + currentHintCount);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Handle collision with obstacle
        if (other.gameObject.CompareTag("Obstacle") && !playerCameraScript.isMovingCamera)
        {
            other.gameObject.layer = 10;
            isGameActive = false;
            playerAudio.PlayOneShot(playerHitClip, 1f);
            playerAudio.PlayOneShot(playerDeathClip, 0.3f);
            playerNavMesh.enabled = false;
            Debug.Log("Collded with Enemy");

            StartCoroutine(ReactToEnemyCollide());
        }
    }

    private void PlantTheBomb()
    {
        bombPlanted = true; // Set bomb planted flag to true
        GameObject spawnedBomb = Instantiate(bombPrefab, transform.position, transform.rotation);

        Rigidbody spawnedBombRb = spawnedBomb.GetComponent<Rigidbody>();

        spawnedBombRb.AddForce(Vector3.up * bombFlyForce, ForceMode.Impulse);
    }

    private IEnumerator ActivatePortal()
    {
        // Activate portal if enough keys are obtained
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

        yield return new WaitForSeconds(1f);

        gameManagerScript.ShowRestartMenu();
    }

    private IEnumerator RotateOnPortalEnter()
    {
        // Hide UI elements and joystick when entering the portal
        gameManagerScript.pauseButton.gameObject.SetActive(false);
        gameManagerScript.toggleImage.gameObject.SetActive(false);
        gameManagerScript.playerJoystick.gameObject.SetActive(false);
        gameManagerScript.BombButtonBackgroundImage.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        gameManagerScript.keyImages.SetActive(false);


        foreach (Image keyImg in keysActivationImage)
        {
            keyImg.gameObject.SetActive(false);
            yield return null; // Yield to the next frame
        }

        // Play portal entry sound only once
        if (!enteredPortal)
        {
            enteredPortal = true; // Ensure this flag is set correctly
            playerAudio.PlayOneShot(portalEntryClip, 1f);
        }

        float jumpDistance = 5f;
        Vector3 originalPosition = transform.position;
        Vector3 newPosition = originalPosition + transform.forward * jumpDistance;

        float elapsedTime = 0f;

        while (elapsedTime < 3f) // Adjust timing as needed
        {
            // Linearly interpolate position to create a smooth jump effect
            transform.position = Vector3.Lerp(originalPosition, newPosition, elapsedTime / 3f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the player is exactly at the new position at the end
        transform.position = newPosition;
    }
}
