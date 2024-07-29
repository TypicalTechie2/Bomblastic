using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    public float speed = 5f;
    public float torque = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // Apply an initial force to get the ball moving
        rb.AddForce(new Vector3(speed, 0, speed), ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Calculate a new direction based on the collision normal
        Vector3 newDirection = Vector3.Reflect(rb.velocity.normalized, collision.contacts[0].normal);
        rb.velocity = newDirection * speed;

        // Apply torque to make the ball roll realistically
        Vector3 torqueDirection = new Vector3(newDirection.z, 0, -newDirection.x);
        rb.AddTorque(torqueDirection * torque, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        // Keep the ball moving at a constant speed
        rb.velocity = rb.velocity.normalized * speed;

        // Apply continuous torque to keep it rolling
        Vector3 torqueDirection = new Vector3(rb.velocity.z, 0, -rb.velocity.x);
        rb.AddTorque(torqueDirection * torque * Time.deltaTime);
    }
}
