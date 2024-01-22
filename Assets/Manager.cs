using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public void SetRespawnObject(GameObject obj, Vector3 respawnPos)
    {
        obj.transform.position = respawnPos;
    }
}
