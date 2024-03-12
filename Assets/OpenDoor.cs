using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OpenDoor : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Transform openPoint;
    [SerializeField] GameObject tpTrigger;

    bool openDoor = false;
    public void OpenDoorNexus()
    {
        openDoor = true;
    }

    private void Update()
    {
        if (openDoor)
        {
            door.transform.position = Vector3.MoveTowards(door.transform.position, openPoint.position, Time.deltaTime * 0.3f);
            tpTrigger.SetActive(true);
        }
    }
}
