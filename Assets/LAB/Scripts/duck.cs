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
       /* if (other.gameObject.CompareTag("Player"))
        {
            //Playear el sonido
            other.GetComponent<playerController>().AddDuck();
            this.enabled = false;
        }*/
    }

    public void GrabDuck()
    {
        //Playear el sonido
      
        this.gameObject.SetActive(false);
    }
}
