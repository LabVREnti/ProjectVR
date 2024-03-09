using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingRocksTrigger : MonoBehaviour
{
    [SerializeField] GameObject platform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            platform.gameObject.SetActive(false);
        }
    }
}
