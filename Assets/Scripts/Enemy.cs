using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Animator enemyAnimator;
    public Transform[] waypoints;
    public Transform player;
    public float patrolSpeed = 2.0f;
    private NavMeshAgent agent;
    public bool isCollidingWithEnemy;
    public bool isPlayerVisible;
    public bool isCollidedWithPlayer;
    public bool isAlive = true;

    public PlayerCamera playerCameraScript;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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
        else
        {
            // Resume normal behavior
            agent.isStopped = false;

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
                    GoToRandomWaypoint();
                    isCollidingWithEnemy = false;
                }
            }
        }
    }

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
            Debug.Log("Collided with Bullet");
            isAlive = false; // Mark the enemy as dead
            Destroy(other.gameObject);
            gameObject.layer = 10;
            enemyAnimator.SetTrigger("isDead");
            agent.isStopped = true;
            Destroy(gameObject, 2f);
        }
    }

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
