using System.Collections;
using UnityEngine;

public class MenuCharacter : MonoBehaviour
{
    public float moveSpeed = 3.0f; // Adjust speed as needed
    public float minXBoundary = -8f;
    public float maxXBoundary = 8f;
    public Animator animator;
    private bool movingRight = true;

    void Start()
    {
        StartCoroutine(MoveCharacter());
    }

    IEnumerator MoveCharacter()
    {
        while (true)
        {
            float targetX = movingRight ? maxXBoundary : minXBoundary;
            float currentX = transform.position.x;

            // Calculate move direction
            Vector3 moveDirection = (movingRight ? Vector3.right : Vector3.left);

            // Rotate player to face the movement direction (optional)
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }

            // Move towards the target X position
            while (Mathf.Abs(currentX - targetX) > 0.1f)
            {
                animator.SetBool("isWalking", true);
                transform.position += moveDirection * moveSpeed * Time.deltaTime;
                currentX = transform.position.x;
                yield return null;
            }

            animator.SetBool("isWalking", false);

            // Rotate character to specific angles based on boundary
            if (targetX == minXBoundary)
            {
                transform.rotation = Quaternion.Euler(0, 135, 0); // Rotate to 135 degrees on y-axis
            }
            else if (targetX == maxXBoundary)
            {
                transform.rotation = Quaternion.Euler(0, 225, 0); // Rotate to 225 degrees on y-axis
            }

            // Wait for 3 seconds at the boundary
            yield return new WaitForSeconds(3.0f);

            // Change direction
            movingRight = !movingRight;
        }
    }
}
