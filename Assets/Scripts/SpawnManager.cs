using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip bossEyeHitClip;
    public AudioClip victoryClip;
    public AudioClip hintAudioClip;
    public PlayerController playerControllerScript;
    public PlayerCamera playerCameraScript;
    public BombBullet bombBulletScript;
    public GameObject[] bossEyes;
    public GameObject[] gateObjects;
    public GameObject[] keys;
    public GameObject bossObject;
    private Coroutine hintAudioCoroutine;
    public bool isGateMoving = false;
    public int bossEyesDestroyedCount;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ActivateBossEyes();
        ActivateGatesOnKeyCollect();
    }

    private void ActivateBossEyes()
    {
        if (playerControllerScript.currentHintCount == 4 && bossEyes[0] != null)
        {
            if (playerControllerScript.isGameActive)
            {
                bossEyes[0].SetActive(true);
                StartHintAudioLoop();
            }

            else
            {
                StopHintAudioLoop();
            }
        }

        if (playerControllerScript.currentHintCount == 6)
        {
            StartCoroutine(MoveGateAndDestroy(1));
        }

        if (playerControllerScript.currentHintCount == 8 && bossEyes[1] != null)
        {
            bossEyes[1].SetActive(true);
            StartHintAudioLoop();
        }
    }

    public void BossEyeDestroyed()
    {
        // Increment the count of destroyed bossEyes
        bossEyesDestroyedCount++;

        // Activate the corresponding key
        if (bossEyesDestroyedCount == 1)
        {
            audioSource.PlayOneShot(bossEyeHitClip, 1f);
            StopHintAudioLoop();

            if (keys.Length > 0)
            {
                keys[0].SetActive(true);
            }
        }
        else if (bossEyesDestroyedCount == 2)
        {
            audioSource.PlayOneShot(bossEyeHitClip, 1f);
            StopHintAudioLoop();

            if (keys.Length > 1)
            {
                keys[1].SetActive(true);
            }
        }
        else if (bossEyesDestroyedCount == 3)
        {
            audioSource.PlayOneShot(bossEyeHitClip, 1f);

            if (keys.Length > 2)
            {
                keys[2].SetActive(true);
            }
        }
        // Add more conditions if there are more bossEyes and keys
    }

    private IEnumerator PlayAudioRepeatedly()
    {
        while (true)
        {
            audioSource.PlayOneShot(hintAudioClip, 1f);
            yield return new WaitForSeconds(hintAudioClip.length);
        }
    }

    private void StartHintAudioLoop()
    {
        if (hintAudioCoroutine == null)
        {
            hintAudioCoroutine = StartCoroutine(PlayAudioRepeatedly());
        }
    }

    private void StopHintAudioLoop()
    {
        if (hintAudioCoroutine != null)
        {
            StopCoroutine(hintAudioCoroutine);
            hintAudioCoroutine = null;
        }
    }

    private void ActivateGatesOnKeyCollect()
    {
        if (playerControllerScript.keyCount == 1)
        {
            StartCoroutine(MoveGateAndDestroy(0));
        }

        if (playerControllerScript.keyCount == 2 && bossEyes[2] != null)
        {
            StartCoroutine(MoveGateAndDestroy(2));
            bossEyes[2].SetActive(true);
        }

        if (playerControllerScript.keyCount == 3 && bossObject != null)
        {
            Destroy(bossObject);
            StartCoroutine(MoveGateAndDestroy(3));
        }
    }

    private IEnumerator MoveGateAndDestroy(int gateIndex)
    {
        isGateMoving = true;

        if (gateObjects[gateIndex] != null)
        {
            Vector3 targetPosition = new Vector3(gateObjects[gateIndex].transform.position.x, -2.02f, gateObjects[gateIndex].transform.position.z);
            float duration = 2f;
            float elapsedTime = 0;

            Vector3 initialPosition = gateObjects[gateIndex].transform.position;

            while (elapsedTime < duration)
            {
                if (gateObjects[gateIndex] == null) yield break; // Stop coroutine if the object is destroyed
                gateObjects[gateIndex].transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            if (gateObjects[gateIndex] != null)
            {
                gateObjects[gateIndex].transform.position = targetPosition;
                Destroy(gateObjects[gateIndex]);
                gateObjects[gateIndex] = null; // Set the reference to null
            }
        }

        isGateMoving = false;
    }
}
