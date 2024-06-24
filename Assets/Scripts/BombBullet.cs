using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BombBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("Brick"))
        {
            Destroy(gameObject);
            Destroy(other.gameObject);

            // Update the NavMesh to make the area walkable
            StartCoroutine(UpdateNavMesh());
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
