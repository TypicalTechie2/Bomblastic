using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 playerOffset;

    private Vector3 originalPosition;
    public bool isMovingCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!isMovingCamera)
        {
            transform.position = playerTransform.position + playerOffset;
        }
    }

    public void MoveCameraToPosition(Vector3 targetPosition)
    {
        if (!isMovingCamera)
        {
            StartCoroutine(MoveCameraSmoothly(targetPosition));
        }
    }

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
