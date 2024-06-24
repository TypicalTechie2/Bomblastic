using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public float bombBulletSpeed = 10f;
    private float horizontalInput;
    private float verticalInput;
    public float moveSpeed = 10f;
    public float rotateSpeed = 500f;
    public float rotateDuration = 3f;
    public int keyCount = 0;
    private Vector3 moveDirection;
    public Animator playerAnimator;
    public GameObject bombPrefab;
    public GameObject explosionParticle;
    public GameObject bombBulletPrefab;
    public GameObject portal;
    public bool bombPlanted;
    public bool keyObtained;
    public bool isGameActive;
    public PlayerCamera playerCameraScript;
    public Rigidbody playerRb;
    public NavMeshAgent playerNavMesh;
    public ParticleSystem[] dustTrail;

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
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && !bombPlanted)
        {
            StartCoroutine(PlantTheBomb());
        }
    }

    private void MovePlayer()
    {
        if (!playerCameraScript.isMovingCamera && isGameActive)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            // Determine the movement direction
            moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Check if there's any input (if moveDirection is not zero vector)
            if (moveDirection != Vector3.zero)
            {
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
            Debug.Log("Entered Portal");
        }

        if (other.gameObject.CompareTag("Key"))
        {
            keyObtained = true;
            Destroy(other.gameObject, 1f);
            keyCount += 1;
            Debug.Log("Key Obtained: " + keyCount);
            StartCoroutine(ActivatePortal());
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            isGameActive = false;
            playerNavMesh.enabled = false;
            Debug.Log("Collded with Enemy");
            StartCoroutine(ReactToEnemyCollide());
        }
    }
    IEnumerator PlantTheBomb()
    {
        bombPlanted = true;
        GameObject spawnedBomb = Instantiate(bombPrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(1.5f);

        Vector3 newPos = new Vector3(spawnedBomb.transform.position.x, 1, spawnedBomb.transform.position.z);

        InstantiateBombBullet(newPos, Vector3.back);
        InstantiateBombBullet(newPos, Vector3.forward);
        InstantiateBombBullet(newPos, Vector3.left);
        InstantiateBombBullet(newPos, Vector3.right);

        GameObject explossion = Instantiate(explosionParticle, newPos, Quaternion.identity);
        Destroy(spawnedBomb);
        bombPlanted = false;

        yield return new WaitForSeconds(2f);

        Destroy(explossion);
    }

    void InstantiateBombBullet(Vector3 position, Vector3 direction)
    {
        GameObject bombBullet = Instantiate(bombBulletPrefab, position, Quaternion.identity);

        Rigidbody bulletRb = bombBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = direction * bombBulletSpeed;

        Destroy(bombBullet, 0.5f);
    }

    private IEnumerator ActivatePortal()
    {
        if (keyCount == 3)
        {
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
        transform.rotation = Quaternion.identity;
    }
}
