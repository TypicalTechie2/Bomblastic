using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public GameObject explosionPrefab;

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
        Vector3 newPos = new Vector3(transform.position.x, 1, transform.position.z);

        GameObject explosions = Instantiate(explosionPrefab, newPos, Quaternion.identity);

        Destroy(explosions, 1f);
        Destroy(gameObject);
    }
}
