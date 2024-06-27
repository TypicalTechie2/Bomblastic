using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody coinRB;

    // Start is called before the first frame update
    void Start()
    {
        coinRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        coinRB.AddForce(Vector3.left * speed * Time.deltaTime);
    }
}
