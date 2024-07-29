using UnityEngine;

public class RotatorObstacle : MonoBehaviour
{
    public PlayerCamera playerCameraScript;
    public Animator rotatorAnimator;

    // Update is called once per frame
    void Update()
    {
        // Check if the camera is not moving
        if (!playerCameraScript.isMovingCamera)
        {
            rotatorAnimator.enabled = true; // Enable the Animator component
            rotatorAnimator.SetBool("isRotating", true); // Set the "isRotating" parameter to true
        }

        // Check if the camera is moving
        else if (playerCameraScript.isMovingCamera)
        {
            rotatorAnimator.enabled = false; // Disable the Animator component
            rotatorAnimator.SetBool("isRotating", false); // Set the "isRotating" parameter to false
        }
    }
}
