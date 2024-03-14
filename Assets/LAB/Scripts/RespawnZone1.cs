using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnZone1 : MonoBehaviour
{
    [SerializeField] GameObject spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = spawnPoint.transform.position;  
        }   
    }
}
