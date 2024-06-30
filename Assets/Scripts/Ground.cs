using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public AudioSource playerAudio;
    public AudioClip bombExplodeClip;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Bomb"))
        {
            playerAudio.PlayOneShot(bombExplodeClip, 1f);
        }
    }
}
