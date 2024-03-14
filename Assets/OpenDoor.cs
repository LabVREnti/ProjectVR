using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] private GameObject door;
    [SerializeField] private Transform openPoint;
    [SerializeField] private GameObject tpTrigger;
    [SerializeField] private AudioSource doorSound;

    bool openDoor = false;

    public void OpenDoorNexus()
    {
        openDoor = true;
        if (!doorSound.isPlaying)
        {
            Invoke("OpenDoorSound", 0f);
        }
    }

    private void Update()
    {
        if (openDoor)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, openPoint.position, Time.deltaTime * 0.5f);
            tpTrigger.SetActive(true);
        }
    }

    private void OpenDoorSound()
    {
        doorSound.Play();
    }
}
