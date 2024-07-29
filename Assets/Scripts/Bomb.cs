using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bombBulletSpeed = 10f;
    public GameObject explosionParticlePrefab;
    private PlayerController playerControllerScript;

    private void Awake()
    {
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            // Trigger bullet instantiation and log the collision with the ground
            BulletInstantiate();
            Debug.Log("Collided with Ground");
            playerControllerScript.bombPlanted = false;
        }
    }

    // Instantiate bullets in four directions and create an explosion effect
    public void BulletInstantiate()
    {
        Vector3 newPos = new Vector3(transform.position.x, 2, transform.position.z);

        InstantiateBombBullet(newPos, Vector3.back);
        InstantiateBombBullet(newPos, Vector3.forward);
        InstantiateBombBullet(newPos, Vector3.left);
        InstantiateBombBullet(newPos, Vector3.right);

        // Instantiate and destroy the explosion particle effect
        GameObject explossion = Instantiate(explosionParticlePrefab, newPos, Quaternion.identity);
        Destroy(gameObject, 0.1f);

        Destroy(explossion, 2f);
    }

    // Instantiate a single bomb bullet and set its velocity
    void InstantiateBombBullet(Vector3 position, Vector3 direction)
    {
        GameObject bombBullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        Rigidbody bulletRb = bombBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = direction * bombBulletSpeed;

        Destroy(bombBullet, 0.2f);
    }
}
