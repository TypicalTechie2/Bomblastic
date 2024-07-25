using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip bombExplodeClip;
    public AudioClip blastSFX;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            playerAudio.PlayOneShot(bombExplodeClip, 1f);
        }

        if (other.gameObject.CompareTag("LandingMark"))
        {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Boss Bullet"))
        {
            playerAudio.PlayOneShot(blastSFX, 0.75f);
        }
    }
}
