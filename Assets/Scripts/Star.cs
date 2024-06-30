using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public int score = 10;
    public float amplitude = 1.5f; // Amplitude of the sine wave
    public float frequency = 1.0f; // Frequency of the sine wave
    public float rotationSpeed = 45.0f; // Rotation speed around y-axis
    public float liftHeight = 2.0f; // Height to lift the coin
    public float liftDuration = 0.3f; // Time to lift the coin
    public float moveDuration = 0.5f; // Time to move towards the player

    private Vector3 initialPosition; // Initial position of the coin
    private Transform playerTransform;

    void Start()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        // Smooth up and down position transition using a sine wave
        Vector3 newPosition = initialPosition;
        newPosition.y += Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = newPosition;

        // Smooth rotation around y-axis
        transform.rotation = Quaternion.Euler(0f, Time.time * rotationSpeed, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.layer = 10; // Assuming layer change is necessary
            playerTransform = other.transform;
            StartCoroutine(MoveStar());
        }
    }

    private IEnumerator MoveStar()
    {
        // Disable collisions while moving towards the player
        GetComponent<Collider>().enabled = false;

        // Lift the coin upwards
        Vector3 startPosition = transform.position;
        Vector3 liftPosition = startPosition + Vector3.up * liftHeight;

        yield return StartCoroutine(SmoothMove(startPosition, liftPosition, liftDuration));

        // Move towards the player
        Vector3 endPosition = playerTransform.position + new Vector3(0, 1, 0);
        yield return StartCoroutine(SmoothMove(liftPosition, endPosition, moveDuration));

        // Destroy the coin after reaching the player
        Destroy(gameObject);
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
