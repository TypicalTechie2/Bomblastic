using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BombBullet : MonoBehaviour
{
    private SpawnManager spawnManagerScript;
    public GameObject bossEyeExplosion;

    // Start is called before the first frame update
    void Start()
    {
        spawnManagerScript = FindObjectOfType<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Brick"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);

            // Update the NavMesh to make the area walkable
            StartCoroutine(UpdateNavMesh());
        }

        if (other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("BossEye"))
        {
            Vector3 newPos = new Vector3(other.transform.position.x, 1.5f, other.transform.position.z);
            GameObject explosions = Instantiate(bossEyeExplosion, newPos, Quaternion.identity);

            Destroy(gameObject);
            Destroy(other.gameObject);
            Destroy(explosions, 1.5f);
            Debug.Log("Destroyed: " + other.gameObject.name);

            // Notify the SpawnManager that a bossEye has been destroyed
            spawnManagerScript.BossEyeDestroyed();
        }
    }

    IEnumerator UpdateNavMesh()
    {
        // Wait a frame to ensure the object is destroyed
        yield return null;

        // Collect all NavMesh data in the scene and update it
        NavMeshSurface surface = FindObjectOfType<NavMeshSurface>();
        if (surface != null)
        {
            surface.UpdateNavMesh(surface.navMeshData);
        }
    }
}
