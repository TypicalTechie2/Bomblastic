using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public float speed = 100f;
    public float liftHeight = 5f; // Height to lift the key
    public float liftDuration = 0.3f; // Time to lift the key
    public float moveDuration = 0.3f; // Time to move towards the player

    private bool isMoving = false;
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving)
        {
            transform.Rotate(Vector3.up * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isMoving)
        {
            gameObject.layer = 10;
            playerTransform = other.transform;
            StartCoroutine(MoveKey());
        }
    }

    private IEnumerator MoveKey()
    {
        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 liftPosition = startPosition + Vector3.up * liftHeight;

        // Smoothly move the key up
        yield return StartCoroutine(SmoothMove(startPosition, liftPosition, liftDuration));

        Vector3 newPos = new Vector3(0, 1f, 0);

        // Smoothly move the key towards the player
        yield return StartCoroutine(SmoothMove(liftPosition, playerTransform.position + newPos, moveDuration));

        // Optionally, you can add logic here for what happens when the key reaches the player
        Destroy(gameObject); // For example, destroy the key
    }

    private IEnumerator SmoothMove(Vector3 start, Vector3 end, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = end;
    }
}
