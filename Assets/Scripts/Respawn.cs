using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public GameObject objectToRespawn; // Assign the prefab in the Inspector
    public float respawnDelay = 2.0f; // Adjust delay in seconds

    private void Start()
    {
        // Optional: Set the initial state of the object to inactive if desired
        // objectToRespawn.SetActive(false); 
    }

    private void OnDestroy()
    {
        StartCoroutine(RespawnObjectCoroutine());
    }

    private IEnumerator RespawnObjectCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);
        Instantiate(objectToRespawn, transform.position, transform.rotation);
    }
}
