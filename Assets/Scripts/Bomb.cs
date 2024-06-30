using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            BulletInstantiate();
            Debug.Log("Collided with Ground");
            playerControllerScript.bombPlanted = false;
        }
    }

    public void BulletInstantiate()
    {
        Vector3 newPos = new Vector3(transform.position.x, 2, transform.position.z);

        InstantiateBombBullet(newPos, Vector3.back);
        InstantiateBombBullet(newPos, Vector3.forward);
        InstantiateBombBullet(newPos, Vector3.left);
        InstantiateBombBullet(newPos, Vector3.right);

        GameObject explossion = Instantiate(explosionParticlePrefab, newPos, Quaternion.identity);
        Destroy(gameObject, 0.1f);

        Destroy(explossion, 2f);
    }

    void InstantiateBombBullet(Vector3 position, Vector3 direction)
    {
        GameObject bombBullet = Instantiate(bulletPrefab, position, Quaternion.identity);

        Rigidbody bulletRb = bombBullet.GetComponent<Rigidbody>();
        bulletRb.velocity = direction * bombBulletSpeed;

        Destroy(bombBullet, 0.2f);
    }
}
