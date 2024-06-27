using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterB : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints
    public float moveSpeed = 5f; // Speed of movement
    public float rotationSpeed = 5f; // Speed of rotation
    private int currentWaypointIndex = 0;
    public Animator characterBAnimator;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = waypoints[currentWaypointIndex].position;
        transform.position += new Vector3(0, 0, -5); // Adjust position on z-axis
    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints.Length == 0)
            return;

        // Move towards the current waypoint
        Vector3 targetPosition = waypoints[currentWaypointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        characterBAnimator.SetBool("isWalking", true);

        // Rotate towards the movement direction
        if (transform.position != targetPosition)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetPosition - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Check if we have reached the current waypoint
        if (transform.position == targetPosition)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}
