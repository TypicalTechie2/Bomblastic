using UnityEngine;

public class PortalExit : MonoBehaviour
{
    public float rotationSpeed = 25f;

    // Update is called once per frame
    void Update()
    {
        // Rotate the portal exit object
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
