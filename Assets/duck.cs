using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class duck : MonoBehaviour
{

    playerController player;

    private void Start()
    {
        player = FindAnyObjectByType<playerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Playear el sonido
            this.enabled = false;
        }
    }
}
