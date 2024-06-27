using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterA : MonoBehaviour
{
    public Transform[] wayPoints;
    public float speed = 2f;
    private int currentWayPointIndex = 0;
    public bool isWaiting;
    public float rotationSpeed = 2.5f;
    public Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = wayPoints[currentWayPointIndex].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (wayPoints.Length == 0 || isWaiting) return;

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        // Move towards the current waypoint
        Vector3 targetPosition = wayPoints[currentWayPointIndex].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        playerAnimator.SetBool("isWalking", true);

        // Rotate towards the current waypoint
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // Check if the character reached the waypoint
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        playerAnimator.SetBool("isWalking", false);

        // Rotate 180 degrees around y-axis to look behind
        Quaternion lookBackRotation = Quaternion.LookRotation(-transform.forward);
        float timeElapsed = 0f;
        float rotateDuration = 1.5f; // Adjust as needed

        while (timeElapsed < rotateDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookBackRotation, timeElapsed / rotateDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2.5f);

        playerAnimator.SetBool("isWalking", true);

        currentWayPointIndex = (currentWayPointIndex + 1) % wayPoints.Length; // Move to the next waypoint
        isWaiting = false;
    }
}
