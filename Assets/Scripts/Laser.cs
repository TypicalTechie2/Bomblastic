using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float xBoundary = 46f;
    public float moveSpeed = 10f;
    private bool movingRight = true; // Flag to track movement direction

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Determine the target X position based on movement direction
        float targetX = movingRight ? xBoundary : -xBoundary;

        // Move towards the target X position
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);

        // Update the position with the new X value, keeping the original Y and Z values
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Check if reached the target boundary, then switch direction
        if (Mathf.Approximately(newX, targetX))
        {
            movingRight = !movingRight; // Toggle movement direction
        }
    }
}
