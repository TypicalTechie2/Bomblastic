using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject explosionPrefab;

    private void OnTriggerEnter(Collider other)
    {
        Vector3 newPos = new Vector3(transform.position.x, 1, transform.position.z);

        GameObject explosions = Instantiate(explosionPrefab, newPos, Quaternion.identity);

        Destroy(explosions, 1f);
        Destroy(gameObject);
    }
}
