using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class closeInitDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] private Vector3 end;

    [SerializeField] private float speed = 0.6f;

    private bool close = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           close = true;
        }
    }

    private void Update()
    {
        if(close)
        {
             door.transform.position = Vector3.MoveTowards(door.transform.position, end, speed * Time.deltaTime);
             Debug.Log("me estas hacienedo caso");
        }
    }
}
