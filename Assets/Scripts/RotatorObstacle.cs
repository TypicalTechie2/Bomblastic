using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorObstacle : MonoBehaviour
{
    public PlayerCamera playerCameraScript;
    public Animator rotatorAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!playerCameraScript.isMovingCamera)
        {
            rotatorAnimator.enabled = true;
            rotatorAnimator.SetBool("isRotating", true);
        }

        else if (playerCameraScript.isMovingCamera)
        {
            rotatorAnimator.enabled = false;
            rotatorAnimator.SetBool("isRotating", false);
        }
    }
}
