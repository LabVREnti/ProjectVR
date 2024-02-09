using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject fire;
    float timer = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12) //Torch
        {
            setFire(true);
        }
    }
    private void Update()
    {
        
    }

    void setFire(bool condition)
    {
        fire.SetActive(condition);
    }
}
