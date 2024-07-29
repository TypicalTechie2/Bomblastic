using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 playerOffset;

    private Vector3 originalPosition;
    public bool isMovingCamera;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isMovingCamera)
        {
            transform.position = playerTransform.position + playerOffset;
        }
    }

    // Method to initiate a smooth camera movement to a specified position
    public void MoveCameraToPosition(Vector3 targetPosition)
    {
        // If the camera is not moving, update its position based on the player's position and offset
        if (!isMovingCamera)
        {
            StartCoroutine(MoveCameraSmoothly(targetPosition));
        }
    }

    // Coroutine to move the camera smoothly from its current position to the target position
    IEnumerator MoveCameraSmoothly(Vector3 targetPosition)
    {
        isMovingCamera = true;

        Vector3 initialPosition = transform.position;
        float elapsedTime = 0f;
        float moveDuration = 2f; // Adjust as needed

        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
        yield return new WaitForSeconds(3f);

        // Return back to original position
        elapsedTime = 0f;
        while (elapsedTime < moveDuration)
        {
            transform.position = Vector3.Lerp(targetPosition, playerTransform.position + playerOffset, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isMovingCamera = false;
    }

}
