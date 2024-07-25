using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public PlayerController playerControllerScript;
    public SpawnManager spawnManagerScript;
    public AudioSource audioSource;
    public AudioClip bulletSpawnClip;
    public Animator bossAnimator;
    public bool isOpen = false;
    public GameObject bossBulletPrefab;
    public float upwardForce = 1500f; // Force to move the bullet up
    public float lateralForce = 650f; // Force to spread the bullet
    private Coroutine toggleIsOpenCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        toggleIsOpenCoroutine = StartCoroutine(ToggleIsOpen());
    }

    // Update is called once per frame
    void Update()
    {
        // Stop the ToggleIsOpen coroutine if bossEyesDestroyedCount is 3
        if (spawnManagerScript.bossEyesDestroyedCount == 3 && toggleIsOpenCoroutine != null)
        {

            bossAnimator.SetBool("isDead", true);
            StopCoroutine(toggleIsOpenCoroutine);
            toggleIsOpenCoroutine = null;
        }
    }

    private IEnumerator ToggleIsOpen()
    {
        while (playerControllerScript.isGameActive)
        {
            yield return new WaitForSeconds(3f);

            isOpen = true;

            audioSource.PlayOneShot(bulletSpawnClip, 0.75f);

            bossAnimator.SetBool("isOpen", true);

            yield return new WaitForSeconds(0.5f);

            InstantiateBossBullets();

            yield return new WaitForSeconds(1f);

            bossAnimator.SetBool("isOpen", false);

            yield return new WaitForSeconds(4f);

            isOpen = true;
        }

        // Stop the coroutine if bossEyesDestroyedCount is 3
        if (spawnManagerScript.bossEyesDestroyedCount == 3)
        {
            yield break;
        }
    }

    private void InstantiateBossBullets()
    {
        int bulletCount = 25; // Number of boss bullets
        for (int i = 0; i < bulletCount; i++)
        {
            Vector3 newPos = new Vector3(transform.position.x, 1, transform.position.z);
            GameObject bullet = Instantiate(bossBulletPrefab, newPos, Quaternion.identity);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddForce(Vector3.up * upwardForce); // Apply upward force
                rb.AddForce(new Vector3(Random.Range(-lateralForce, lateralForce), 0, Random.Range(-lateralForce, lateralForce))); // Apply random lateral force
            }
        }
    }
}
