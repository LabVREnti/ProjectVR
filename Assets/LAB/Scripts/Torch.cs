using Autohand;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] GameObject fire;
    [SerializeField] Transform holder;
    Grabbable grabbable;
    float timer = 0;
   [SerializeField] PlacePoint placePoint;

    private void Start()
    {
        grabbable = GetComponent<Grabbable>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Invoke("RespawnT", 3.0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12) //Torch
        {
            setFire(true);
        }
    }
    void RespawnT()
    {
        placePoint.Place(grabbable);
        
    }

    void setFire(bool condition)
    {
        fire.SetActive(condition);
    }
}
