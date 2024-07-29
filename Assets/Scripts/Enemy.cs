using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public AudioSource enemyAudio;
    public AudioClip enemyDeathClip;
    public Animator enemyAnimator;
    public Transform[] waypoints;
    public Transform player;
    public GameObject enemyExplosionParticle;
    public float patrolSpeed = 2.0f;
    private NavMeshAgent agent;
    private Rigidbody enemyRb;
    public bool isCollidingWithEnemy;
    public bool isPlayerVisible;
    public bool isCollidedWithPlayer;
    public bool isAlive = true;

    public PlayerCamera playerCameraScript;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        agent.speed = patrolSpeed;
        GoToRandomWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the enemy is alive before performing any actions
        if (!isAlive) return;

        // Check if PlayerCamera is moving the camera
        if (playerCameraScript.isMovingCamera || isCollidedWithPlayer)
        {
            // Pause enemy movement
            agent.isStopped = true;
            enemyAnimator.SetBool("isRunning", false);
        }

        else if (!playerCameraScript.isMovingCamera || !isCollidedWithPlayer)
        {
            // Pause enemy movement
            agent.isStopped = false;
            enemyAnimator.SetBool("isRunning", true);

            if (isPlayerVisible)
            {
                ChasePlayer();
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    GoToRandomWaypoint();
                }

                if (isCollidingWithEnemy)
                {
                    SwitchWaypointOnCollision();
                }
            }
        }
    }

    // Method to move the enemy to a random waypoint
    void GoToRandomWaypoint()
    {
        if (waypoints.Length == 0)
            return;

        enemyAnimator.SetBool("isRunning", true);

        int randomIndex = Random.Range(0, waypoints.Length);
        agent.destination = waypoints[randomIndex].position;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isCollidingWithEnemy = true;
            Debug.Log("Collided with Enemy");
        }

        if (other.gameObject.CompareTag("Player"))
        {
            isCollidedWithPlayer = true;
            gameObject.layer = 10;
        }

        if (other.gameObject.CompareTag("Bullet"))
        {
            // Handle collision with a bullet (e.g., play death sound, destroy enemy)
            Debug.Log("Collided with Bullet");
            StartCoroutine(ActivateEnemyExplosionParticle());
            enemyAudio.PlayOneShot(enemyDeathClip, 1f);
            gameObject.layer = 10;
            isAlive = false; // Mark the enemy as dead
            Destroy(other.gameObject, 0.03f);
            enemyAnimator.SetTrigger("isDead");
            agent.isStopped = true;
            Destroy(gameObject, 0.5f);
        }
    }

    // Coroutine to activate the enemy explosion particle effect
    IEnumerator ActivateEnemyExplosionParticle()
    {
        yield return new WaitForSeconds(0.1f);
        Vector3 newPos = transform.position;

        newPos.y += 1.5f;

        GameObject explosion = Instantiate(enemyExplosionParticle, newPos, Quaternion.identity);
        Destroy(explosion, 1.25f);
    }

    // Method to switch to a new waypoint when colliding with another enemy
    void SwitchWaypointOnCollision()
    {
        isCollidingWithEnemy = false; // Reset collision flag
        GoToRandomWaypoint(); // Go to a new random waypoint
    }

    // Method to chase the player
    void ChasePlayer()
    {
        enemyAnimator.SetBool("isRunning", true);
        agent.destination = player.position;
    }

    void FixedUpdate()
    {
        // Perform raycast to check if player is visible
        RaycastHit hit;
        Vector3 direction = player.position - transform.position;

        // Visualize the raycast in the Scene view
        Debug.DrawRay(transform.position, direction, Color.green);

        // Perform the raycast with a maximum distance of 5 units
        if (Physics.Raycast(transform.position, direction, out hit, 15f))
        {
            if (hit.transform.CompareTag("Player"))
            {
                isPlayerVisible = true;
            }
            else
            {
                isPlayerVisible = false;
            }
        }
        else
        {
            isPlayerVisible = false;
            // Debug.Log("Raycast did not hit anything.");
        }
    }
}
